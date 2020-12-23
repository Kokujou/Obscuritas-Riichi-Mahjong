using ObscuritasRiichiMahjong.Models;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class MahjongTile2DComponent : MonoBehaviour
    {
        public MahjongTile Tile;

        public void Initialize(MahjongTile tile)
        {
            Tile = tile;
            name = tile.Name;

            var image = gameObject.AddComponent<RawImage>();
            image.texture = tile.Material.mainTexture;

            var button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(AddToHand);
        }

        public void AddToHand()
        {
            FindObjectOfType<PointCalculator>()
                .AddToHand(gameObject.GetComponent<MahjongTile2DComponent>());
        }

        public void RemoveFromHand()
        {
            FindObjectOfType<PointCalculator>()
                .RemoveFromHand(gameObject.GetComponent<MahjongTile2DComponent>());
        }

        public static MahjongTile2DComponent AddToObject(GameObject target, MahjongTile tile)
        {
            var component = target.AddComponent<MahjongTile2DComponent>();
            component.Initialize(tile);
            return component;
        }
    }
}