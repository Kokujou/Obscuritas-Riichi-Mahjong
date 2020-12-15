using ObscuritasRiichiMahjong.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongTile : ScriptableObject
    {
        public byte Number = 0;
        public MahjongTileType Type;
        public string Name;
        public Material Material;
        public bool Red;
    }
}
