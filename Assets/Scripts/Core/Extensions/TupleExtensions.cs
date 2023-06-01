using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions
{
    public static class TupleExtensions
    {
        public static List<T> ToList<T>(this ITuple tuple)
        {
            return Enumerable
                .Range(0, tuple.Length)
                .Select(i => (T)tuple[i])
                .ToList();
        }
    }
}
