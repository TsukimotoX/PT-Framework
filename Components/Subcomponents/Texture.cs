using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Graphics;

namespace ProjectTerra.Framework.Components.SubComponents;

public class Texture : Component {
    private IRenderable _drawable;

    public Texture(string path) {
        //_drawable = new Drawable(path);
    }

    public override void Render()
    {
        base.Render();
        Use();
    }

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        //_drawable.Use(unit);
    }
}