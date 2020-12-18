using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class PlayerHandComponent : MonoBehaviour
    {
        private List<MahjongTileComponent> _tiles = new List<MahjongTileComponent>();

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

            StartCoroutine(SortHand());
        }

        private IEnumerator SortHand()
        {
            _tiles = _tiles.OrderBy(GetTileOrder).ToList();

            yield return _tiles.Select(x => x.transform).ToList().MoveToParent(transform);
        }

        private static int GetTileOrder(MahjongTileComponent tileComponent)
        {
            return (int) tileComponent.Tile.Type * 20 + tileComponent.Tile.Number;
        }
    }
}