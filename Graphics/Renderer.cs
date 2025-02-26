namespace ProjectTerra.Framework.Graphics;

public interface IRenderer
{
    public static Dictionary<string, Action> renderActions;

    void Initialize(IWindow window);
    void Render();
    void AddRenderAction(string name, Action action);
    void RemoveRenderAction(string name);
}
