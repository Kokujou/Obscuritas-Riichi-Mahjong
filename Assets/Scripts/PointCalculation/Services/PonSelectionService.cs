using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Services
{
    public class PonSelectionService : NormalSelectionService
    {
        public PonSelectionService(MahjongPlayer player, MahjongBoard board) : base(player, board)
        {
        }

        public override void AddToHand(MahjongTile2DComponent tile, Transform handParent)
        {
            Player.HandOpen = true;
            Player.ExposedHand.Add(new List<MahjongTile>());
            for (var i = 0; i < 3; i++)
            {
                var clonedTile = CloneTileToTransform(tile, handParent);
                Player.ExposedHand.Last().Add(clonedTile.Tile);
            }
        }

        public override bool CanSelect(MahjongTile tile)
        {
            if (Player.HandTileCount > 11)
                return false;

            var handCount = AllTiles.Count(x => x.Name == tile.Name);
            if (handCount > 1)
                return false;

            return true;
        }
    }
}