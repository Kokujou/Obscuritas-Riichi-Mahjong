using System.Collections.Generic;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class DeadWallDraw : MahjongRule
    {
        public override string Name => "Dead wall draw";
        public override string JapName => "Rinshan Kaihou";
        public override string KanjiName => "嶺上開花";

        public override string Description =>
            "Completing the hand with the winning tile being drawn from the dead wall after a Kan.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (player.LastMoveType == MoveType.HiddenKan ||
                player.LastMoveType == MoveType.OpenKan)
                return true;

            return false;
        }
    }
}