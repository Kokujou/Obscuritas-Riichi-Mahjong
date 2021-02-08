using ObscuritasRiichiMahjong.Core.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class SeatWindSetterComponent : MonoBehaviour
    {
        public PointCalculator PointCalculator;

        public SelectableButton EastWind;
        public SelectableButton WestWind;
        public SelectableButton SouthWind;
        public SelectableButton NorthWind;

        public void Start()
        {
            EastWind.IsSelected = () => PointCalculator.Player.CardinalPoint == CardinalPoint.East;
            WestWind.IsSelected = () => PointCalculator.Player.CardinalPoint == CardinalPoint.West;
            SouthWind.IsSelected =
                () => PointCalculator.Player.CardinalPoint == CardinalPoint.South;
            NorthWind.IsSelected =
                () => PointCalculator.Player.CardinalPoint == CardinalPoint.North;
        }

        public void SetEastSeat()
        {
            PointCalculator.Player.CardinalPoint = CardinalPoint.East;
        }

        public void SetWestSeat()
        {
            PointCalculator.Player.CardinalPoint = CardinalPoint.West;
        }

        public void SetSouthSeat()
        {
            PointCalculator.Player.CardinalPoint = CardinalPoint.South;
        }

        public void SetNorthSeat()
        {
            PointCalculator.Player.CardinalPoint = CardinalPoint.North;
        }
    }
}