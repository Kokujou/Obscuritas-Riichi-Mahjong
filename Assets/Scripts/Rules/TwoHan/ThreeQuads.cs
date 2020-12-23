using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class ThreeQuads : MahjongRule
    {
        public override int Han { get; set; } = 2;
        public override string Name => "Three Quads";
        public override string JapName => "San Kantsu";
        public override string KanjiName => "三槓子";
        public override string Description => "Three quads of any color.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var quads = handSplit.EnrichSplittedHand(player).GetQuads();
            if (quads.Count >= 3)
                return true;

            return false;
        }
    }
}