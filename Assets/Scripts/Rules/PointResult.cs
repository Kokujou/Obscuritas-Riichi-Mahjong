using System.Collections.Generic;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules
{
    public class PointResult
    {
        public PointResult()
        {
            CollectedYaku = new List<MahjongRule>();
        }

        public int Yakuman { get; set; }
        public int Han { get; set; }
        public int Fu { get; set; }
        public int TotalPoints { get; set; }
        public bool FromAll { get; set; }

        public List<MahjongRule> CollectedYaku { get; set; }

        // Missing: Fu Calculation List

        public string PointsDescription => GetPointsDescription();

        private string GetPointsDescription()
        {
            var description = $"{Han} Han / {Fu} Fu";

            if (Yakuman > 0)
                description = $"{Yakuman}x Yakuman";

            return description;
        }
    }
}