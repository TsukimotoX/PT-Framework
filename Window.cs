using SDL;

public unsafe class DefaultWindow : IWindow
{
    private SDL_Window* _sdlWindow;

    public (int width, int height) Size { get; private set; } = (800, 600);
    public (int x, int y) Position { get; private set; } = (100, 100);
    public string Title { get; private set; } = "Default Window";

    public void Initialize()
    {
        SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO);
        _sdlWindow = SDL3.SDL_CreateWindow(Title, Size.width, Size.height, SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        SDL3.SDL_SetWindowPosition(_sdlWindow, Position.x, Position.y);
    }

    public void Update() => SDL3.SDL_GL_SwapWindow(_sdlWindow); //as for now

    ~DefaultWindow()
    {
        SDL3.SDL_DestroyWindow(_sdlWindow);
        SDL3.SDL_Quit();
    }

    public SDL_Window* getWindow() => _sdlWindow;
}

public interface IWindow
{
    (int width, int height) Size { get; }
    (int x, int y) Position { get; }
    string Title { get; }

    void Initialize();
    void Update();
}