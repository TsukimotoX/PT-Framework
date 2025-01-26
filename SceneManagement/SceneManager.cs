namespace ProjectTerra.Framework.SceneManagement;

#nullable disable

public class SceneManager
{
    private Scene _currentScene;

    public void ChangeScene(Scene scene){
        _currentScene?.Unload();
        _currentScene = scene;
        _currentScene.Load();
    }

    public void Update(float deltaTime) => _currentScene?.Update(deltaTime);
    public void Render() => _currentScene?.Render();
}