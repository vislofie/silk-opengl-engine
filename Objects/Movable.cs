using System.Numerics;
using Utility.Extensions;

public abstract class Movable : ITransform
{
    public Vector3 Position { get; set; }
    private Vector3 _rotation;
    public Vector3 Rotation
    {
        get
        {
            return _rotation;
        }
        set
        {
            _quatRotation = value.ToQuaternion();
            _rotation = value;
        }
    }
    private Quaternion _quatRotation = Quaternion.Identity;
    public Quaternion QuatRotation
    {
        get
        {
            return _quatRotation;
        }
        set
        {
            _rotation = value.ToEuler();
            _quatRotation = value;
        }
    }

    public Vector3 Scale { get; set; }

    public void Translate(Vector3 value)
    {
        Vector3 translation = QuatRotation.RotateVector3(value);
        Position += translation;
    }
}