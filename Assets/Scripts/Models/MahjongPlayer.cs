using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Rules.Extensions;

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

        public bool CanPon(MahjongTile lastDiscard)
        {
            if (Hand.Count(x => x == lastDiscard) >= 2)
                return true;

            return false;
        }

        public bool CanChi(MahjongTile lastDiscard)
        {
            var start = lastDiscard.Number - 2;
            start = start >= 0 ? start : 0;

            var longestSequence = 0;
            var suit = Hand.Where(x => x.Type == lastDiscard.Type).ToList();
            suit.Add(lastDiscard);
            for (var i = start; i <= 9; i++)
            {
                if (suit.Any(x => x.Number == i))
                    longestSequence++;
                else
                    longestSequence = 0;

                if (longestSequence == 3)
                    return true;
            }

            return false;
        }

        public bool CanKan(MahjongTile lastDiscard)
        {
            if (Hand.Count(x => x == lastDiscard) >= 3)
                return true;

            return false;
        }

        public bool CanRon(MahjongTile lastDiscard)
        {
            var virtualHand = Hand.ToList();
            virtualHand.Add(lastDiscard);

            var handSplits = virtualHand.GetValidHands();
            if (handSplits is null) return false;
            return handSplits.Any(split => split.EnrichSplittedHand(this).Count == 5);
        }

        public bool IsTenpai()
        {
            return false;
        }
    }
}