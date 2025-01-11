using System.Runtime.InteropServices;

public class Host
{
    private readonly IWindow _window;
    private readonly IRenderer _renderer;
    private readonly InputManager _inputManager;

    public Host(IWindow window, IRenderer renderer){
        _window = window;
        _renderer = renderer;
        _inputManager = new InputManager();
    }

    public void Run(){
        _window.Initialize();
        _renderer.Initialize();
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

    public void Quit() => Environment.Exit(0);
}
