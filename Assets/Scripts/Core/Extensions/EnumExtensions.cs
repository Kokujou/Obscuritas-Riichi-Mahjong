using System;
using System.Linq;

namespace ObscuritasRiichiMahjong.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T target) where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var nextIndex = enumValues.IndexOf(target) + 1;

            if (nextIndex >= enumValues.Count)
                return enumValues.First();
            return enumValues[nextIndex];
        }

        public static T Last<T>(this T target) where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var nextIndex = enumValues.IndexOf(target) - 1;

            if (nextIndex < 0)
                return enumValues.Last();
            return enumValues[nextIndex];
        }
    }
}