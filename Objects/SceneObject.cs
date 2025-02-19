public abstract class SceneObject : Movable
{
    public Shader Shader { get; set; }
    public abstract float[] Shape { get; }
}