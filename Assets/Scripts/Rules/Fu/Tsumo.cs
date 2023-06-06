using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class Tsumo : IMahjongFuRule
    {
        public int Fu { get; set; } = 2;
        public string Name => "Self-Draw";
        public string JapName => "Tsumo";
        public string KanjiName => "自摸";
        public string Description => "Tile is drawn from the bank.";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.WinningMoveType == WinningMoveType.Tsumo)
                return Fu;

            return 0;
        }
    }
}