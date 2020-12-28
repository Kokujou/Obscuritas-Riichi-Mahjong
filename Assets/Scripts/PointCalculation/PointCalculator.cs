using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using ObscuritasRiichiMahjong.PointCalculation.Services;
using ObscuritasRiichiMahjong.PointCalculation.Services.Interfaces;
using ObscuritasRiichiMahjong.Rules.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation
{
    public class PointCalculator : MonoBehaviour
    {
        public static readonly Color TileBackColor = new Color(0.7450981f, 0.5686275f, 0.2117647f);

        public MahjongBoard Board = new MahjongBoard();
        public Button Calculate;
        public Transform CalculatedHandsParent;

        public Transform HandParent;

        public MahjongPlayer Player =
            new MahjongPlayer(new List<MahjongTile>(), CardinalPoint.East);

        public Transform PoolParent;

        public GameObject ResultsViewTemplate;
        public List<MahjongTile> Tiles;

        public ButtonGroup ActionButtonGroup;
        public Button ChiButton;
        public Button PonButton;
        public Button KanButton;
        public Button OpenKanButton;

        private ITileSelectionService _tileSelectionService;

        private PonSelectionService _ponSelectionService;
        private ChiSelectionService _chiSelectionService;
        private KanSelectionService _kanSelectionService;
        private OpenKanSelectionService _openKanSelectionService;
        private NormalSelectionService _normalSelectionService;

        public void ChangeTileSelectionType(Button selection)
        {
            if (selection == null)
                _tileSelectionService = _normalSelectionService;
            else if (selection.GetInstanceID() == ChiButton.GetInstanceID())
                _tileSelectionService = _chiSelectionService;
            else if (selection.GetInstanceID() == PonButton.GetInstanceID())
                _tileSelectionService = _ponSelectionService;
            else if (selection.GetInstanceID() == KanButton.GetInstanceID())
                _tileSelectionService = _kanSelectionService;
            else if (selection.GetInstanceID() == OpenKanButton.GetInstanceID())
                _tileSelectionService = _openKanSelectionService;

            UpdateSelectableTiles();
        }

        public void UpdateSelectableTiles()
        {
            var selectableTiles = PoolParent
                .Cast<Transform>()
                .Select(child => child.gameObject.GetComponent<MahjongTile2DComponent>());

            foreach (var tile in selectableTiles)
                tile.GetComponent<Button>().interactable =
                    _tileSelectionService.CanSelect(tile.Tile);
        }

        public void AddToHand(MahjongTile2DComponent tile)
        {
            _tileSelectionService.AddToHand(tile, HandParent);
            UpdateSelectableTiles();
            SortHand();
            if (_normalSelectionService.HandTileCount == 14)
                Calculate.interactable = true;
        }

        public void SortHand()
        {
            Player.Hand = Player.Hand.OrderBy(x => x.GetTileOrder()).ToList();

            var children = HandParent
                .Cast<Transform>()
                .Select(child => child.gameObject.GetComponent<MahjongTile2DComponent>()).ToList();
            foreach (var tileComponent in children)
                tileComponent.transform.SetSiblingIndex(Player.Hand.IndexOf(tileComponent.Tile));
        }

        public void RemoveFromHand(MahjongTile2DComponent tile)
        {
            Destroy(tile.gameObject);
            Player.Hand.Remove(tile.Tile);

            foreach (Transform child in PoolParent)
            {
                var tileButton = child.gameObject.GetComponent<Button>();
                if (tileButton)
                    tileButton.interactable = Player.Hand.Count(x => x.Name == tile.Tile.Name) < 4;
                Calculate.interactable = false;
            }
        }

        public void CalculatePoints()
        {
            foreach (var hand in Player.Hand.GetValidHands())
            {
                var calculatedHand = Instantiate(ResultsViewTemplate, CalculatedHandsParent);
                var resultComponent = calculatedHand.GetComponent<HandSplitResultComponent>();
                resultComponent.Load(hand, Player, Board);
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            ActionButtonGroup.OnSelectionChange = ChangeTileSelectionType;

            Board.WinningMoveType = WinningMoveType.Tsumo;
            Tiles = Tiles.OrderBy(x => x.Type).ToList();
            foreach (var tile in Tiles)
            {
                var tileObject = new GameObject();
                tileObject.transform.parent = PoolParent;
                MahjongTile2DComponent.AddToObject(tileObject, tile);
            }

            _ponSelectionService = new PonSelectionService(Player, Board);
            _chiSelectionService = new ChiSelectionService(Player, Board);
            _kanSelectionService = new KanSelectionService(Player, Board);
            _openKanSelectionService = new OpenKanSelectionService(Player, Board);
            _normalSelectionService = new NormalSelectionService(Player, Board);
            _tileSelectionService = _normalSelectionService;
        }
    }
}