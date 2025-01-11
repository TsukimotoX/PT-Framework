using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SDL;

public unsafe class Renderer : IRenderer
{
    private SDL_GLContextState* _glContext;

    public void Initialize(DefaultWindow window) {
        _glContext = SDL3.SDL_GL_CreateContext(window.getWindow());
        if (_glContext == null) { throw new Exception("Failed to create OpenGL context.");}
        GL.Viewport(0, 0, window.Size.width, window.Size.height);
        GL.ClearColor(Color4.Black);
    }

    public void Render() {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        
    }
}

public interface IRenderer
{
    void Initialize(DefaultWindow window);
    void Render();
}