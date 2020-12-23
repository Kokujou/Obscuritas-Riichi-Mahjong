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

            return description;
        }
    }
}