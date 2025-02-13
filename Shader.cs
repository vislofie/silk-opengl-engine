using Silk.NET.OpenGL;

public class Shader
{
    public uint ProgramID { get; private set; }
    private GL _gl;

    public Shader(GL gl, string vertPath, string fragPath)
    {
        _gl = gl;
        uint vertexShader = LoadShader(vertPath, ShaderType.VertexShader);
        uint fragmentShader = LoadShader(fragPath, ShaderType.FragmentShader);

        ProgramID = _gl.CreateProgram();

        _gl.AttachShader(ProgramID, vertexShader);
        _gl.AttachShader(ProgramID, fragmentShader);

        _gl.LinkProgram(ProgramID);

        _gl.GetProgram(ProgramID, GLEnum.LinkStatus, out int pStatus);
        if (pStatus != (int) GLEnum.True)
        {
            throw new Exception("Failed to link program comprised of " + vertPath + " && " + fragPath + "\n" + _gl.GetProgramInfoLog(ProgramID));
        }

        _gl.DetachShader(ProgramID, vertexShader);
        _gl.DetachShader(ProgramID, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
    }

    private uint LoadShader(string path, ShaderType type)
    {
        string shaderText = File.ReadAllText(path);
        uint shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, shaderText);
        _gl.CompileShader(shader);
        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
        if (status != (int) GLEnum.True)
        {
            throw new Exception("Failed to compile shader " + path + " of type " + type.ToString() + "\n" + _gl.GetShaderInfoLog(shader));
        }

        return shader;
    }

    public void Use()
    {
        _gl.UseProgram(ProgramID);
    }
}