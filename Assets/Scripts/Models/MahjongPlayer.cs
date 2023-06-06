using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Global;
using ObscuritasRiichiMahjong.Rules.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongPlayer
    {
        public RiichiType Riichi { get; set; }
        public bool Dealer => CardinalPoint == CardinalPoint.East;
        public CardinalPoint CardinalPoint { get; set; }
        public bool HandOpen { get; set; } = false;

        public List<MahjongTile> Hand { get; set; } = new(14);
        public List<List<MahjongTile>> ExposedHand { get; set; } = new(4);
        public List<List<MahjongTile>> HiddenKan { get; set; } = new(4);
        public List<MahjongTile> DiscardedTiles { get; set; } = new();

        public MoveType LastMoveType { get; set; }

        public int HandTileCount => Hand.Count
                                    + HiddenKan.Count * 3
                                    + ExposedHand.Count * 3;

        public int Points { get; set; } = 20000;

        public bool CanRiichi => true; //!HandOpen && IsTenpai();

        public MahjongPlayer(CardinalPoint cardinalPoint)
        {
            CardinalPoint = cardinalPoint;
        }

        public List<CallType> GetAvailableCallTypes(MahjongTile lastDiscard)
        {
            var possibleCalls = new List<CallType>();

            if (CanPon(lastDiscard))
                possibleCalls.Add(CallType.Pon);
            if (CanChi(lastDiscard))
                possibleCalls.Add(CallType.Chi);
            if (CanKan(lastDiscard))
                possibleCalls.Add(CallType.OpenKan);
            if (CanRon(lastDiscard))
                possibleCalls.Add(CallType.Ron);

            possibleCalls.Add(CallType.Skip);

            return possibleCalls;
        }

        public List<MahjongTile> GetNonTenpaiTiles()
        {
            if (Hand.Count != 14) throw new Exception("For this check the hand needs to consist out of 14 tiles. (+ last drawn)");
            var tiles = new List<MahjongTile>(Hand);
            var nonTenpaiTiles = new List<MahjongTile>();
            foreach (var tile in tiles)
            {
                if (IsTenpai(tiles.Except(tile))) continue;
                nonTenpaiTiles.Add(tile);
            }

            return nonTenpaiTiles;
        }

        public bool CanPon(MahjongTile lastDiscard)
        {
            if (Hand.Count(x => x == lastDiscard) >= 2)
                return true;

            return false;
        }

        public bool CanChi(MahjongTile lastDiscard)
        {
            if (!lastDiscard.IsNumbered) return false;

            var sortedNumbers = Hand
                .Where(x => x.Type == lastDiscard.Type && Math.Abs(lastDiscard.Number - x.Number) <= 2)
                .Select(x => x.Number)
                .Append(lastDiscard.Number)
                .OrderBy(x => x)
                .Distinct()
                .ToList();
            if (sortedNumbers.Count < 3) return false;

            for (var i = 0; i < sortedNumbers.Count - 2; i++)
            {
                if (sortedNumbers[i] + 1 == sortedNumbers[i + 1] &&
                    sortedNumbers[i + 1] + 1 == sortedNumbers[i + 2])
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
            if (handSplits is null || handSplits.Count == 0) return false;
            return handSplits.Any(split => split.EnrichSplittedHand(this).Count == 5);
        }


        public bool IsTenpai(IEnumerable<MahjongTile> hand)
        {
            return GetTilesFromTenpaiToYaku(hand).Count > 0;
        }

        public List<MahjongTile> GetTilesFromTenpaiToYaku(IEnumerable<MahjongTile> hand)
        {
            var yakuTiles = new List<MahjongTile>();
            foreach (var tile in PrefabCollection.Instance.TileSet)
            {
                var handSplits = hand.Append(tile).ToList().GetValidHands();
                if (handSplits is not null && handSplits.Count > 0) yakuTiles.Add(tile);
            }

            return yakuTiles;
        }
    }
}