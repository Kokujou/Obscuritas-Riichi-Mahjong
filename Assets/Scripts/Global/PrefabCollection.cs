using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Global
{
    public class PrefabCollection : MonoBehaviour
    {
        public static PrefabCollection Instance;

        public GameObject MahjongTileTemplate;
        public List<MahjongTile> TileSet;

        public UDictionary<CallType, GameObject> ActionButtonDictionary;

        [Serializable]
        public struct ActionButton
        {
            public CallType Type;
            public GameObject ActionButtonPrefab;
        }


        public PrefabCollection()
        {
            Instance = this;
        }
    }
}