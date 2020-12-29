using ObscuritasRiichiMahjong.Data;
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
            return PointCalculator.Player.Riichi && PointCalculator.Board.CurrentRound != 1;
        }

        private bool IsDoubleRiichi()
        {
            return PointCalculator.Player.Riichi && PointCalculator.Board.CurrentRound == 1;
        }

        private bool IsIppatsu()
        {
            return PointCalculator.Player.Riichi &&
                   PointCalculator.Player.LastMoveType == MoveType.Riichi;
        }

        public void SetRiichi()
        {
            if (IsRiichi())
            {
                PointCalculator.Player.Riichi = false;
            }
            else
            {
                PointCalculator.Player.Riichi = true;
                PointCalculator.Board.CurrentRound = 2;
            }
        }

        public void SetDoubleRiichi()
        {
            if (IsDoubleRiichi())
            {
                PointCalculator.Player.Riichi = false;
                PointCalculator.Board.CurrentRound = 2;
            }
            else
            {
                PointCalculator.Player.Riichi = true;
                PointCalculator.Board.CurrentRound = 1;
            }
        }

        public void SetIppatsu()
        {
            if (IsIppatsu())
            {
                PointCalculator.Player.LastMoveType = MoveType.Normal;
            }
            else
            {
                PointCalculator.Player.Riichi = true;
                PointCalculator.Player.LastMoveType = MoveType.Riichi;
            }
        }
    }
}