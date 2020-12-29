using ObscuritasRiichiMahjong.PointCalculation.Services;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class InputTypeSetterComponent : MonoBehaviour
    {
        public SelectableButton ChiButton;
        public SelectableButton PonButton;
        public SelectableButton KanButton;
        public SelectableButton OpenKanButton;

        public PointCalculator PointCalculator;

        public void Start()
        {
            PonButton.IsSelected = InputAsPon;
            ChiButton.IsSelected = InputAsChi;
            KanButton.IsSelected = InputAsHiddenKan;
            OpenKanButton.IsSelected = InputAsOpenKan;
        }

        private bool InputAsPon()
        {
            return PointCalculator.TileSelectionService.GetType() == typeof(PonSelectionService);
        }

        private bool InputAsChi()
        {
            return PointCalculator.TileSelectionService.GetType() == typeof(ChiSelectionService);
        }

        private bool InputAsHiddenKan()
        {
            return PointCalculator.TileSelectionService.GetType() == typeof(KanSelectionService);
        }

        private bool InputAsOpenKan()
        {
            return PointCalculator.TileSelectionService.GetType() ==
                   typeof(OpenKanSelectionService);
        }

        public void SetPonInput()
        {
            if (InputAsPon())
                SetNormalInput();
            else
                PointCalculator.TileSelectionService =
                    new PonSelectionService(PointCalculator.Player, PointCalculator.Board);

            PointCalculator.UpdateSelectableTiles();
        }

        public void SetChiInput()
        {
            if (InputAsChi())
                SetNormalInput();
            else
                PointCalculator.TileSelectionService =
                    new ChiSelectionService(PointCalculator.Player, PointCalculator.Board);

            PointCalculator.UpdateSelectableTiles();
        }

        public void SetHiddenKanInput()
        {
            if (InputAsHiddenKan())
                SetNormalInput();
            else
                PointCalculator.TileSelectionService =
                    new KanSelectionService(PointCalculator.Player, PointCalculator.Board);

            PointCalculator.UpdateSelectableTiles();
        }

        public void SetOpenKanInput()
        {
            if (InputAsOpenKan())
                SetNormalInput();
            else
                PointCalculator.TileSelectionService =
                    new OpenKanSelectionService(PointCalculator.Player, PointCalculator.Board);

            PointCalculator.UpdateSelectableTiles();
        }

        private void SetNormalInput()
        {
            PointCalculator.TileSelectionService =
                new NormalSelectionService(PointCalculator.Player, PointCalculator.Board);

            PointCalculator.UpdateSelectableTiles();
        }
    }
}