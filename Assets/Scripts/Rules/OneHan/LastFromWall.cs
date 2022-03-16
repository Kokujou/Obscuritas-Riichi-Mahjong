using System.Collections.Generic;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class LastFromWall : MahjongRule
    {
        public override string Name => "Last tile from the wall";
        public override string JapName => "Haitei Raoyue";
        public override string KanjiName => "海底撈月";

        public override string Description =>
            "Declaring SelfPick with the last tile from the own wall";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.Wall.Count == 0 && board.WinningMoveType == WinningMoveType.Tsumo)
                return true;

            return false;
        }
    }
}