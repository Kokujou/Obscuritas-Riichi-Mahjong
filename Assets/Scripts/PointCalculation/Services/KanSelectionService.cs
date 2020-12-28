using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Services
{
    public class KanSelectionService : NormalSelectionService
    {
        public KanSelectionService(MahjongPlayer player, MahjongBoard board) : base(player, board)
        {
        }

        public override void AddToHand(MahjongTile2DComponent tile, Transform handParent)
        {
            Player.HiddenKan.Add(new List<MahjongTile>());
            for (var i = 0; i < 4; i++)
            {
                var clonedTile = CloneTileToTransform(tile, handParent);
                Player.HiddenKan.Last().Add(clonedTile.Tile);
                if (i == 0 || i == 3)
                    HideTile(clonedTile);
            }
        }

        public void HideTile(MahjongTile2DComponent tile)
        {
            var image = tile.GetComponent<RawImage>();
            image.texture = null;
            image.color = PointCalculator.TileBackColor;
        }

        public override bool CanSelect(MahjongTile tile)
        {
            if (HandTileCount > 10)
                return false;

            if (AllTiles.Any(x => x.Name == tile.Name))
                return false;

            return true;
        }
    }
}