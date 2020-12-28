using System;
using System.Collections.Generic;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules
{
    public class PointResult
    {
        public const int YakumanPoints = 32000;
        public const int SanbaimanPoints = 24000;
        public const int BaimanPoints = 16000;
        public const int HanemanPoints = 12000;
        public const int ManganPoints = 8000;

        public int Yakuman { get; set; }
        public int Han { get; set; }
        public int Fu { get; set; }
        public int TotalPoints => CalculateTotalPoints();
        public bool FromAll { get; set; }

        public List<MahjongRule> CollectedYaku { get; set; }

        // Missing: Fu Calculation List

        public string PointsDescription => GetPointsDescription();

        public PointResult()
        {
            CollectedYaku = new List<MahjongRule>();
        }

        private string GetPointsDescription()
        {
            var description = $"{Han} Han / {Fu} Fu";

            if (Yakuman > 0)
                description = $"{Yakuman}x Yakuman";

            return description;
        }

        private int CalculateTotalPoints()
        {
            if (Yakuman > 0)
                return YakumanPoints * Yakuman;
            if (Han >= 13)
                return YakumanPoints;
            if (Han >= 11)
                return SanbaimanPoints;
            if (Han >= 8)
                return BaimanPoints;
            if (Han >= 6)
                return HanemanPoints;
            if (Han == 5
                || Han == 4 && Fu >= 40
                || Han == 3 && Fu >= 70)
                return ManganPoints;

            return Fu * (int) Math.Pow(2, 2 + Han);
        }
    }
}