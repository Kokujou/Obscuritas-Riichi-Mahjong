using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Extensions
{
    public static class GetValidTripletsExtension
    {
        public static List<List<MahjongTile>> GetValidTriplets(this List<MahjongTile> subHand)
        {
            if (subHand.First().Type == MahjongTileType.Dragon ||
                subHand.First().Type == MahjongTileType.Wind)
                return GetPons(subHand);

            return GetPons(subHand).Union(subHand.GetChis()).ToList();
        }

        private static List<List<MahjongTile>> GetPons(this List<MahjongTile> subHand)
        {
            var pons = new List<List<MahjongTile>>();

            for (var i = 0; i < subHand.Count; i++)
            {
                var firstTile = subHand[i];
                var tilesAfter = subHand.Skip(i);
                var matchingTileIndices =
                    tilesAfter.Where(x => x == firstTile).ToList();

                if (matchingTileIndices.Count == 3)
                    pons.Add(matchingTileIndices);
            }

            return pons;
        }

        public static IEnumerable<List<MahjongTile>> GetChis(this IEnumerable<MahjongTile> subHand)
        {
            return subHand
                .Where(x => x.IsNumbered)
                .OrderBy(x => x.Number)
                .GroupBy(x => x.Type)
                .SelectMany(group => group.GetChisForType())
                .DistinctBy(chi => string.Join("|", chi.Select(tile => tile.ToString())))
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