using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class PonAnimation
    {
        public static IEnumerator DoPonAnimation(this MahjongTileComponent discardedTile,
            MahjongPlayerComponentBase player, float duration)
        {
            var tiles = player.HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == discardedTile.Tile);

            foreach (var tile in tiles)
            {
                tile.GetComponent<Rigidbody>().isKinematic = false;
                yield return tile.gameObject.InterpolationAnimation(duration / 3,
                    tile.transform.position + Vector3.forward, Vector3.right * 90);
            }

            yield return new WaitForSeconds(.5f);

            foreach (var tile in tiles)
            {
                var rigidBody = tile.GetComponent<Rigidbody>();
                rigidBody.AddForce(rigidBody.mass * 25, 0, 0, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(.5f);
        }
    }
}