using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Data
{
    public static class TransformToEnumerableExtension
    {
        public static IEnumerable<Transform> ToEnumerable(this Transform target)
        {
            return new TransformEnumerable(target);
        }
    }
}