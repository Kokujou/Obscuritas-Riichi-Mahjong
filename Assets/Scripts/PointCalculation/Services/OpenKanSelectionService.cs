using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Services
{
    public class OpenKanSelectionService : NormalSelectionService
    {
        public OpenKanSelectionService(MahjongPlayer player, MahjongBoard board) : base(player,
            board)
        {
        }

        public override void AddToHand(MahjongTile2DComponent tile, Transform handParent)
        {
            Player.HandOpen = true;
            Player.ExposedHand.Add(new List<MahjongTile>());
            for (var i = 0; i < 4; i++)
            {
                var clonedTile = CloneTileToTransform(tile, handParent);
                Player.ExposedHand.Last().Add(clonedTile.Tile);
            }
        }

        public override bool CanSelect(MahjongTile tile)
        {
            if (Player.HandTileCount > 10)
                return false;

            if (AllTiles.Any(x => x.Name == tile.Name))
                return false;

            return true;
        }
    }
}