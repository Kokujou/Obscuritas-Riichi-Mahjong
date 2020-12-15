using ObscuritasRiichiMahjong.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class MahjongMain : MonoBehaviour
    {
        public Transform TileSpawnPoint;
        public GameObject MahjongTileTemplate;
        public List<MahjongTile> TileSet;

        private IEnumerator DropTiles()
        {
            foreach (var tile in TileSet)
            {
                var tileObject = Instantiate(MahjongTileTemplate, TileSpawnPoint, false);

                var startXPos = Random.Range(-5, 5);
                var startYPos = Random.Range(-5, 5);

                var startXRot = Random.Range(0, 360);
                var startYRot = Random.Range(0, 360);
                var startZRot = Random.Range(0, 360);

                tileObject.transform.localPosition = new Vector3(startXPos, startYPos, 0);
                tileObject.transform.localRotation = Quaternion.Euler(new Vector3(startXRot, startYRot, startZRot));
                var tileFace = tileObject.transform.Find("Top");
                tileFace.GetComponent<MeshRenderer>().material = tile.Material;

                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        // Start is called before the first frame update
        void Start()
        {
            var tilesToMultiply = TileSet.Where(x => x.Number != 5).ToList();
            var nonRedFives = TileSet.Where(x => x.Number == 5 && !x.Red).ToList();
            for (int i = 0; i < 3; i++)
                TileSet.AddRange(tilesToMultiply);

            for (int i = 0; i < 2; i++)
                TileSet.AddRange(nonRedFives);

            Debug.Log(TileSet.Count);


            StartCoroutine(DropTiles());
        }
    }
}
