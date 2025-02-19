using System.Numerics;

public class Cube : SceneObject
{
    public override float[] Shape => MeshShapes.CubeShape;

    public Cube(Vector3 position, Vector3 scale, Vector3 rotation, Shader shader)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
        Shader = shader;
    }
}