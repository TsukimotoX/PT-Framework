using OpenTK.Mathematics;

namespace ProjectTerra.Framework.Components.SubComponents;

public class Transform : Component {
    public Vector3 position { get; private set; }
    public Vector3 rotation { get; private set; }
    public Vector3 scale { get; private set; }

    public Transform() {
        position = Vector3.Zero;
        rotation = Vector3.Zero;
        scale = Vector3.One;
    }

    public void Translate(Vector3 direction) => position += direction;

    public void Rotate(Vector3 direction) => rotation += direction;

    public void Scale(Vector3 scale) => this.scale += scale;
}