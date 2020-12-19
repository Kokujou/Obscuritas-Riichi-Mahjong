using System.Linq;
using ObscuritasRiichiMahjong.Models;
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

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var targetSuit = player.Hand.GroupBy(x => x.Type)
                .First(x => x.Count() >= 9).ToList();
            var tripletNumbers =
                targetSuit.Where(x => targetSuit.Count(y => x.Number == y.Number) >= 3)
                    .Select(x => x.Number).OrderBy(x => x).ToList();

            for (var i = 1; i < tripletNumbers.Count - 1; i++)
                if (tripletNumbers[i - 1] == tripletNumbers[i] - 1 &&
                    tripletNumbers[i + 1] == tripletNumbers[i] + 1)
                    return true;

            return false;
        }
    }
}