using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class FourKans : MahjongRule
    {
        public override int Yakuman => 1;
        public override string Name => "Four Kans";
        public override string JapName => "Suu Kantsu";
        public override string KanjiName => "四槓子";

        public override string Description =>
            "A hand containing four quads.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var quads = handSplit.EnrichSplittedHand(player).GetQuads();

            if (quads.Count == 4)
                return true;

            return false;
        }
    }
}