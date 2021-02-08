using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class ThirteenOrphans : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override int Yakuman => 1;
        public override string Name => "Thirteen Orphans";
        public override string JapName => "Kokushi Musou";
        public override string KanjiName => "国士無双";

        public override string Description =>
            "All non-triplet terminals plus a matching pair of terminals with the winning tile being drawn from the orphans.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.WinningMoveType == WinningMoveType.Ron)
                return false;

            var handByName = player.Hand.Where(x => x.IsTerminal()).GroupBy(x => x.Name).ToList();

            if (handByName.Count != 13 || handByName.Count(x => x.Count() > 1) != 1)
                return false;

            var pair = handByName.First(x => x.Count() == 2);
            if (board.WinningTile == pair.First())
                return false;

            return true;
        }
    }
}