using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using ObscuritasRiichiMahjong.PointCalculation.Services.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Services
{
    public class NormalSelectionService : ITileSelectionService
    {
        protected readonly MahjongPlayer Player;
        protected readonly MahjongBoard Board;

        protected IEnumerable<MahjongTile> AllTiles => Player.Hand
            .Concat(Player.ExposedHand.SelectMany(x => x))
            .Concat(Player.HiddenKan.SelectMany(x => x));

        public NormalSelectionService(MahjongPlayer player, MahjongBoard board)
        {
            Player = player;
            Board = board;
        }

        public MahjongTile2DComponent CloneTileToTransform(MahjongTile2DComponent tile,
            Transform parent)
        {
            var newObject = Object.Instantiate(tile.gameObject, parent);
            var clonedTile = newObject.GetComponent<MahjongTile2DComponent>();
            var button = newObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => clonedTile.RemoveFromHand());

            return clonedTile;
        }

        public virtual void AddToHand(MahjongTile2DComponent tile, Transform handParent)
        {
            var clonedTile = CloneTileToTransform(tile, handParent);

            Player.Hand.Add(clonedTile.Tile);
            Board.WinningTile = clonedTile.Tile;
        }

        public virtual bool CanSelect(MahjongTile tile)
        {
            if (Player.HandTileCount >= 14)
                return false;

            var handCount = AllTiles.Count(x => x.Name == tile.Name);
            if (handCount > 3)
                return false;

            return true;
        }
    }
}