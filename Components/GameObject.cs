#pragma warning disable IDE1006
using ProjectTerra.Framework.Components.SubComponents;

namespace ProjectTerra.Framework.Components;

public abstract class GameObject {
    public string name { get; set; }
    private List<Component> _components { get; set; } = new();
    private GameObject? parent { get; set; }
    private List<GameObject> children { get; set; } = new();

    public Transform transform { get; set; }

    public GameObject(string name) {
        this.name = name;
        transform = AddComponent<Transform>();
    }

    public virtual void Update() {
        foreach (var component in _components) {
            component.Update();
        }
    }

    public virtual void Render() {
        foreach (var component in _components) {
            component.Render();
        }
    }

    public void AddChild(GameObject child) { if (child.parent != null) { child.parent.RemoveChild(child); } children.Add(child); child.parent = this; }
    public void RemoveChild(GameObject child) { children.Remove(child); child.parent = null; }

    public T AddComponent<T>() where T : Component, new() { var component = new T(); _components.Add(component); return component; }
    public void RemoveComponent<T>() where T : Component {var component = GetComponent<T>(); if (component != null) _components.Remove(component); }
    public T? GetComponent<T>() where T : Component { return _components.Find(component => component is T) as T; }
}