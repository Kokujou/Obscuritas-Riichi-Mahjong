using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongPlayer
    {
        public RiichiType Riichi { get; set; }
        public bool Dealer => CardinalPoint == CardinalPoint.East;
        public CardinalPoint CardinalPoint { get; set; }
        public bool HandOpen { get; set; } = false;

        public List<MahjongTile> Hand { get; set; } = new List<MahjongTile>(14);

        public List<List<MahjongTile>> ExposedHand { get; set; }
            = new List<List<MahjongTile>>(4);

        public List<List<MahjongTile>> HiddenKan { get; set; }
            = new List<List<MahjongTile>>(4);

        public List<MahjongTile> DiscardedTiles { get; set; } = new List<MahjongTile>();

        public List<MahjongTile> Wall { get; set; } = new List<MahjongTile>();

        public MoveType LastMoveType { get; set; }

        public int HandTileCount => Hand.Count
                                    + HiddenKan.Count * 3
                                    + ExposedHand.Count * 3;

        public int Points { get; set; } = 20000;

        public MahjongPlayer(CardinalPoint cardinalPoint)
        {
            CardinalPoint = cardinalPoint;
        }

        public bool IsTenpai()
        {
            return false;
        }
    }
}