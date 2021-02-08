using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Core.Extensions
{
    public static class RandomSubsetExtension
    {
        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> set, int count, out List<T> leftover)
        {
            return set.TransformRandomSubset(count, x => x, out leftover);
        }

        public static IEnumerable<TResult> TransformRandomSubset<TInput, TResult>(this IEnumerable<TInput> set,
            int count, Func<TInput, TResult> transform, out List<TInput> leftover)
        {
            leftover = set.ToList();
            var subset = new List<TResult>(count);
            for (var i = 0; i < count; i++)
            {
                var index = Random.Range(0, leftover.Count);
                var inputObject = leftover[index];
                var outputObject = transform(inputObject);

                subset.Add(outputObject);
                leftover.RemoveAt(index);
            }

            return subset;
        }
    }
}