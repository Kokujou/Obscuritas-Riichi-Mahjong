using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions
{
    public static class MahjongTileExtensions
    {
        public static IEnumerable<List<MahjongTileComponent>> GetChisWithTile
            (this IEnumerable<MahjongTileComponent> subHand, MahjongTileComponent discard)
        {
            var dictionary = subHand.ToDictionary(x => x.Tile, x => x);
            return subHand.Select(x => x.Tile).GetChisWithTile(discard.Tile).Select(x => x.Select(y => dictionary[y]).ToList());
        }

        public static IEnumerable<List<MahjongTile>> GetChisWithTile(this IEnumerable<MahjongTile> subHand, MahjongTile discard)
        {
            return subHand
                .Where(x => x.Type == discard.Type && Mathf.Abs(discard.Number - x.Number) <= 2)
                .GetChis();
        }

        public static IEnumerable<List<MahjongTileComponent>> GetChis(this IEnumerable<MahjongTileComponent> subHand)
        {
            var dictionary = subHand.ToDictionary(x => x.Tile, x => x);
            return subHand.Select(x => x.Tile).GetChis().Select(x => x.Select(y => dictionary[y]).ToList());
        }

        public static IEnumerable<List<MahjongTile>> GetChis(this IEnumerable<MahjongTile> subHand)
        {
            return subHand
                .Where(x => x.IsNumbered)
                .OrderBy(x => x.Number)
                .GroupBy(x => x.Type)
                .SelectMany(group => group.GetChisForType())
                .ToList();
        }

        private static IEnumerable<List<MahjongTile>> GetChisForType(this IEnumerable<MahjongTile> group)
        {
            var tiles = group.ToList();
            for (var firstTileIndex = 0; firstTileIndex < tiles.Count - 2; firstTileIndex++)
            {
                var firstTile = tiles[firstTileIndex];

                var secondTile = tiles.FirstOrDefault(x => x.Number == firstTile.Number + 1);
                if (secondTile == null) continue;

                var thirdTile = tiles.FirstOrDefault(x => x.Number == firstTile.Number + 2);
                if (thirdTile == null) continue;
                yield return new List<MahjongTile> { firstTile, secondTile, thirdTile };
            }
        }

    }
}
