using System.Numerics;

namespace Utility.Extensions;

public static class Vector3Extensions
{
    public static Quaternion ToQuaternion(this Vector3 euler)
    {
        float cr = MathF.Cos(euler.X * 0.5f);
        float sr = MathF.Sin(euler.X * 0.5f);
        float cp = MathF.Cos(euler.Y * 0.5f);
        float sp = MathF.Sin(euler.Y * 0.5f);
        float cy = MathF.Cos(euler.Z * 0.5f);
        float sy = MathF.Cos(euler.Z * 0.5f);

        Quaternion quat = new Quaternion()
        {
            X = sr * cp * cy - cr * sp * sy,
            Y = cr * sp * cy + sr * cp * sy,
            Z = cr * cp * sy - sr * sp * cy,
            W = cr * cp * cy + sr * sp * sy
        };

        return quat;
    }
}