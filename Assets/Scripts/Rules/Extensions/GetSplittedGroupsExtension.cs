using ObscuritasRiichiMahjong.Models;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Extensions
{
    public static class GetSplittedGroupsExtension
    {
        public static List<List<MahjongTile>> GetSequences(
            this IEnumerable<List<MahjongTile>> handSplit)
        {
            return handSplit.Where(x => x.First() != x.Last()).ToList();
        }

        public static List<List<MahjongTile>> GetTriplets(
            this IEnumerable<List<MahjongTile>> handSplit)
        {
            return handSplit.Where(group => group.All(x => x == group.First()) && group.Count == 3)
                .ToList();
        }

        public static List<List<MahjongTile>> GetQuads(
            this IEnumerable<List<MahjongTile>> handSplit)
        {
            return handSplit.Where(group => group.Count == 4).ToList();
        }

        public static List<List<MahjongTile>> GetTripletsOrQuads(
            this IEnumerable<List<MahjongTile>> handSplit)
        {
            return handSplit.Where(group => group.All(x => x == group.First()) && group.Count >= 3)
                .ToList();
        }
    }
}