namespace ProjectTerra.Framework.Components.SubComponents;

public abstract class Component {
    public bool enabled { get; set; } = true;
    public virtual string name { get; set; } = "UnknownComponent";

    public Component() {
        Start();
    }
    public virtual void Start() {}

    public virtual void Update() {
        if (!enabled) return;
    }
    public virtual void Render() {}
}