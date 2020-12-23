using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class LastDiscard : MahjongRule
    {
        public override string Name => "Last discard";
        public override string JapName => "Houtei Raoyue";
        public override string KanjiName => "河底撈魚";

        public override string Description =>
            "Declaring DiscardPick on the last players tile, if it was the last tile from his bank.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (player.Wall.Count == 0
                && board.WinningMoveType == WinningMoveType.Ron)
                return true;

            return false;
        }
    }
}