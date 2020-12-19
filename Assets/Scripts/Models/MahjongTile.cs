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

        public bool IsTerminal()
        {
            if (Type == MahjongTileType.Dragon)
                return true;
            if (Type == MahjongTileType.Wind)
                return true;

            if (Number == 1 || Number == 9)
                return true;

            return false;
        }

        public int GetTileOrder()
        {
            return (int) Type * 20 + Number;
        }
    }
}