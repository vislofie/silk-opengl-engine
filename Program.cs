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

    private static GL _gl;
    private static uint _vao;
    private static uint _vbo;
    private static uint _ebo;
    private static uint _texture;

    private static float _angle = -75f;
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

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        const float textureScale = 0.75f;

        _cubePositions = new Vector3[]
        {
            new Vector3(0, -1.0f, -2.0f),
            new Vector3(0, 0.3f, -5.0f),
            new Vector3(0, 1.2f, -7.0f),
            new Vector3(0.5f, 1.0f, -2.0f),
            new Vector3(3.0f, 0.3f, -7.0f)
        };

        float[] vertices =
        {
        //     // aPosition       | aTexCoords
        //     1f,   1f, 0.0f,  1.0f, 0.0f,
        //     1f,  -1f, 0.0f,  1.0f, 1.0f,
        //    -1f,  -1f, 0.0f,  0.0f, 1.0f,
        //    -1f,   1f, 0.0f,  0.0f, 0.0f

           -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
-0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
-0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
-0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
-0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
-0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
-0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
-0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
-0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
-0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
-0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
-0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
-0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
-0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
-0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
-0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
-0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
-0.5f, 0.5f, -0.5f, 0.0f, 1.0f
        };

        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        fixed (float* buf = vertices)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }

        uint[] indices =
        {
            0u, 1u, 3u,
            1u, 2u, 3u
        };

        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

        fixed (uint* buf = indices)
        {
            _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);
        }
        
        _shader = new Shader(_gl, "shader/shader.vert", "shader/shader.frag");

        const uint positionLoc = 0;
        _gl.EnableVertexAttribArray(positionLoc);
        _gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)0);

        const uint texCoordLoc = 1;
        _gl.EnableVertexAttribArray(texCoordLoc);
        _gl.VertexAttribPointer(texCoordLoc, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)(3 * sizeof(float)));

        const uint transformLoc = 2;
        _gl.EnableVertexAttribArray(transformLoc);
        _gl.VertexAttribPointer(transformLoc, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)0);

        Matrix4x4 modelMatrix = Matrix4x4.CreateRotationX(-75 * MathF.PI / 180.0f);
        Matrix4x4 viewMatrix = Matrix4x4.Identity;
        viewMatrix.Translation = new Vector3(0, 0, -3.0f);
        Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView(45 * MathF.PI / 180.0f, 800.0f / 600.0f, 0.01f, 100f);

        _shader.SetUniform("model", modelMatrix);
        _shader.SetUniform("view", viewMatrix);
        _shader.SetUniform("proj", proj);

        _gl.BindVertexArray(0);
        _gl.BindBuffer(GLEnum.ArrayBuffer, 0);
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, 0);

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

        _gl.BindVertexArray(_vao);
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
        _shader.Use();

        _angle -= 3 * (float)deltaTime;
        Matrix4x4 model = Matrix4x4.CreateRotationX(_angle / 2) * Matrix4x4.CreateRotationY(_angle / 2);
        _shader.SetUniform("model", model);

        // _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
        for (int i = 0; i < _cubePositions.Length; i++)
        {
            Matrix4x4 transl = Matrix4x4.CreateTranslation(_cubePositions[i]);
            _shader.SetUniform("view", transl);
            _gl.DrawArrays(GLEnum.Triangles, 0, 36);
        }
        
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }
}