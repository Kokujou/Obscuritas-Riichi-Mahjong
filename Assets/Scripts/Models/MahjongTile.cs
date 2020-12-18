using ObscuritasRiichiMahjong.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongTile : ScriptableObject
    {
        public Material Material;
        public string Name;
        public byte Number = 0;
        public bool Red;
        public MahjongTileType Type;
    }
}
