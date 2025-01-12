#pragma warning disable CS8618

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SDL;

namespace ProjectTerra.Framework.Graphics;

public unsafe class Renderer : IRenderer
{
    private IWindow _window;
    private SDL_GLContextState* _glContext;

    public void Initialize(IWindow window) {
        _window = window;
        _glContext = SDL3.SDL_GL_CreateContext(window.getWindow());
        if (_glContext == null) { throw new Exception("Failed to create OpenGL context.");}
        GL.Viewport(0, 0, window.Size.width, window.Size.height);
        GL.ClearColor(Color4.Black);
    }

    public void Render() {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        SDL3.SDL_GL_SwapWindow(_window.getWindow());
    }
}

public interface IRenderer
{
    void Initialize(IWindow window);
    void Render();
}