using System.Numerics;

namespace Utility.Extensions;

public static class QuaternionExtensions
{
    public static Vector3 RotateVector3(this Quaternion quaternion, Vector3 v)
    {
        Vector3 u = new Vector3 (quaternion.X, quaternion.Y, quaternion.Z);
        float s = quaternion.W;

        Vector3 value = 2.0f * Vector3.Dot(u, v) * u +
                        (s * s - Vector3.Dot(u, u)) * v +
                        2.0f * s * Vector3.Cross(u, v);

        return value;
    }

    public static Vector3 ToEuler(this Quaternion quaternion)
    {
        Vector3 euler = new Vector3();

        float sinr_cosp = 2 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
        float cosr_cosp = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);
        euler.X = MathF.Atan2(sinr_cosp, cosr_cosp);

        float sinp = MathF.Sqrt(1 + 2 * (quaternion.W * quaternion.Y - quaternion.X * quaternion.Z));
        float cosp = MathF.Sqrt(1 - 2 * (quaternion.W * quaternion.Y - quaternion.X * quaternion.Z));
        euler.Y = 2 * MathF.Atan2(sinp, cosp) - MathF.PI / 2;

        float siny_cosp = 2 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
        float cosy_cosp = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
        euler.Z = MathF.Atan2(siny_cosp, cosy_cosp);

        return euler;
    }
}