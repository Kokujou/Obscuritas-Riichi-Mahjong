﻿using ObscuritasRiichiMahjong.Core.Data;
using System.Collections.Generic;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongBoard
    {
        public Dictionary<CardinalPoint, MahjongPlayer> Players { get; set; } = new(4);
        public MahjongPlayer Winner { get; set; }

        public List<MahjongTile> Wall { get; set; } = new(70);
        public List<MahjongTile> KanDora { get; set; } = new(5);
        public List<MahjongTile> UraDora { get; set; } = new(5);
        public List<MahjongTile> KanWall { get; set; } = new(4);

        public CardinalPoint CardinalPoint { get; set; }
        public int MaxRounds { get; set; }
        public int CurrentRound { get; set; }
        public MoveType LastMoveType { get; set; }
        public WinningMoveType WinningMoveType { get; set; }
        public MahjongTile LastDiscardedTile { get; set; }
        public MahjongTile WinningTile { get; set; }
        public CardinalPoint CurrentRoundWind { get; set; }

        public MahjongBoard()
        {
            MaxRounds = 70;
            CurrentRound = 1;
            CurrentRoundWind = CardinalPoint.East;
        }
    }
}