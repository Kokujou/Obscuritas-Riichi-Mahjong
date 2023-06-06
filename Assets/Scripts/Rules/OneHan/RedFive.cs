using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class RedFive : MahjongRule
    {
        public override string Name => "Red Five";
        public override string JapName => "Akago";
        public override string KanjiName => "赤五";
        public override string Description => "The winning hand contains one or more red fives.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return GetHan(handSplit, board, player) > 0;
        }

        public override int GetHan(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return player.Hand.Count(x => x.Red);
        }
    }
}