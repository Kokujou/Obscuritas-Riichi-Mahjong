using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Models;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Extensions
{
    public static class GetValidTripletsExtension
    {
        public static List<List<MahjongTile>> GetValidTriplets(this List<MahjongTile> subHand)
        {
            return GetPons(subHand).Union(subHand.GetChis()).ToList();
        }

        private static List<List<MahjongTile>> GetPons(this List<MahjongTile> subHand)
        {

            return subHand.GroupBy(x => x.Name).Where(x => x.Count() == 3).Select(x => x.ToList()).ToList();
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