using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class RobbingQuad : MahjongRule
    {
        public override string Name => "Robbing a Quad";
        public override string JapName => "RobbingQuad";
        public override string KanjiName => "搶槓";

        public override string Description =>
            "Declaring DiscardPick on the previous players open Kan.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            if (board.LastMoveType == MoveType.Kan && board.WinningMoveType == WinningMoveType.Ron)
                return false;

            return true;
        }
    }
}