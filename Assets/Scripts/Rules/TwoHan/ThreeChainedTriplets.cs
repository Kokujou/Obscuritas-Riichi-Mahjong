using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class ThreeChainedTriplets : MahjongRule
    {
        public override int Han => 2;
        public override string Name => "Three Chained Triplets";
        public override string JapName => "Sanrenkou";
        public override string KanjiName => "三連刻";

        public override string Description =>
            "Three triplets of successive numbers of the same suit.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTriplets = handSplit.EnrichSplittedHand(player).GetTriplets()
                .Where(x => x.First().Number > 0 && x.First().Number < 10);
            var biggestSuitGroups =
                allTriplets.GroupBy(x => x.First().Type).SingleOrDefault(x => x.Count() >= 3);

            if (biggestSuitGroups == default) return false;

            var biggestSuitNumbers =
                biggestSuitGroups.Select(x => x.First().Number).Distinct().OrderBy(x => x).ToList();

            for (var i = 1; i < biggestSuitNumbers.Count - 1; i++)
                if (biggestSuitNumbers[i] == biggestSuitNumbers[i - 1] - 1 &&
                    biggestSuitNumbers[i] == biggestSuitNumbers[i + 1] + 1)
                    return true;

            return false;
        }
    }
}