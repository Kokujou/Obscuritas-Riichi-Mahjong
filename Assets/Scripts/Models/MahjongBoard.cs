using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongBoard
    {
        public List<MahjongPlayer> Players { get; set; }
        public CardinalPoint CardinalPoint { get; set; }
        public int MaxRounds { get; set; }
        public int CurrentRound { get; set; } = 1;
        public MoveType LastMoveType { get; set; }
        public WinningMoveType WinningMoveType { get; set; }
        public MahjongTile LastDiscardedTile { get; set; }
        public MahjongTile WinningTile { get; set; }
    }
}