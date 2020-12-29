using System;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Services
{
    public class ChiSelectionService : NormalSelectionService
    {
        public ChiSelectionService(MahjongPlayer player, MahjongBoard board) : base(player, board)
        {
        }

        public override void AddToHand(MahjongTile2DComponent tile, Transform handParent)
        {
            Player.HandOpen = true;
            var selectableTiles = tile.transform.parent
                .Cast<Transform>()
                .Select(child => child.gameObject.GetComponent<MahjongTile2DComponent>())
                .Where(x => TileSequenceContains(tile, x))
                .OrderByDescending(x => x.Tile.Number)
                .ToList();

            var secondTile = selectableTiles[0];
            var thirdTile = selectableTiles[1];

            CloneTileToTransform(tile, handParent);
            CloneTileToTransform(secondTile, handParent);
            CloneTileToTransform(thirdTile, handParent);

            Player.ExposedHand.Add(new List<MahjongTile>());
            Player.ExposedHand.Last().AddRange(new[] {tile.Tile, secondTile.Tile, thirdTile.Tile});
        }

        public static bool TileSequenceContains(MahjongTile2DComponent first,
            MahjongTile2DComponent current)
        {
            var offset = Math.Abs(current.Tile.Number - first.Tile.Number);
            return current.Tile.Type == first.Tile.Type && (offset == 1 || offset == 2);
        }

        public override bool CanSelect(MahjongTile tile)
        {
            if (Player.HandTileCount > 12)
                return false;

            if (tile.Type != MahjongTileType.Bamboo && tile.Type != MahjongTileType.Circle &&
                tile.Type != MahjongTileType.Kanji)
                return false;

            if (!base.CanSelect(tile))
                return false;

            for (var i = 1; i <= 2; i++)
            {
                var nextTileCount =
                    AllTiles.Count(x => x.Number == tile.Number + i && x.Type == tile.Type);
                if (nextTileCount >= 4)
                    return false;
            }

            return true;
        }
    }
}