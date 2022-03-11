using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class CallAnimations
    {
        public static IEnumerator DoPonAnimation(this MahjongTileComponent discardedTile,
            MahjongPlayerComponentBase player, float duration, Vector3 forceOffset)
        {
            var tiles = player.HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == discardedTile.Tile).Select(x => x.GetComponent<Rigidbody>());

            foreach (var tile in tiles)
                player.StartCoroutine(tile.PonAnimation(3));

            yield return new WaitForSeconds(3);
        }

        private static IEnumerator PonAnimation(this Rigidbody tile, float duration)
        {
            yield return tile.ExposeTile(duration / 2);
            yield return tile.ThrowToRightHandSide(duration / 2);
        }

        private static IEnumerator ExposeTile(this Rigidbody rigidBody, float duration)
        {
            rigidBody.isKinematic = false;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            rigidBody.AddForceAtPosition(Vector3.forward * 10,
                rigidBody.transform.position + rigidBody.transform.rotation * Vector3.up * 0.4f, ForceMode.Impulse);
            yield return new WaitForSeconds(duration);

            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.isKinematic = true;
        }

        public static IEnumerator ThrowToRightHandSide(this Rigidbody rigidBody, float duration)
        {
            rigidBody.isKinematic = false;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                                    RigidbodyConstraints.FreezeRotation;
            rigidBody.AddForce(Vector3.right * 100, ForceMode.Impulse);

            yield return new WaitForSeconds(duration);

            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.isKinematic = true;
        }
    }
}