using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class PureStraight : MahjongRule
    {
        public override int Han { get; set; } = 2;
        public override int OpenHandPunishment => 1;
        public override string Name => "Straight";
        public override string JapName => "Ittsuu";
        public override string KanjiName => "一通";
        public override string Description => "Contains 1-2-3, 4-5-6, 7-8-9 in the same suit.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var sequences = handSplit.EnrichSplittedHand(player).GetSequences();

            var biggestSuitGroups =
                sequences.GroupBy(x => x.First().Type).SingleOrDefault(x => x.Count() >= 3);

            if (biggestSuitGroups == default)
                return false;

            var numberGroups = new List<string> { "123", "456", "789" };
            foreach (var sequence in biggestSuitGroups)
            {
                var sequenceString = string.Join("", sequence.Select(x => x.Number.ToString()));
                numberGroups.Remove(sequenceString);
            }

            if (numberGroups.Count == 0)
                return true;

            return false;
        }
    }
}