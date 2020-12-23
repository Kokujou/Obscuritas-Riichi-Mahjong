using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.ThreeHan
{
    public class TerminalInEachSet : MahjongRule
    {
        public override int Han {get; set;} = 3;
        public override int OpenHandPunishment => 1;
        public override string Name => "Fully Outside Hand";
        public override string JapName => "Junchan";
        public override string KanjiName => "純チャン";
        public override string Description => "Every set of the hand contains at least one 1 or 9.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var groups = handSplit.EnrichSplittedHand(player);

            if (groups.Any(group => group.All(tile => tile.Number != 9 && tile.Number != 1)))
                return false;

            return true;
        }
    }
}