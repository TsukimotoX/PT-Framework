#pragma warning disable CS8618

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SDL;

namespace ProjectTerra.Framework.Graphics;

public unsafe class DefaultRenderer : IRenderer
{
    private IWindow _window;
    private SDL_GLContextState* _glContext;
    public static Dictionary<string, Action> _renderActions = new Dictionary<string, Action>();

    public void Initialize(IWindow window) {
        _window = window;
        _glContext = SDL3.SDL_GL_CreateContext(window.getWindow());
        if (_glContext == null) { throw new Exception("Failed to create OpenGL context.");}
        GL.LoadBindings(new OpenGLBindings());
        GL.Viewport(0, 0, window.Size.width, window.Size.height);
        GL.ClearColor(Color4.SkyBlue);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public void Render() {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        foreach (var action in _renderActions.Values) action.Invoke();
        GL.Viewport(0, 0, _window.Size.width, _window.Size.height);
        //Console.WriteLine($"RenderActions count: {_renderActions.Count}");
    }

    public void AddRenderAction(string name, Action action) { _renderActions.Add(name, action); Console.WriteLine("Added render action: " + name); }
    public void RemoveRenderAction(string name) => _renderActions.Remove(name);
}

public interface IRenderer
{
    public static Dictionary<string, Action> renderActions;

    void Initialize(IWindow window);
    void Render();
    void AddRenderAction(string name, Action action);
    void RemoveRenderAction(string name);
}

public class OpenGLBindings : IBindingsContext {public IntPtr GetProcAddress(string procName) => SDL3.SDL_GL_GetProcAddress(procName);}