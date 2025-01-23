using OpenTK.Graphics.OpenGL;

namespace ProjectTerra.Framework.Graphics;

public class OpenGLShader : IShader {
    private int programID;

    public OpenGLShader(string vertexSource, string fragmentSource){
        
    }

    public void Compile(){
    }

    public void Use(){}
}

// IShader is an interface for creating Shaders based on different graphics APIs, like OpenGL, Vulkan, DirectX and etc.
public interface IShader { 
    void Compile();
    void Use();
}
