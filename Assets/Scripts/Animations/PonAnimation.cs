using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class PonAnimation
    {
        [SuppressMessage("ReSharper", "BitwiseOperatorOnEnumWithoutFlags")]
        public static IEnumerator DoPonAnimation(this MahjongTileComponent discardedTile,
            MahjongPlayerComponentBase player, float duration, Vector3 forceOffset)
        {
            var tileBodies = player.HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == discardedTile.Tile).Select(x => x.GetComponent<Rigidbody>()).ToList();

            yield return tileBodies.ExposeTiles(duration / 2, forceOffset);

            //foreach (var rigidBody in tileBodies) rigidBody.AddForce(rigidBody.mass * 25, 0, 0, ForceMode.Impulse);

            yield return new WaitForSeconds(.5f);
        }

        private static IEnumerator ExposeTiles(this IEnumerable<Rigidbody> rigidBodies, float duration,
            Vector3 forceOffset)
        {
            foreach (var rigidBody in rigidBodies)
            {
                rigidBody.isKinematic = false;
                rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY |
                                        RigidbodyConstraints.FreezeRotationZ;

                var direction = Vector3.forward;
                var position = rigidBody.transform.position + rigidBody.transform.rotation * Vector3.up;
                rigidBody.AddForceAtPosition(direction * 10, position + forceOffset,
                    ForceMode.Impulse);
            }

            yield return new WaitForSeconds(duration / 3 + .5f);
        }
    }
}