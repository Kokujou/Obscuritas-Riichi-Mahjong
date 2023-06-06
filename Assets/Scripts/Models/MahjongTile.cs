using ObscuritasRiichiMahjong.Core.Data;
using System;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Models
{
    public class MahjongTile : ScriptableObject, IEquatable<MahjongTile>
    {
        public bool Green;
        public Material Material;
        public string Name;
        public byte Number;
        public bool Red;
        public bool Exposed;
        public MahjongTileType Type;

        public bool IsNumbered =>
            Type == MahjongTileType.Bamboo || Type == MahjongTileType.Circle || Type == MahjongTileType.Kanji;

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
            if (Type == MahjongTileType.Dragon || Type == MahjongTileType.Wind) return true;

            if (Number == 1 || Number == 9)
                return true;

            return false;
        }

        public int GetTileOrder()
        {
            return (int)Type * 20 + Number;
        }

        public byte GetTilesLeftInGame(MahjongBoard board)
        {
            var visibleTiles = board.Players.Values
                .SelectMany(x => x.ExposedHand
                    .SelectMany(x => x)
                    .Concat(x.DiscardedTiles))
                .Concat(board.KanDora
                .Where(x => x.Exposed));

            return (byte)visibleTiles.Count(x => x == this);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MahjongTile)obj);
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

        public string GetTileLetter()
        {
            if (Number > 0 && Number < 10)
                return Number.ToString();

            return Name.First().ToString();
        }

        public bool Equals(MahjongTile other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Type != other.Type) return false;

            if (IsNumbered)
                return Number == other.Number;

            return base.Equals(other) && Name == other.Name;
        }
    }
}