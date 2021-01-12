using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongBoard
    {
        public Dictionary<CardinalPoint, MahjongPlayer> Players { get; set; } =
            new Dictionary<CardinalPoint, MahjongPlayer>(4);

        public CardinalPoint CardinalPoint { get; set; }
        public int MaxRounds { get; set; }
        public int CurrentRound { get; set; } = 1;
        public MoveType LastMoveType { get; set; }
        public WinningMoveType WinningMoveType { get; set; }
        public MahjongTile LastDiscardedTile { get; set; }
        public MahjongTile WinningTile { get; set; }
        public CardinalPoint CurrentRoundWind { get; set; }
    }
}