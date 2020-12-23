using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class RobbingQuad : MahjongRule
    {
        public override string Name => "Robbing a Quad";
        public override string JapName => "Chankan";
        public override string KanjiName => "搶槓";

        public override string Description =>
            "Declaring Ron on the previous players open Kan.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.LastMoveType == MoveType.Kan && board.WinningMoveType == WinningMoveType.Ron)
                return true;

            return false;
        }
    }
}