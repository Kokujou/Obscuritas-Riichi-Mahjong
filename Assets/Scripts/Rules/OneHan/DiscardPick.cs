using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class DiscardPick : MahjongRule
    {
        public override string Name => "DiscardPick";
        public override string JapName => "DiscardPick";
        public override string KanjiName => "栄";

        public override string Description =>
            "Using a previous players discarded tile to complete the own hand.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            if (board.WinningMoveType == WinningMoveType.Ron)
                return true;

            return false;
        }
    }
}