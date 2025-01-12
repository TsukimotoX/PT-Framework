using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Input;

public class Host
{
    private readonly IWindow _window;
    private readonly IRenderer _renderer;
    private readonly InputManager _inputManager;
    public static string? platform;

    public Host(IWindow window, IRenderer renderer){
        _window = window;
        _renderer = renderer;
        _inputManager = new InputManager();

        platform = CheckPlatform();
    }

    private static string CheckPlatform(){
        if (OperatingSystem.IsAndroid()) return "Android";
        if (OperatingSystem.IsIOS()) return "iOS";
        if (OperatingSystem.IsWindows()) return "Windows";
        if (OperatingSystem.IsLinux()) return "Linux";
        if (OperatingSystem.IsMacOS()) return "MacOS";
        return "Unknown";
    }

    public void Run(){
        if (platform == "Unknown" || platform == null) throw new Exception("Sorry, but platform you're running on is not supported.");

        _window.Initialize();
        _renderer.Initialize(_window);
        Start();

        while(true){
            _inputManager.Update();
            _renderer.Render();
            _window.Update();
        }
    }

    public void Start(){
        Console.WriteLine("The game is starting...");
    }

    public static void Quit() => Environment.Exit(0);
}
