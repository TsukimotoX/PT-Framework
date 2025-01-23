using System.Numerics;
using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Input;
using ProjectTerra.Framework.Math;

public class Host
{
    private readonly IWindow _window;
    private readonly IRenderer _renderer;
    private readonly InputManager _inputManager;
    public static string platform = CheckPlatform();

    public Host(IWindow window, IRenderer renderer){
        _window = window;
        _renderer = renderer;
        _inputManager = new InputManager();
    }

    private static string CheckPlatform(){
        if (OperatingSystem.IsAndroid()) return "Android";
        if (OperatingSystem.IsIOS()) return "iOS";
        if (OperatingSystem.IsWindows()) return "Windows";
        if (OperatingSystem.IsLinux()) return "Linux";
        if (OperatingSystem.IsMacOS()) return "MacOS";

        Console.WriteLine("Platform detected as Unknown.");
        return "Unknown";
    }

    public void Run(){
        if (platform == "Unknown" || platform == null) throw new Exception("Sorry, but platform you're running on is not supported.");
        Console.WriteLine($"Platform: {platform}");

        _window.Initialize();
        _renderer.Initialize(_window);

        while(true){
            _window.Update();
            _renderer.Render();
            _inputManager.Update();
        }
    }

    public void Start(){
        Console.WriteLine("The game is starting...");
        Console.WriteLine("ProjectTerra version is " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        Console.WriteLine("ProjectTerra.Framework version is " + typeof(ProjectTerra.Framework.Input.InputManager).Assembly.GetName().Version);

        Vertex[] vertices = [
            new Vertex(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector2(0, 0)),
            new Vertex(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector2(1, 0)),
            new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, 1), new Vector2(1, 1)),
            new Vertex(new Vector3(0, 1, 0), new Vector3(1, 1, 1), new Vector2(0, 1))
        ];
    }

    public static void Quit() => Environment.Exit(0);
}
