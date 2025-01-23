#pragma warning disable CS8618

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SDL;

namespace ProjectTerra.Framework.Graphics;

public unsafe class Renderer : IRenderer
{
    private IWindow _window;
    private SDL_GLContextState* _glContext;
    public Dictionary<string, Action> _renderActions = new Dictionary<string, Action>();

    public void Initialize(IWindow window) {
        _window = window;
        _glContext = SDL3.SDL_GL_CreateContext(window.getWindow());
        if (_glContext == null) { throw new Exception("Failed to create OpenGL context.");}
        GL.LoadBindings(new OpenGLBindings());
        GL.Viewport(0, 0, window.Size.width, window.Size.height);
        GL.ClearColor(Color4.Black);
        GL.Enable(EnableCap.DepthTest);
    }

    public void Render() {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        foreach (var action in _renderActions.Values) { action.Invoke(); }
        GL.ClearColor(Color4.SkyBlue);
    }
}

public interface IRenderer
{
    void Initialize(IWindow window);
    void Render();
}

public class OpenGLBindings : IBindingsContext {public IntPtr GetProcAddress(string procName) => SDL3.SDL_GL_GetProcAddress(procName);}