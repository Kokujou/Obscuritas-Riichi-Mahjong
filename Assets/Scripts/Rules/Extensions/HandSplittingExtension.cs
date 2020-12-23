﻿using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Models;

namespace ObscuritasRiichiMahjong.Rules.Extensions
{
    public static class HandSplittingExtension
    {
        public static List<List<List<MahjongTile>>> GetValidHands(this MahjongPlayer player)
        {
            var validHands = player.Hand.GetValidHands();
            for (var index = 0; index < validHands.Count; index++)
                validHands[index] = validHands[index].Union(player.ExposedHand)
                    .Union(player.HiddenKan)
                    .ToList();

            return validHands;
        }

        private static List<List<List<MahjongTile>>> GetValidHands(this List<MahjongTile> hand)
        {
            if (hand.Count < 2 || (hand.Count - 2) % 3 != 0)
                return null;

            var targetTripletCount = (hand.Count - 2) / 3;

            var suitSplits = hand.GroupBy(x => x.Type);
            var validTriplets = new List<List<MahjongTile>>();
            var validSplits = new List<List<List<MahjongTile>>>();

            foreach (var suitSplit in suitSplits)
                validTriplets.AddRange(suitSplit.ToList().GetValidTriplets());

            var possibleCombinations =
                validTriplets.GetCombinations(targetTripletCount).Select(x => x.ToList()).ToList();

            foreach (var combination in possibleCombinations)
            {
                if (!hand.TryApplyTriplets(combination, out var leftTiles)) continue;
                if (leftTiles.Count != 2 || leftTiles[0] != leftTiles[1]) continue;

                combination.Add(leftTiles);
                validSplits.Add(combination);
            }

            return validSplits.GroupBy(x => x.Stringify()).Select(x => x.First()).ToList();
        }

        public static string Stringify(this IEnumerable<List<MahjongTile>> handSplit)
        {
            return $"|{string.Join(",", handSplit.Select(x => $"<{string.Join(",", x)}>"))}|";
        }

        private static bool TryApplyTriplets(this IEnumerable<MahjongTile> refHand,
            IEnumerable<List<MahjongTile>> triplets, out List<MahjongTile> leftTiles)
        {
            leftTiles = null;
            var hand = new List<MahjongTile>(refHand);
            var tiles = triplets.SelectMany(x => x).ToList();

            foreach (var tile in tiles)
            {
                if (!hand.Remove(tile))
                    return false;

                leftTiles = hand;
            }

            leftTiles = hand;
            return true;
        }
    }
}