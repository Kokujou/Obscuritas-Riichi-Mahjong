using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongTileComponent : MonoBehaviour
    {
        public bool Selectable;
        public MahjongTile Tile;

        public void Initialize(MahjongTile tile)
        {
            Tile = tile;

            gameObject.layer = LayerMask.NameToLayer("MahjongTile");
        }

        public void HandleInput()
        {
            if (!Selectable) return;
            foreach (Transform side in transform)
                side.gameObject.layer = LayerMask.NameToLayer("HoveredTile");
        }

        public void HandleMouseOut()
        {
            if (!Selectable) return;
            foreach (Transform side in transform)
                side.gameObject.layer = 0;
        }

        public static MahjongTileComponent AddToObject(GameObject target, MahjongTile tile)
        {
            var component = target.AddComponent<MahjongTileComponent>();
            component.Initialize(tile);
            return component;
        }
    }
}