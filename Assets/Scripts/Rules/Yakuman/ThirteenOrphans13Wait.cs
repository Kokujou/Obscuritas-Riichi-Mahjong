using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class ThirteenOrphans13Wait : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override int Yakuman => 2;
        public override string Name => "Thirteen Orphans 13-way wait";
        public override string JapName => "Kokushi Musou 13 Men Machi";
        public override string KanjiName => "国士無双１３面待ち";

        public override string Description =>
            "All non-triplet terminals plus a matching pair of terminals with the winning tile being from the pair.";

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
                return true;

            return false;
        }
    }
}