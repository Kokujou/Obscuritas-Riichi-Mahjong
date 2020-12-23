using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong
{
    public class PointCalculator : MonoBehaviour
    {
        public static Transform HandParentTransform;
        public static Transform PoolParentTransform;
        public static Button CalculateButton;

        public static MahjongPlayer Player =
            new MahjongPlayer(new List<MahjongTile>(), CardinalPoint.East);

        public Button Calculate;

        public Transform HandParent;
        public Transform PoolParent;
        public List<MahjongTile> Tiles;

        public static void AddToHand(MahjongTile2DComponent tile)
        {
            var newObject = Instantiate(tile.gameObject, HandParentTransform);
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

            var children = HandParentTransform
                .Cast<Transform>()
                .Select(child => child.gameObject.GetComponent<MahjongTile2DComponent>()).ToList();
            foreach (var tileComponent in children)
                tileComponent.transform.SetSiblingIndex(Player.Hand.IndexOf(tileComponent.Tile));

            if (Player.Hand.Count < 14) return;

            foreach (Transform child in PoolParentTransform)
            {
                var tileButton = child.gameObject.GetComponent<Button>();
                if (tileButton)
                    tileButton.interactable = false;
                CalculateButton.interactable = true;
            }
        }

        public static void RemoveFromHand(MahjongTile2DComponent tile)
        {
            Destroy(tile.gameObject);
            Player.Hand.Remove(tile.Tile);

            foreach (Transform child in PoolParentTransform)
            {
                var tileButton = child.gameObject.GetComponent<Button>();
                if (tileButton)
                    tileButton.interactable = Player.Hand.Count(x => x.Name == tile.Tile.Name) < 4;
                CalculateButton.interactable = false;
            }
        }

        public static void CalculatePoints()
        {
            foreach (var hand in Player.GetValidHands()) Debug.Log(hand.Stringify());
        }

        // Start is called before the first frame update
        private void Start()
        {
            HandParentTransform = HandParent;
            PoolParentTransform = PoolParent;
            CalculateButton = Calculate;

            Tiles = Tiles.OrderBy(x => x.Type).ToList();
            for (var index = 0; index < Tiles.Count; index++)
            {
                var tile = Tiles[index];
                var tileObject = new GameObject();
                tileObject.transform.parent = PoolParent;
                MahjongTile2DComponent.AddToObject(tileObject, tile);
            }
        }
    }
}