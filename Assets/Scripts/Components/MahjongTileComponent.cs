using ObscuritasRiichiMahjong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongTileComponent : MonoBehaviour
    {
        private static readonly List<MahjongTileComponent> _hoveredTiles = new();
        public const float SizeX = 1.1f;

        private static Color InactiveColor => new Color(.75f, .75f, .75f);
        private static Color ActiveColor => new Color(.75f, .75f, .75f);
        private static Color HoverColor => new Color(.9f, .9f, .9f);

        public static GameObject MahjongTileTemplate;
        public MahjongTile Tile;
        public event EventHandler<Collision> CollisionExit = delegate { };

        public static void UnhoverAll()
        {
            foreach (var tile in _hoveredTiles) tile.SetActive();
            _hoveredTiles.Clear();
        }

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

        public void SetHovered()
        {
            _hoveredTiles.Add(this);
            var hoveredPropertyBlock = new MaterialPropertyBlock();
            hoveredPropertyBlock.SetColor("_Color", HoverColor);
            foreach (var side in transform.GetComponentsInChildren<MeshRenderer>().Where(x => x.tag != "TileBottom"))
                side.SetPropertyBlock(hoveredPropertyBlock);
        }

        public void SetInactive()
        {
            var inactivePropertyBlock = new MaterialPropertyBlock();
            inactivePropertyBlock.SetColor("_Color", InactiveColor);
            foreach (var side in transform.GetComponentsInChildren<MeshRenderer>().Where(x => x.tag != "TileBottom"))
                side.SetPropertyBlock(inactivePropertyBlock);

        }

        private void SetActive()
        {
            foreach (var side in transform.GetComponentsInChildren<MeshRenderer>()) side.SetPropertyBlock(null);
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionExit?.Invoke(this, other);
        }


        public static MahjongTileComponent FromTile(MahjongTile tile)
        {
            if (!MahjongTileTemplate)
                throw new ArgumentException(
                    "before using this component, please initialize the MahjongTileTemplate property");

            var tileObject = Instantiate(MahjongTileTemplate);
            var component = tileObject.AddComponent<MahjongTileComponent>();
            component.Initialize(tile);
            return component;
        }
    }
}