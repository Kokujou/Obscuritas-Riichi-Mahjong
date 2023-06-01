
using UnityEngine;

namespace ObscuritasRiichiMahjong.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ReplaceX(this Vector3 input, float x)
        {
            return new Vector3(x, input.y, input.z);
        }

        public static Vector3 ReplaceY(this Vector3 input, float y)
        {
            return new Vector3(input.x, y, input.z);
        }

        public static Vector3 ReplaceZ(this Vector3 input, float z)
        {
            return new Vector3(input.x, input.y, z);
        }
    }
}
