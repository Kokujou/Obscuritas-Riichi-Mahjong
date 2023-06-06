using ObscuritasRiichiMahjong.Rules.Interfaces;
using System;
using System.Collections.Generic;

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
        public List<IMahjongFuRule> CollectedFu { get; set; }

        // Missing: Fu Calculation List

        public string PointsDescription => GetPointsDescription();

        public PointResult()
        {
            CollectedYaku = new List<MahjongRule>();
        }

        public string GetTotalPointsString(bool ron, bool isDealer)
        {
            var totalPoints = TotalPoints;

            if (isDealer)
                totalPoints = (int)(totalPoints * 1.5);

            if (ron)
                return $"{totalPoints} pts.";

            if (isDealer)
                return $"{TotalPoints / 3} pts.\nfrom All";

            return $"{TotalPoints / 4} / {TotalPoints / 2} pts.";
        }

        private string GetPointsDescription()
        {
            var description = $"{Han} Han / {Fu} Fu";

            if (Yakuman > 0)
                return $"{Yakuman}x Yakuman";

            var totalPoints = CalculateTotalPoints();
            if (totalPoints == YakumanPoints)
                description += "\nCalculated Yakuman";
            else if (totalPoints == SanbaimanPoints)
                description += "\nSanbaiman";
            else if (totalPoints == BaimanPoints)
                description += "\nBaiman";
            else if (totalPoints == HanemanPoints)
                description += "\nHaneman";
            else if (totalPoints == ManganPoints)
                description += "\nMangan";

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

            return Fu * (int)Math.Pow(2, 2 + Han);
        }
    }
}