using ObscuritasRiichiMahjong.Assets.Scripts.Core.Data;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules
{
    public class PointResult
    {
        public const int YakumanPoints = 32000;
        public const int SanbaimanPoints = 24000;
        public const int BaimanPoints = 16000;
        public const int HanemanPoints = 12000;
        public const int ManganPoints = 8000;

        public int Yakuman { get; }
        public int Han { get; }
        public int Fu { get; }
        public bool FromAll { get; }

        public int TotalPoints => CalculateTotalPoints();
        public string PointsDescriptionText => GetPointsDescriptionText();

        public List<MahjongRule> CollectedYaku { get; set; }


        public PointResult(List<MahjongRule> fulfilledYaku, int han, int fu, bool fromAll)
        {
            CollectedYaku = fulfilledYaku;
            FromAll = fromAll;
            Han = han;
            Fu = fu;

            var yakuman = fulfilledYaku.Where(x => x.Yakuman > 0).ToList();
            if (yakuman.Count <= 0) return;

            CollectedYaku = yakuman;
            Yakuman = yakuman.Sum(x => x.Yakuman);
            CollectedYaku = yakuman;
        }

        public string GetTotalPointsString(bool ron, bool isDealer)
        {
            var totalPoints = TotalPoints;
            if (isDealer) totalPoints = (int)(totalPoints * 1.5);
            if (ron) return $"{totalPoints} pts.";
            if (isDealer) return $"{TotalPoints / 3} pts.\nfrom All";
            return $"{TotalPoints / 4} / {TotalPoints / 2} pts.";
        }

        private string GetPointsDescriptionText()
        {
            var pointsDescription = GetPointsDescription();
            if (pointsDescription == PointDescription.Yakuman) return GetYakumanName();
            if (pointsDescription == PointDescription.None) return null;
            else return pointsDescription.ToString();
        }

        public string GetYakumanName()
        {
            if (Yakuman == 0 && Han < 13) return null;

            return Yakuman switch
            {
                0 => "Calculated Yakuman",
                1 => "Yakuman",
                2 => "Double Yakuman",
                3 => "Triple Yakuman",
                4 => "Quadruple Yakuman",
                5 => "Quintuple Yakuman",
                6 => "Sixtuple Yakuman",
                _ => null
            };
        }

        public PointDescription GetPointsDescription()
        {
            var totalPoints = CalculateTotalPoints();
            if (Han >= 13 || Yakuman > 0) return PointDescription.Yakuman;
            if (Han >= 11) return PointDescription.Sanbaiman;
            if (Han >= 8) return PointDescription.Baiman;
            if (Han >= 6) return PointDescription.Haneman;
            if (Han == 5 || Han == 4 && Fu >= 40 || Han == 3 && Fu >= 70) return PointDescription.Mangan;
            return PointDescription.None;
        }

        private int CalculateTotalPoints()
        {
            if (Yakuman > 0) return YakumanPoints * Yakuman;

            var pointDescription = GetPointsDescription();
            return pointDescription switch
            {
                PointDescription.Yakuman => YakumanPoints,
                PointDescription.Sanbaiman => SanbaimanPoints,
                PointDescription.Baiman => BaimanPoints,
                PointDescription.Haneman => HanemanPoints,
                PointDescription.Mangan => ManganPoints,
                PointDescription.None => Fu * (int)Math.Pow(2, 2 + Han),
                _ => 0
            };
        }
    }
}