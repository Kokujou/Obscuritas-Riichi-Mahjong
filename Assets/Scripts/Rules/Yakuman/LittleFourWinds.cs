using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class LittleFourWinds : MahjongRule
    {
        public override int Yakuman => 1;
        public override string Name => "Little Four Winds";
        public override string JapName => "Shousuushii";
        public override string KanjiName => "小四喜";

        public override string Description =>
            "A hand consisting of three triplets/quads of winds and a pair of the fourth wind.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var windTripletsOrQuads = handSplit.EnrichSplittedHand(player).GetTripletsOrQuads()
                .Where(x => x.First().Type == MahjongTileType.Wind).ToList();

            if (windTripletsOrQuads.Count == 3 && handSplit.First(x => x.Count == 2).First().Type ==
                MahjongTileType.Wind)
                return true;

            return false;
        }
    }
}