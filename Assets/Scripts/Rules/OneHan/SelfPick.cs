using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class SelfPick : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override string Name => "Self-Pick";
        public override string JapName => "Tsumo";
        public override string KanjiName => "自摸";

        public override string Description =>
            "Declaring SelfPick when the Hand was completed without calling on another players discard.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.WinningMoveType == WinningMoveType.Tsumo)
                return true;

            return false;
        }
    }
}