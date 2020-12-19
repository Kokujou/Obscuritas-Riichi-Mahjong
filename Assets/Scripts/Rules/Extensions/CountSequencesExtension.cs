using System;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;

namespace ObscuritasRiichiMahjong.Rules.Extensions
{
    public static class CountSequencesExtension
    {
        public static List<List<MahjongTile>> GetSequences(this List<MahjongTile> hand)
        {
            var orderedHand =
                hand.OrderBy(x => x.GetTileOrder()).ToList();

            var sequences = new List<List<MahjongTile>>(4);
            sequences.Add(new List<MahjongTile>(3));
            for (var i = orderedHand.Count; i >= 0; i--)
            {
                var tile = orderedHand[i];

                if (tile.IsTerminal())
                    continue;

                var lastGroup = sequences.Last();
                if (lastGroup.Count == 3)
                {
                    lastGroup = new List<MahjongTile>();
                    sequences.Add(lastGroup);
                }

                if (lastGroup.Count == 0)
                {
                    lastGroup.Add(tile);
                    continue;
                }

                var lastTile = lastGroup.Last();
                if (Math.Abs(lastTile.Number - tile.Number) == 1 && lastTile.Type == tile.Type)
                    lastGroup.Add(tile);
                else
                    lastGroup.Clear();
            }

            if (sequences.Last().Count < 3)
                sequences.RemoveAt(sequences.Count - 1);

            return sequences;
        }
    }
}