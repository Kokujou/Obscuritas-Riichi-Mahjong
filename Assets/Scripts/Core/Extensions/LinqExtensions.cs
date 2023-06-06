using System;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
       (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static IEnumerable<Source> Except<Source>(this IEnumerable<Source> source, Source item)
        {
            return source.Except(new List<Source>() { item });
        }
    }
}
