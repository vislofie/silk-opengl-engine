using System.Numerics;

public interface ITransform
{
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public Quaternion QuatRotation { get; set; }
    public Vector3 Scale { get; set; }

    /// <summary>
    /// Translates forward by value depending on the rotation
    /// </summary>
    /// <param name="value">value to translate forward by</param>
    public void Translate(Vector3 value);
}