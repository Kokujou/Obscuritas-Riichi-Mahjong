using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Core.Extensions
{
    public static class RandomSubsetExtension
    {
        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> set, int count)
        {
            var list = set.ToList();
            var subset = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                var index = Random.Range(0, list.Count);
                var mahjongTile = list[index];

                subset.Add(mahjongTile);
            }

            return subset;
        }
    }
}