using System;
using ObscuritasRiichiMahjong.Data;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongTile : ScriptableObject, IEquatable<MahjongTile>
    {
        public Material Material;
        public string Name;
        public byte Number = 0;
        public bool Red;
        public MahjongTileType Type;

        public bool Equals(MahjongTile other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Name == other.Name;
        }

        public static bool operator ==(MahjongTile a, MahjongTile b)
        {
            return a?.Equals(b) ?? true;
        }

        public static bool operator !=(MahjongTile a, MahjongTile b)
        {
            return !(a == b);
        }

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

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MahjongTile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}