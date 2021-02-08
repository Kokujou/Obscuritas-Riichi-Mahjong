using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class BigFourWinds : MahjongRule
    {
        public override int Yakuman => 2;
        public override string Name => "Big Four Winds";
        public override string JapName => "Daisuushii";
        public override string KanjiName => "大四喜";

        public override string Description =>
            "A hand consisting of triplets/quads of all winds.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var windTripletsOrQuads = handSplit.EnrichSplittedHand(player).GetTripletsOrQuads()
                .Where(x => x.First().Type == MahjongTileType.Wind).ToList();

            if (windTripletsOrQuads.Count == 4)
                return true;

            return false;
        }
    }
}