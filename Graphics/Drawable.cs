using StbImageSharp;
using OpenTK.Graphics.OpenGL;

namespace ProjectTerra.Framework.Graphics;


// Drawable is a class for loading and using OpenGL textures. For using texture on GameObject, use Texture class.
public class Drawable {
    private int _handle;

    public Drawable(string path){
        var imageData = LoadTextureData(path, out int width, out int height);
        Console.WriteLine($"Width: {width}, Height: {height}");

        _handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _handle);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, imageData);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    ~Drawable(){
        GL.DeleteTexture(_handle);
    }

    private byte[] LoadTextureData(string path, out int width, out int height){
        using var stream = File.OpenRead(path);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        width = image.Width;
        height = image.Height;

        return image.Data;
    }

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }

    public int GetHandle() => _handle;
}