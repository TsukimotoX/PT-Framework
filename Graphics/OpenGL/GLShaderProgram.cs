using OpenTK.Graphics.OpenGL;

namespace ProjectTerra.Framework.Graphics.OpenGL;

// DefaultShader is a default implementation of IShader using OpenGL
public class GLShaderProgram : IShaderProgram {
    private int programID;
    private string vertexSource;
    private string fragmentSource;

    public GLShaderProgram(string vertexSource, string fragmentSource) {
        this.vertexSource = AdjustPlatform(vertexSource, ShaderType.VertexShader);
        this.fragmentSource = AdjustPlatform(fragmentSource, ShaderType.FragmentShader);

        Compile();
    }

    ~GLShaderProgram() {
        GL.DeleteProgram(programID);
    }

    public string AdjustPlatform(string source, ShaderType shaderType) {
        bool isMobile = Host.platform == "Android" || Host.platform == "iOS";

        string version = isMobile ? "#version 300 es\n" : "#version 330 core\n";
        string precision = isMobile && shaderType == ShaderType.FragmentShader ? "precision mediump float;" : "";

        var lines = source.Split('\n');
        source = string.Join("\n", lines.Where(line => !line.StartsWith("#version") && !line.StartsWith("precision")));

        return $"{version}\n{precision}\n{source}";
    }

    public void Compile() {
        //Console.WriteLine("Vertex Shader Source:");
        //Console.WriteLine(vertexSource);
        int vertexShader = CompileShader(vertexSource, ShaderType.VertexShader);
        //Console.WriteLine("Fragment Shader Source:");
        //Console.WriteLine(fragmentSource);
        int fragmentShader = CompileShader(fragmentSource, ShaderType.FragmentShader);

        programID = GL.CreateProgram();
        GL.AttachShader(programID, vertexShader);
        GL.AttachShader(programID, fragmentShader);
        GL.LinkProgram(programID);

        GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out int linkStatus);
        if (linkStatus == 0) {
            string infoLog = GL.GetProgramInfoLog(programID);
            throw new Exception($"Shader linking failed: {infoLog}");
        }
        
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    private int CompileShader(string source, ShaderType shaderType) {
        int shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        // Проверка статуса компиляции
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int compileStatus);
        if (compileStatus == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"{shaderType} compilation failed: {infoLog}");
        }

        return shader;
    }

    public void Use(){
        GL.UseProgram(programID);
    }

    public int GetProgram() => programID;

    public static string builtinVertexShader() =>
        "#version 330 core\n" +
        "layout(location = 0) in vec3 aPos;\n" +
        "layout(location = 1) in vec4 aColor;\n" +
        "layout(location = 2) in vec2 aTexCoord;\n" +
        "out vec4 vertexColor;\n" +
        "out vec2 TexCoord;\n" +
        "uniform mat4 model;\n" +
        "uniform mat4 view;\n" +
        "uniform mat4 projection;\n" +
        "void main()\n" +
        "{\n" +
        "    gl_Position = projection * view * model * vec4(aPos, 1.0);\n" +
        "    vertexColor = aColor;\n" +
        "    TexCoord = aTexCoord;\n" +
        "}";

    public static string builtinFragmentShader() =>
        "#version 330 core\n" +
        "in vec4 vertexColor;\n" +
        "in vec2 TexCoord;\n" +
        "out vec4 FragColor;\n" +
        "uniform sampler2D texture0;\n" +
        "uniform bool useTexture;\n" +
        "void main()\n" +
        "{\n" +
        "    if (!useTexture) {\n" +
        "        FragColor = vertexColor;\n" +
        "        return;\n" +
        "    } else {\n" +
        "        FragColor = texture(texture0, TexCoord);\n" +
        "    }\n" +
        "}";

}
