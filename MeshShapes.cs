using System.Numerics;
using Silk.NET.OpenGL;
using Utility.Extensions;

public static class MeshShapes
{
    public static uint VBO { get; private set; }
    public static uint VAO { get; private set; }
    private static GL _gl;

    public static float[] CubeShape
    {
        get
        {
            return
            [ //  X      Y      Z      U    V
                -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
                 0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
                -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f, 1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f, 1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
                 0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
                 0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                 0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                 0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
                 0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
                 0.5f, -0.5f,  0.5f, 1.0f, 0.0f,
                -0.5f, -0.5f,  0.5f, 0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f,  0.5f, -0.5f, 0.0f, 1.0f,
                 0.5f,  0.5f, -0.5f, 1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f, 1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f, 0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f, 0.0f, 1.0f
            ];
        }
    }

    private static uint _brushVertexSize = 0;

    public static void SetContext(GL gl)
    {
        _gl = gl;
    }

    public static unsafe void SetBrush(float[] vertices)
    {
        VAO = _gl.GenVertexArray();
        _gl.BindVertexArray(VAO);

        const uint positionLoc = 0;
        _gl.EnableVertexAttribArray(positionLoc);
        _gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)0);
        const uint texCoordLoc = 1;
        _gl.EnableVertexAttribArray(texCoordLoc);
        _gl.VertexAttribPointer(texCoordLoc, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*)(3 * sizeof(float)));

        VBO = _gl.GenBuffer();

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
        _brushVertexSize = (uint)vertices.Length / 5;

        fixed (float* buf = vertices)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        }
    }

    public static void Paint(Camera camera, Vector3 position, Vector3 scale, Vector3 rotation, Shader shader)
    {
        Matrix4x4 modelMatrix = Matrix4x4.CreateRotationX(rotation.X) * Matrix4x4.CreateRotationY(rotation.Y) * Matrix4x4.CreateRotationZ(rotation.Z);
        Matrix4x4 viewMatrix = Matrix4x4.Identity;
        viewMatrix.Translation = position / scale;
        
        Vector3 cameraTarget = camera.Position + camera.QuatRotation.RotateVector3(new Vector3(0, 0, -1));
        viewMatrix *= Matrix4x4.CreateLookAt(camera.Position, cameraTarget, Vector3.UnitY);

        Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView(45 * MathF.PI / 180.0f, 800.0f / 600.0f, 0.01f, 100f);

        shader.SetUniform("model", modelMatrix);
        shader.SetUniform("view", viewMatrix);
        shader.SetUniform("proj", proj);

        _gl.DrawArrays(GLEnum.Triangles, 0, _brushVertexSize);
    }
}