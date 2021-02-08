using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class BigThreeDragons : MahjongRule
    {
        public override int Yakuman => 1;
        public override string Name => "Big Three Dragons";
        public override string JapName => "Dai San Gen";
        public override string KanjiName => "大三元";

        public override string Description =>
            "A triplet or quad of each dragon tile.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var dragonTripletsOrQuads = handSplit.EnrichSplittedHand(player).GetTripletsOrQuads()
                .Where(x => x.First().Type == MahjongTileType.Dragon);

            if (dragonTripletsOrQuads.Count() == 3)
                return true;

            return false;
        }
    }
}