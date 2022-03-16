using System.Collections;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class CallAnimations
    {
        public static IEnumerator ExposeAndThrowTile(this MahjongTileComponent tile, float duration)
        {
            tile.transform.localRotation = Quaternion.Euler(0, 0, 0);
            yield return null;
            var rigidBody = tile.GetComponent<Rigidbody>();
            yield return rigidBody.ExposeTile(duration / 2);
            yield return rigidBody.ThrowToRightHandSide(duration / 2);

            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.isKinematic = true;
        }

        private static IEnumerator ExposeTile(this Rigidbody rigidBody, float duration)
        {
            var transform = rigidBody.transform;
            rigidBody.isKinematic = false;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            rigidBody.AddForceAtPosition(transform.rotation * Vector3.forward * 30f,
                transform.position + transform.rotation * Vector3.up * 1f, ForceMode.Impulse);

            yield return new WaitForSeconds(duration);
        }

        public static IEnumerator ThrowToRightHandSide(this Rigidbody rigidBody, float duration)
        {
            rigidBody.isKinematic = false;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                                    RigidbodyConstraints.FreezeRotation;
            rigidBody.AddForce(Vector3.right * 300, ForceMode.Impulse);

            yield return null;

            var tile = rigidBody.GetComponent<MahjongTileComponent>();

            async void CollisionExitEvent(object sender, Collision other)
            {
                if (other.gameObject.layer != LayerMask.NameToLayer("MahjongTile") &&
                    other.gameObject.layer != LayerMask.NameToLayer("TableBorder")) return;

                await Task.Delay(200);

                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
            }

            tile.CollisionExit += CollisionExitEvent;

            yield return new WaitForSeconds(duration);

            tile.CollisionExit -= CollisionExitEvent;
        }
    }
}