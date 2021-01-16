using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongBoard
    {
        public MahjongBoard()
        {
            MaxRounds = 70;
            CurrentRound = 0;
            CurrentRoundWind = CardinalPoint.East;
        }

        public Dictionary<CardinalPoint, MahjongPlayer> Players { get; set; } =
            new Dictionary<CardinalPoint, MahjongPlayer>(4);

        public List<MahjongTile> Wall { get; set; } = new List<MahjongTile>(70);
        public List<MahjongTile> KanDora { get; set; } = new List<MahjongTile>(5);
        public List<MahjongTile> UraDora { get; set; } = new List<MahjongTile>(5);

        public int KanCalls { get; set; }
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