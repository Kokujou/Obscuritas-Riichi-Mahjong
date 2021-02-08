using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Global
{
    public class PrefabCollection : MonoBehaviour
    {
        public static PrefabCollection Instance;

        public GameObject MahjongTileTemplate;
        public List<MahjongTile> TileSet;
        public GameObject ActionButtonTemplate;

        public PrefabCollection()
        {
            Instance = this;
        }
    }
}