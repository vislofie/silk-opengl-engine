using System.Numerics;

public abstract class SceneObject
{
    public abstract Vector3 Position { get; set; }
    public abstract Vector3 Rotation { get; set; }
    public abstract Vector3 Scale { get; set; }
    public abstract Shader Shader { get; set; }
    public abstract float[] Shape { get; }
}