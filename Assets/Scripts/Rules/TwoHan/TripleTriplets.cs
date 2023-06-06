using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class TripleTriplets : MahjongRule
    {
        public override int Han { get; set; } = 2;
        public override int OpenHandPunishment => 1;
        public override string Name => "Three Triplets";
        public override string JapName => "Sanshoku";
        public override string KanjiName => "三色";

        public override string Description =>
            "Three Triplets of the same number in all three suits.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var tripletsByNumber = handSplit.EnrichSplittedHand(player).GetTriplets()
                .Where(x => x.First().Number > 0 && x.First().Number < 10)
                .GroupBy(x => x.First().Number);

            if (tripletsByNumber.Any(x => x.Count() >= 3))
                return true;

            return false;
        }
    }
}