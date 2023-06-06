using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class DoubleReady : MahjongRule
    {
        public override int Han { get; set; } = 2;
        public override string Name => "Double Ready";
        public override string JapName => "Double Riichi";
        public override string KanjiName => "ダブルリーチ";
        public override string Description => "Declare Riichi in the first round of the game.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (player.Riichi == RiichiType.DoubleRiichi)
                return true;

            return false;
        }
    }
}