using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongPlayer
    {
        public MahjongPlayer(List<MahjongTile> hand, CardinalPoint cardinalPoint)
        {
            CardinalPoint = cardinalPoint;
            if (CardinalPoint == CardinalPoint.East)
                Dealer = true;
            Hand = hand;
        }

        public bool Riichi {get; set;} = false;
        public bool Ippatsu {get; set;} = false;
        public bool Dealer {get; set;}
        public CardinalPoint CardinalPoint {get; set;}
        public bool HandOpen {get; set;} = false;

        public List<MahjongTile> Hand {get; set;}

        public List<List<MahjongTile>> ExposedHand {get; set;}
            = new List<List<MahjongTile>>(4);

        public List<List<MahjongTile>> HiddenKan {get; set;}
            = new List<List<MahjongTile>>(4);

        public List<MahjongTile> DiscardedTiles {get; set;} = new List<MahjongTile>();

        public List<MahjongTile> Wall {get; set;} = new List<MahjongTile>();

        public bool IsTenpai()
        {
            return false;
        }
    }
}