using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Core.Extensions
{
    public static class GetCombinationsExtension
    {
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> target,
            int length)
        {
            var items = target.ToList();
            return length == 1
                ? items.Select(item => new[] { item })
                : items.SelectMany((item, i) => items.Skip(i + 1)
                    .GetCombinations(length - 1)
                    .Select(result => new[] { item }.Concat(result)));
        }
    }
}