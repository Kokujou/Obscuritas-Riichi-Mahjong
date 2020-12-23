using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class AllGreen : MahjongRule
    {
        public override int Yakuman => 1;
        public override string Name => "All Green";
        public override string JapName => "Ryuuiisou";
        public override string KanjiName => "緑一色";

        public override string Description =>
            "A hand only composed of green-only tiles being bamboo 2,3,4,6,9 and green dragon.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x);

            if (allTiles.All(x => x.Green))
                return true;

            return false;
        }
    }
}