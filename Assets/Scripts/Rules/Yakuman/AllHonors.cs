using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class AllHonors : MahjongRule
    {
        public override int Yakuman => 1;
        public override string Name => "All Honors";
        public override string JapName => "Tsuuiisou";
        public override string KanjiName => "字一色";

        public override string Description =>
            "A hand only composed of wind and dragon tiles.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x);

            if (allTiles.All(
                    x => x.Type == MahjongTileType.Dragon || x.Type == MahjongTileType.Wind))
                return true;

            return false;
        }
    }
}