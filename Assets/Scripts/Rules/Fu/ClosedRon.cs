using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class ClosedRon : IMahjongFuRule
    {
        public int Fu { get; set; } = 10;
        public string Name => "Closed Ron";
        public string JapName => "Menzen-Kafu";
        public string KanjiName => "門前加符";
        public string Description => "Winning the game with Ron and a closed hand";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.WinningMoveType == WinningMoveType.Ron && !player.HandOpen)
                return Fu;

            return 0;
        }
    }
}