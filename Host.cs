using JetBrains.Annotations;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Input;
using ProjectTerra.Framework.Maths;
using SDL;

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

        Start();

        while(true){
            var error = GL.GetError();
            if (error != ErrorCode.NoError) Console.WriteLine($"OpenGL Error: {error}");
            var sdlerror = SDL3.SDL_GetError();
            if (sdlerror != null && sdlerror != "") Console.WriteLine($"SDL Error: [{sdlerror}]");


            _window.Update();
            _renderer.Render();
            _inputManager.Update();
        }
    }

    public void Start(){

        Console.WriteLine("The game is starting...");
        Console.WriteLine("ProjectTerra version is " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        Console.WriteLine("ProjectTerra.Framework version is " + typeof(ProjectTerra.Framework.Input.InputManager).Assembly.GetName().Version);

        Vertex[] vertices = {
            new Vertex((0, 0, 0), (1, 0, 0, 1), (0, 0)), //0
            new Vertex((0, 1, 0), (0, 1, 0, 1), (0, 1)), //1
            new Vertex((1, 0, 0), (0, 0, 1, 1), (1, 0)), //2
            new Vertex((1, 1, 0), (1, 1, 0, 1), (1, 1))  //3
        };

        int[] indices = { 
            0, 1, 2, 
            1, 3, 2,
        };

        var buffer = new ShaderBuffer(vertices, indices);

        var shader = new DefaultShader(DefaultShader.builtinVertexShader(), DefaultShader.builtinFragmentShader());
        shader.Use();

        var texture = new Drawable("../PT-Framework/stone.png");
        texture.Use(TextureUnit.Texture0);

        int textureLocation = GL.GetUniformLocation(shader.GetProgram(), "texture0");
        GL.Uniform1(textureLocation, 0);
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, texture.GetHandle());
        
        _renderer.AddRenderAction("render", () => {
            buffer.Bind();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            buffer.Unbind();
            //Console.WriteLine("Rendering...");
        });

    }


    public static void Quit() => Environment.Exit(0);
}
