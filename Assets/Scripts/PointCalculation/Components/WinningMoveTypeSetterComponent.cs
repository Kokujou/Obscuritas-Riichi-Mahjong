using ObscuritasRiichiMahjong.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class WinningMoveTypeSetterComponent : MonoBehaviour
    {
        public SelectableButton TsumoButton;
        public SelectableButton RonButton;

        public PointCalculator PointCalculator;

        public bool Tsumo => PointCalculator.Board.WinningMoveType == WinningMoveType.Tsumo;

        public bool Ron => PointCalculator.Board.WinningMoveType == WinningMoveType.Ron;

        public void Start()
        {
            TsumoButton.IsSelected = () => Tsumo;
            RonButton.IsSelected = () => Ron;
        }

        public void SetTsumo()
        {
            PointCalculator.Board.WinningMoveType = WinningMoveType.Tsumo;
        }

        public void SetRon()
        {
            PointCalculator.Board.WinningMoveType = WinningMoveType.Ron;
        }
    }
}