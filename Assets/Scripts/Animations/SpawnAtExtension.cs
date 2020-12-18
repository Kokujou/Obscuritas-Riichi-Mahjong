using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class SpawnAtExtension
    {
        public static GameObject SpawnAtRandom(this GameObject template, Transform parent)
        {
            var tileObject = Object.Instantiate(template, parent, false);

            var startXPos = Random.Range(-5, 5);
            var startYPos = Random.Range(-5, 5);

            var startXRot = Random.Range(0, 360);
            var startYRot = Random.Range(0, 360);
            var startZRot = Random.Range(0, 360);

            tileObject.transform.localPosition = new Vector3(startXPos, startYPos, 0);
            tileObject.transform.localRotation =
                Quaternion.Euler(new Vector3(startXRot, startYRot, startZRot));
            return tileObject;
        }
    }
}