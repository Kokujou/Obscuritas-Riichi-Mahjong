using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class AllTerminals : MahjongRule
    {
        public override int Yakuman => 1;
        public override string Name => "All Terminals";
        public override string JapName => "Chinroutou";
        public override string KanjiName => "清老頭";

        public override string Description =>
            "A hand only composed 1 or 9 tiles.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x);

            if (allTiles.All(
                    x => x.Number == 1 || x.Number == 9))
                return true;

            return false;
        }
    }
}