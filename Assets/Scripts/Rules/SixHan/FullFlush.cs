using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.SixHan
{
    public class FullFlush : MahjongRule
    {
        public override int Han {get; set;} =6;
        public override int OpenHandPunishment => 1;
        public override string Name => "Full Flush";
        public override string JapName => "Chin'itsu";
        public override string KanjiName => "清一";
        public override string Description => "All tiles in the hand beyond to the same suit";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x);
            if (allTiles.GroupBy(x => x.Type).Count() > 1)
                return false;

            return true;
        }
    }
}