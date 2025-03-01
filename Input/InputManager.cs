#pragma warning disable CS8602 // Thinks gamepad in the code can be null, which's not possible. Using this for now, better fix in the future.

using SDL;
using System.Numerics;

namespace ProjectTerra.Framework.Input;

// This is the main input manager, responsible for processing inputs on host from various connectable devices
public unsafe class InputManager {
    public List<IInputDevice> InputDevices { get; private set; } = new List<IInputDevice>();
    public IInputDevice? ActiveInputDevice { get; private set; }

    public InputManager() {
        if (Host.platform == "Windows" || Host.platform == "MacOS" || Host.platform == "Linux") {
            var keyboardMouse = new KeyboardMouseID();
            InputDevices.Add(keyboardMouse);
            Console.WriteLine("Keyboard and mouse detected.");
        }
        else if (Host.platform == "Android" || Host.platform == "iOS") {
            var touch = new TouchID();
            InputDevices.Add(touch);
            Console.WriteLine("Touch input detected.");
        }
        else {
            throw new NotSupportedException("Unsupported platform.");
        }

        Console.WriteLine($"Input devices: {string.Join(", ", InputDevices.Select(x => x.Name))}");

        if (InputDevices.Count > 0) {
            ActiveInputDevice = InputDevices[0];
        } else {
            Console.WriteLine("No input devices available for this platform. Please check your input settings.");
        }
    }

    public void Update() {
        foreach (var device in InputDevices) {
            device.Update();

            if (device.IsActive && ActiveInputDevice != device) {
                ActiveInputDevice = device;
                Console.WriteLine($"Input device changed: Now it's {device.Name}");
            }
        }

        SDL_Event e;
        while (SDL3.SDL_PollEvent(&e)) {
            switch (e.Type) {
                case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
                    var gamepad = new GamepadID();
                    InputDevices.Add(gamepad);
                    ActiveInputDevice = gamepad;
                    break;
            }
        }
    }

    // This is a function to help get active input device's methods. 
    // Example:
    // var keyboard = inputManager.GetActiveDevice<KeyboardMouseID>();
    // if (keyboard.IsKeyDown(SDL_Scancode.SDL_SCANCODE_SPACE)) {
    //     Console.WriteLine("Space key is pressed!");
    // }
    public T? GetActiveDevice<T>() where T : class, IInputDevice => ActiveInputDevice as T;
}

// Keyboard and Mouse input device
public unsafe class KeyboardMouseID : IInputDevice {
    public string Name => "Keyboard&Mouse";

    private readonly byte* keyStates;
    private SDL_MouseButtonFlags mouseState; // Mouse can only return 1 button at a time
    public (float x, float y) mousePos { get; private set; }

    public KeyboardMouseID() {
        int numkeys;
        float x, y;
        keyStates = (byte*)SDL3.SDL_GetKeyboardState(&numkeys);

        mouseState = SDL3.SDL_GetMouseState(&x, &y);
    }

    public void Update() {
        SDL_Event e;
        while (SDL3.SDL_PollEvent(&e)) {
            switch (e.Type) {
                case SDL_EventType.SDL_EVENT_QUIT:
                    Host.Quit();
                    break;
                case SDL_EventType.SDL_EVENT_KEY_DOWN:
                    keyStates[(int)e.key.scancode] = 1;
                    break;
                case SDL_EventType.SDL_EVENT_KEY_UP:
                    keyStates[(int)e.key.scancode] = 0;
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
                    mouseState |= (SDL_MouseButtonFlags)e.button.button;
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                    mouseState &= ~(SDL_MouseButtonFlags)e.button.button;
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                    mousePos = (e.motion.x, e.motion.y);
                    break;
            }
        }
    }

    public bool IsActive => IsMouseActive() || IsKeyboardActive(); // Active if either mouse or keyboard gets input

    // Keyboard interactions

    public bool IsKeyDown(SDL_Scancode key) => keyStates[(int)key] != 0; // Returns that it's down if key is pressed
    public bool IsKeyUp(SDL_Scancode key) => keyStates[(int)key] == 0; // Returns that it's up if key is not pressed

    public bool IsKeyComboDown(params SDL_Scancode[] keys) => keys.All(key => keyStates[(int)key] != 0); // Returns that it's down if all keys listed are pressed
    public bool IsKeyComboUp(params SDL_Scancode[] keys) => keys.All(key => keyStates[(int)key] == 0); // Returns that it's up if all keys listed are not pressed (idk if its needed?)

    // Mouse interactions

    public bool IsMouseDown(SDL_MouseButtonFlags button) => (mouseState & button) != 0;
    public bool IsMouseUp(SDL_MouseButtonFlags button) => (mouseState & button) == 0;
    public (float x, float y) GetMousePos() => mousePos;

    // Active or inactive junk code to check if keyboard or mouse are even active
    private bool IsMouseActive() {
        float x, y;
        SDL_MouseButtonFlags buttonInput = SDL3.SDL_GetMouseState(&x, &y);
        return (buttonInput & SDL_MouseButtonFlags.SDL_BUTTON_LMASK) != 0 ||  // I am making this check buttons instead of mouse
               (buttonInput & SDL_MouseButtonFlags.SDL_BUTTON_RMASK) != 0 ||  // movement, because it would be really frustrating
               (buttonInput & SDL_MouseButtonFlags.SDL_BUTTON_MMASK) != 0 ||  // to change back to any other input device when you
               (buttonInput & SDL_MouseButtonFlags.SDL_BUTTON_X1MASK) != 0 || // accidentally hit the table and mouse moves for example. xD
               (buttonInput & SDL_MouseButtonFlags.SDL_BUTTON_X2MASK) != 0;
    }

    private bool IsKeyboardActive() {
        int numKeys;
        SDLBool* keyInput = SDL3.SDL_GetKeyboardState(&numKeys);

        for (int i = 0; i < numKeys; i++) {
            if (keyInput[i] == true) return true; // if any key is pressed, then keyboard is active, return that it is!
        }

        return false; // nothing pressed, return false
    }
}

// Touch input device
public unsafe class TouchID : IInputDevice {
    public string Name => "Touch";
    private readonly Dictionary<long, (float x, float y)> activeTouches = new(); // List of active touches, or fingers on the screen, with their positions.

    public void Update() {
        SDL_Event e;
        while (SDL3.SDL_PollEvent(&e)) {
            switch (e.Type) {
                case SDL_EventType.SDL_EVENT_FINGER_DOWN:
                    activeTouches[(long)e.tfinger.fingerID] = (e.tfinger.x, e.tfinger.y);
                    break;
                case SDL_EventType.SDL_EVENT_FINGER_UP:
                    activeTouches.Remove((long)e.tfinger.fingerID);
                    break;
                case SDL_EventType.SDL_EVENT_FINGER_MOTION:
                    if (activeTouches.TryGetValue((long)e.tfinger.fingerID, out _)) {
                        activeTouches[(long)e.tfinger.fingerID] = (e.tfinger.x, e.tfinger.y);
                    }
                    break;
            }
        }
    }
    
    public bool IsActive => activeTouches.Count > 0;

    // Gets all active touches
    public IEnumerable<(long id, float x, float y)> GetActiveTouches() => activeTouches.Select(touch => (touch.Key, touch.Value.x, touch.Value.y));

    // Gets the position of a specific touch
    public (float x, float y)? GetTouchPos(long id) => activeTouches.TryGetValue(id, out var pos) ? pos : null;
}

// Gamepad input device
public unsafe class GamepadID : IInputDevice {
    public string Name => "Gamepad";
    private SDL_Gamepad* controller;
    private readonly Dictionary<SDL_GamepadButton, bool> buttonStates = new();
    private (float x, float y) joystickPos;
    private (float x, float y) pointerPos;

    public GamepadID() {
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_GAMEPAD)) throw new Exception("Failed to initialize SDL Gamepad.");
        if (!SDL3.SDL_HasGamepad()) return; // if no gamepad then we don't even bother creating it
        controller = SDL3.SDL_OpenGamepad(SDL3.SDL_GetGamepads()[0]);
    }

    ~GamepadID(){
        SDL3.SDL_CloseGamepad(controller);
        controller = null;
    }

    public void Update() {
        SDL_Event e;
        while (SDL3.SDL_PollEvent(&e)) {
            switch (e.Type) {
                case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN:
                    buttonStates[(SDL_GamepadButton)e.gbutton.button] = true;
                    break;
                case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP:
                    buttonStates[(SDL_GamepadButton)e.gbutton.button] = false;
                    break;
                case SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION:
                    switch((SDL_GamepadAxis)e.gaxis.axis) { // that's right baby im going full yanderedev mode
                        // left axis case blocks are for left stick, to move joystick (character), or inventory selection around.
                        case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTX:
                            joystickPos.x = NormalizeAxis(e.gaxis.value);
                            break;
                        case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTY:
                            joystickPos.y = NormalizeAxis(e.gaxis.value);
                            break;

                        // right axis case blocks are for right stick, to move mouse around.
                        case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHTX:
                            pointerPos.x = NormalizeAxis(e.gaxis.value);
                            break;
                        case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHTY:
                            pointerPos.y = NormalizeAxis(e.gaxis.value);
                            break;
                    }

                    break;
                
            }
        }
    }
    public bool IsActive => controller != null;

    public bool IsButtonDown(SDL_GamepadButton button) => buttonStates.TryGetValue(button, out var state) && state;
    public bool IsButtonUp(SDL_GamepadButton button) => buttonStates.TryGetValue(button, out var state) && !state;
    public bool IsButtonComboDown(params SDL_GamepadButton[] buttons) => buttons.All(button => IsButtonDown(button));
    
    // Joystick vibration.
    public void Rumble(float strength, float speed, uint durationMs) => SDL3.SDL_RumbleGamepad(controller, (ushort)(strength * 65535), (ushort)(speed * 65535), durationMs);

    // Use this to normalize values between -1 and 1, because SDL returns values between -32767 and 32767.
    // Trust me, it's easier to work with.
    float NormalizeAxis(int value, float deadZone = 0.2f) => MathF.Abs(value / 32767f) < deadZone ? 0 : value / 32767f;

    public (float x, float y) GetJoystickPos() => joystickPos;
    public (float x, float y) GetPointerPos() => pointerPos; 
}

// This is basically any device that can output input, that's on your device.
// Mouse, Keyboard, Gamepad, Touchscreen, etc
public interface IInputDevice {
    string Name { get; }
    void Update();
    bool IsActive { get; } // If the device suddently gives signal it changes input device for the manager (and game)
}