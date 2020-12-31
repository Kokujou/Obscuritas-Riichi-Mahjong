using System.Collections.Generic;
using ObscuritasRiichiMahjong.Animations;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class PlayerHandComponent : MonoBehaviour
    {
        private readonly List<MahjongTileComponent> _tiles = new List<MahjongTileComponent>();

        public void Initialize()
        {
            transform.Rotate(45, 0, 0);

            foreach (Transform child in transform)
            {
                var mahjongTile = child.GetComponent<MahjongTileComponent>();

                if (!mahjongTile) continue;

                _tiles.Add(mahjongTile);
                if (mahjongTile)
                    mahjongTile.Selectable = true;
            }

            StartCoroutine(transform.SortHand());
        }
    }
}