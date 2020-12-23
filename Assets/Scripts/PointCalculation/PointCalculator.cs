using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.PointCalculation.Components;
using ObscuritasRiichiMahjong.Rules.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation
{
    public class PointCalculator : MonoBehaviour
    {
        public MahjongBoard Board = new MahjongBoard();
        public Button Calculate;
        public Transform CalculatedHandsParent;

        public Transform HandParent;

        public MahjongPlayer Player =
            new MahjongPlayer(new List<MahjongTile>(), CardinalPoint.East);

        public Transform PoolParent;

        public GameObject ResultsViewTemplate;
        public List<MahjongTile> Tiles;

        public void AddToHand(MahjongTile2DComponent tile)
        {
            var newObject = Instantiate(tile.gameObject, HandParent);
            var clickedTile = newObject.GetComponent<MahjongTile2DComponent>();
            var button = newObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => clickedTile.RemoveFromHand());

            var handCount = Player.Hand.Count(x => x.Name == clickedTile.Tile.Name);
            if (handCount >= 3)
                tile.gameObject.GetComponent<Button>().interactable = false;
            if (handCount < 4)
                Player.Hand.Add(clickedTile.Tile);

            Player.Hand = Player.Hand.OrderBy(x => x.GetTileOrder()).ToList();

            var children = HandParent
                .Cast<Transform>()
                .Select(child => child.gameObject.GetComponent<MahjongTile2DComponent>()).ToList();
            foreach (var tileComponent in children)
                tileComponent.transform.SetSiblingIndex(Player.Hand.IndexOf(tileComponent.Tile));

            if (Player.Hand.Count < 14) return;

            foreach (Transform child in PoolParent)
            {
                var tileButton = child.gameObject.GetComponent<Button>();
                if (tileButton)
                    tileButton.interactable = false;
                Calculate.interactable = true;
            }
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
            Tiles = Tiles.OrderBy(x => x.Type).ToList();
            foreach (var tile in Tiles)
            {
                var tileObject = new GameObject();
                tileObject.transform.parent = PoolParent;
                MahjongTile2DComponent.AddToObject(tileObject, tile);
            }
        }
    }
}