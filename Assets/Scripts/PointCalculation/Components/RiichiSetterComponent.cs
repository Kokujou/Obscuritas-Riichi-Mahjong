using ObscuritasRiichiMahjong.Core.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class RiichiSetterComponent : MonoBehaviour
    {
        public SelectableButton RiichiButton;
        public SelectableButton DoubleRiichiButton;
        public SelectableButton IppatsuButton;

        public PointCalculator PointCalculator;

        private bool _riichiSet;

        public void Start()
        {
            RiichiButton.IsSelected = IsRiichi;
            DoubleRiichiButton.IsSelected = IsDoubleRiichi;
            IppatsuButton.IsSelected = IsIppatsu;
        }

        private bool IsRiichi()
        {
            return PointCalculator.Player.Riichi == RiichiType.Riichi;
        }

        private bool IsDoubleRiichi()
        {
            return PointCalculator.Player.Riichi == RiichiType.DoubleRiichi;
        }

        private bool IsIppatsu()
        {
            return PointCalculator.Player.Riichi != RiichiType.NoRiichi &&
                   PointCalculator.Player.LastMoveType == MoveType.Riichi;
        }

        public void SetRiichi()
        {
            PointCalculator.Player.Riichi = IsRiichi() ? RiichiType.NoRiichi : RiichiType.Riichi;
        }

        public void SetDoubleRiichi()
        {
            PointCalculator.Player.Riichi =
                IsDoubleRiichi() ? RiichiType.NoRiichi : RiichiType.DoubleRiichi;
        }

        public void SetIppatsu()
        {
            if (IsIppatsu())
            {
                PointCalculator.Player.LastMoveType = MoveType.Normal;
            }
            else
            {
                if (PointCalculator.Player.Riichi == RiichiType.NoRiichi)
                    PointCalculator.Player.Riichi = RiichiType.Riichi;
                PointCalculator.Player.LastMoveType = MoveType.Riichi;
            }
        }
    }
}