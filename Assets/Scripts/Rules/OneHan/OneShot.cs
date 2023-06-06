using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class OneShot : MahjongRule
    {
        public override string Name => "One Shot";
        public override string JapName => "Ippatsu";
        public override string KanjiName => "一発";

        public override string Description =>
            "Complete the Hand the first round after declaring ReadyHand.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (player.LastMoveType == MoveType.Riichi)
                return true;

            return false;
        }
    }
}