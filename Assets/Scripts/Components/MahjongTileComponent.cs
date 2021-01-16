using System.Linq;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongTileComponent : MonoBehaviour
    {
        public bool Selectable;
        public MahjongTile Tile;

        private bool _selected;

        public void Initialize(MahjongTile tile)
        {
            Tile = tile;
            name = tile.Name;
            var tileFace = transform.Find("Top");
            tileFace.GetComponent<MeshRenderer>().material = tile.Material;

            var tileLabel = tileFace.GetComponentInChildren<TextMesh>();
            tileLabel.text = tile.GetTileLetter();
            if (tile.Red)
                tileLabel.color = Color.black;

            gameObject.layer = LayerMask.NameToLayer("MahjongTile");
        }

        public void HandleInput()
        {
            if (!Selectable) return;
            if (Input.GetMouseButtonDown(0))
                _selected = true;

            foreach (var side in transform.GetComponentsInChildren<Transform>()
                .Where(x => x.GetInstanceID() != transform.GetInstanceID()))
                side.gameObject.layer = LayerMask.NameToLayer("HoveredTile");

            if (_selected && Input.GetMouseButtonUp(0))
                HandleTileClick();
        }

        private void HandleTileClick()
        {
            var playerHand = GetComponentInParent<MahjongPlayerComponent>();

            if (!playerHand)
                return;

            playerHand.DiscardTile(this);
        }

        public void HandleMouseOut()
        {
            if (!Selectable) return;
            _selected = false;

            foreach (var side in transform.GetComponentsInChildren<Transform>()
                .Where(x => x.GetInstanceID() != transform.GetInstanceID()))
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