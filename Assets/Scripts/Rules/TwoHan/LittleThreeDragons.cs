using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class LittleThreeDragons : MahjongRule
    {
        public override int Han { get; set; } = 2;
        public override string Name => "Little Three Dragons";
        public override string JapName => "Shousangen";
        public override string KanjiName => "小三元";

        public override string Description =>
            "Two triplets or quads of dragons, plus a pair of the third.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var dragonTripletsOrQuads = handSplit.EnrichSplittedHand(player)
                .Where(group => group.Any(x => x.Type == MahjongTileType.Dragon)).ToList();

            if (dragonTripletsOrQuads.Count >= 3 && dragonTripletsOrQuads.Any(x => x.Count == 2))
                return true;

            return false;
        }
    }
}