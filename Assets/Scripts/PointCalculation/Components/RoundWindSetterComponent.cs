using ObscuritasRiichiMahjong.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class RoundWindSetterComponent : MonoBehaviour
    {
        public PointCalculator PointCalculator;

        public SelectableButton EastWind;
        public SelectableButton WestWind;
        public SelectableButton SouthWind;
        public SelectableButton NorthWind;

        public void Start()
        {
            EastWind.IsSelected = () => PointCalculator.Board.CardinalPoint == CardinalPoint.East;
            WestWind.IsSelected = () => PointCalculator.Board.CardinalPoint == CardinalPoint.West;
            SouthWind.IsSelected = () => PointCalculator.Board.CardinalPoint == CardinalPoint.South;
            NorthWind.IsSelected = () => PointCalculator.Board.CardinalPoint == CardinalPoint.North;
        }

        public void SetEastRound()
        {
            PointCalculator.Board.CardinalPoint = CardinalPoint.East;
        }

        public void SetWestRound()
        {
            PointCalculator.Board.CardinalPoint = CardinalPoint.West;
        }

        public void SetSouthRound()
        {
            PointCalculator.Board.CardinalPoint = CardinalPoint.South;
        }

        public void SetNorthRound()
        {
            PointCalculator.Board.CardinalPoint = CardinalPoint.North;
        }
    }
}