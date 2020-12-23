using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;

namespace ObscuritasRiichiMahjong.Rules.Extensions
{
    public static class GetValidTripletsExtension
    {
        public static List<List<MahjongTile>> GetValidTriplets(this List<MahjongTile> subHand)
        {
            if (subHand.First().Type == MahjongTileType.Dragon ||
                subHand.First().Type == MahjongTileType.Wind)
                return GetPons(subHand);

            return GetPons(subHand).Union(GetChis(subHand)).ToList();
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

        private static List<List<MahjongTile>> GetChis(this List<MahjongTile> subHand)
        {
            var chis = new List<List<MahjongTile>>();

            for (var firstTileIndex = 0; firstTileIndex < subHand.Count; firstTileIndex++)
            {
                var firstTile = subHand[firstTileIndex];
                var matchingTiles = new List<MahjongTile> {firstTile};
                var tilesAfter = subHand.Skip(firstTileIndex).ToList();

                var secondTileIndex =
                    tilesAfter.FirstOrDefault(x => x.Number == firstTile.Number + 1);
                if (secondTileIndex) matchingTiles.Add(secondTileIndex);
                else continue;

                var thirdTileIndex =
                    tilesAfter.FirstOrDefault(x => x.Number == firstTile.Number + 2);
                if (thirdTileIndex) matchingTiles.Add(thirdTileIndex);
                else continue;

                chis.Add(matchingTiles);
            }

            return chis;
        }
    }
}