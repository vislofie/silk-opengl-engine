using System.Numerics;

public class Cube : SceneObject
{
    public override Vector3 Position { get; set; }
    public override Vector3 Scale { get; set; }
    public override Vector3 Rotation { get; set; }
    public override Shader Shader { get; set; }
    public override float[] Shape => MeshShapes.CubeShape;

    public Cube(Vector3 position, Vector3 scale, Vector3 rotation, Shader shader)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
        Shader = shader;
    }
}