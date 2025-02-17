using System.Drawing;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using StbImageSharp;

class Program
{
    private static IWindow _window;

    private static Shader _shader;
    private static Scene _currentScene;

    private static GL _gl;
    private static uint _vao;
    private static uint _vbo;
    private static uint _ebo;
    private static uint _texture;
    private static Vector3[] _cubePositions = new Vector3[5];

    public static void Main(string[] args)
    {
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "Hello, window!"
        };

        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Run();
    }

    private static unsafe void OnLoad()
    {
        IInputContext input = _window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;

        _gl = _window.CreateOpenGL();
        _gl.Enable(EnableCap.DepthTest);

        _currentScene = new Scene();
        MeshShapes.SetContext(_gl);
        _shader = new Shader(_gl, "shader/shader.vert", "shader/shader.frag");
        _currentScene.AddCube(new Vector3(0, -0.5f, -3), Vector3.One, Vector3.Zero, _shader);
        _currentScene.AddCube(new Vector3(0, 1.5f, -5), Vector3.One, new Vector3(0.1f, -0.3f, 0.0f), _shader);

        _texture = _gl.GenTexture();
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        ImageResult result = ImageResult.FromMemory(File.ReadAllBytes("img/krol.jpg"), ColorComponents.RedGreenBlueAlpha);

        fixed (byte* ptr = result.Data)
        {
            _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)result.Width, (uint)result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
        }

        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)TextureWrapMode.Repeat);
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)TextureMinFilter.Nearest);
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)TextureMagFilter.Nearest);
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.Mipmap, (int)TextureMinFilter.Nearest);
        _gl.TexParameterI(GLEnum.Texture2D, GLEnum.Mipmap, (int)TextureMagFilter.Nearest);

        _gl.BindTexture(TextureTarget.Texture2D, 0);

        int samplerLoc = _gl.GetUniformLocation(_shader.ProgramID, "uTexture");
        _gl.Uniform1(samplerLoc, 0);
    }

    private static unsafe void OnRender(double deltaTime)
    {
        _gl.ClearColor(Color.CornflowerBlue);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);

        _currentScene.Render();
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }
}