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
    }
}