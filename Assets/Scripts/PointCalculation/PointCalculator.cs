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

        public MahjongPlayer Player = new MahjongPlayer(CardinalPoint.East);

        public MahjongBoard Board = new MahjongBoard();

        public ITileSelectionService TileSelectionService;

        public Transform CalculatedHandsParent;
        public Transform HandParent;
        public Transform PoolParent;

        public Button Calculate;

        public GameObject ResultsViewTemplate;
        public List<MahjongTile> Tiles;

        private void Start()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Player.Wall.Add(new MahjongTile());
            Board.CurrentRound = 2;
            Board.WinningMoveType = WinningMoveType.Tsumo;

            Tiles = Tiles.OrderBy(x => x.Type).ToList();
            foreach (var tile in Tiles)
            {
                var tileObject = new GameObject();
                tileObject.transform.SetParent(PoolParent, false);
                MahjongTile2DComponent.AddToObject(tileObject, tile);
            }

            TileSelectionService = new NormalSelectionService(Player, Board);
        }

        public void UpdateSelectableTiles()
        {
            var selectableTiles = PoolParent
                .Cast<Transform>()
                .Select(child => child.gameObject.GetComponent<MahjongTile2DComponent>());

            foreach (var tile in selectableTiles)
                tile.GetComponent<Button>().interactable =
                    TileSelectionService.CanSelect(tile.Tile);
        }

        public void AddToHand(MahjongTile2DComponent tile)
        {
            TileSelectionService.AddToHand(tile, HandParent);
            UpdateSelectableTiles();
            SortHand();
            if (Player.HandTileCount == 14)
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
            foreach (Transform handSplitResultComponent in CalculatedHandsParent)
                Destroy(handSplitResultComponent.gameObject);

            Destroy(tile.gameObject);
            Player.Hand.Remove(tile.Tile);

            UpdateSelectableTiles();
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

        public void ClearBoard()
        {
            foreach (Transform handSplitResultComponent in CalculatedHandsParent)
                Destroy(handSplitResultComponent.gameObject);

            foreach (Transform child in HandParent)
                Destroy(child.gameObject);

            foreach (Transform child in PoolParent)
                Destroy(child.gameObject);

            Player = new MahjongPlayer(CardinalPoint.East);
            Board = new MahjongBoard();

            InitializeBoard();
        }
    }
}