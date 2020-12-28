using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Services.Interfaces
{
    public interface ITileSelectionService
    {
        void AddToHand(MahjongTile2DComponent tile, Transform handParent);
        bool CanSelect(MahjongTile tile);
    }
}