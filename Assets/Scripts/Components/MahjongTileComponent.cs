using System;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongTileComponent : MonoBehaviour
    {
        public static GameObject MahjongTileTemplate;

        public bool Selectable;
        public MahjongTile Tile;

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

        public void Hover()
        {
            foreach (var side in transform.GetComponentsInChildren<Transform>()
                .Where(x => x.GetInstanceID() != transform.GetInstanceID()))
                side.gameObject.layer = LayerMask.NameToLayer("HoveredTile");
        }

        public void Unhover()
        {
            foreach (var side in transform.GetComponentsInChildren<Transform>()
                .Where(x => x.GetInstanceID() != transform.GetInstanceID()))
                side.gameObject.layer = 0;
        }

        public static MahjongTileComponent FromTile(MahjongTile tile)
        {
            if (!MahjongTileTemplate)
                throw new ArgumentException(
                    "before using this component, please initialize the MahjongTileTemplate property");

            try
            {
                lock (MahjongTileTemplate)
                {
                    var tileObject = Instantiate(MahjongTileTemplate);
                    var component = tileObject.AddComponent<MahjongTileComponent>();
                    component.Initialize(tile);
                    return component;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }
    }
}