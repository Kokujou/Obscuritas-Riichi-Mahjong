using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions
{
    public static class MahjongTileExtensions
    {
        public static IEnumerable<List<MahjongTile>> GetChisWithTile
            (this IEnumerable<MahjongTileComponent> subHand, MahjongTileComponent discard)
        {
            return subHand
                .Select(x => x.Tile)
                .GetChisWithTile(discard.Tile);
        }

        public static IEnumerable<List<MahjongTile>> GetChisWithTile(this IEnumerable<MahjongTile> subHand, MahjongTile discard)
        {
            return subHand
                .Where(x => x.Type == discard.Type && Mathf.Abs(discard.Number - x.Number) <= 2)
                .GetChis()
                .DistinctBy(chi => string.Join("|", chi.Select(tile => tile.ToString())));
        }

        public static IEnumerable<List<MahjongTileComponent>> GetChis(this IEnumerable<MahjongTileComponent> subHand)
        {
            var dictionary = subHand.ToDictionary(x => x.Tile, x => x);
            return subHand.Select(x => x.Tile).GetChis().Select(x => x.Select(y => dictionary[y]).ToList());
        }



    }
}
