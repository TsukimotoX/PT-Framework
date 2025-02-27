using ProjectTerra.Framework.Components;

namespace ProjectTerra.Framework.SceneManagement;

public class Scene {
    public string name { get; set; } = "UnnamedScene";
    protected List<GameObject> _gameObjects = new List<GameObject>();

    public virtual void Load()
    {
        Console.WriteLine($"{name}: Loading the scene...");
        if (_gameObjects.Count == 0) return;
        
        Render();
    }

    public virtual void Update(float deltaTime)
    {
        if (_gameObjects.Count == 0) return;
        Console.WriteLine($"{name}: Updating the scene...");
        foreach (var gameobject in _gameObjects) {gameobject.Update();}
    }

    public virtual void Render()
    {
        if (_gameObjects.Count == 0) return;
        Console.WriteLine($"{name}: Rendering the scene...");
        foreach (var gameobject in _gameObjects) {gameobject.Render();}
    }

    public virtual void Unload()
    {
        Console.WriteLine($"{name}: Unloading the scene...");
        _gameObjects.Clear();
    }

    public void AddGameObject(GameObject gameObject) => _gameObjects.Add(gameObject);
    public void RemoveGameObject(GameObject gameObject) => _gameObjects.Remove(gameObject);
}
