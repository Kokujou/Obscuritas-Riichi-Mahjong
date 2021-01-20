namespace ObscuritasRiichiMahjong.Core.Extensions
{
    public static class RoundingExtensions
    {
        public static bool AboutEquals(this float input, float other, float threshold = .5f)
        {
            return input >= other - threshold && input <= other + threshold;
        }
    }
}