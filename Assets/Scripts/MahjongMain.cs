using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class MahjongMain : MonoBehaviour
    {
        private static MahjongTileComponent _activeTile;
        public List<Transform> BankFirstRowSpawnPoints;
        public List<Transform> BankSecondRowSpawnPoints;
        public List<Transform> HandSpawnPoints;

        public GameObject MahjongTileTemplate;
        public List<MahjongTile> TileSet;
        public Transform TileSpawnPoint;
        public Transform UiPanel;

        private IEnumerator DropTiles()
        {
            foreach (var tile in TileSet)
            {
                var tileObject = MahjongTileTemplate.SpawnAtRandom(TileSpawnPoint);
                MahjongTileComponent.AddToObject(tileObject, tile);
                var tileFace = tileObject.transform.Find("Top");
                tileFace.GetComponent<MeshRenderer>().material = tile.Material;

                var tileLabel = tileFace.GetComponentInChildren<TextMesh>();
                tileLabel.text = tile.GetTileLetter();
                if (tile.Red)
                    tileLabel.color = Color.black;

                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(5);
        }

        private IEnumerator DealTiles()
        {
            const float duration = 3f;
            var transformList = TileSpawnPoint.Cast<Transform>().ToList();

            foreach (var handSpawnPoint in HandSpawnPoints)
                StartCoroutine(transformList.MoveToParent(handSpawnPoint, duration,
                    randomOrder: true, tileCount: 13));

            yield return new WaitForSeconds(duration);

            foreach (var bankSpawnPoint in BankFirstRowSpawnPoints)
                StartCoroutine(transformList.MoveToParent(bankSpawnPoint, duration,
                    randomOrder: true, tileCount: 10));

            yield return new WaitForSeconds(duration);

            foreach (var bankSpawnPoint in BankSecondRowSpawnPoints)
                StartCoroutine(transformList.MoveToParent(bankSpawnPoint, duration,
                    randomOrder: true, tileCount: 10));

            yield return new WaitForSeconds(duration);

            yield return transformList.MoveToParent(UiPanel, 1f,
                new Vector3(1.1f, 0), useScale: true, randomOrder: true, tileCount: 4);
        }

        private IEnumerator BuildBoard()
        {
            yield return DropTiles();

            yield return DealTiles();

            FindObjectOfType<PlayerHandComponent>().Initialize();
        }

        // Start is called before the first frame update
        public void Start()
        {
            var tilesToMultiply = TileSet.Where(x => x.Number != 5).ToList();
            var nonRedFives = TileSet.Where(x => x.Number == 5 && !x.Red).ToList();
            for (var i = 0; i < 3; i++)
                TileSet.AddRange(tilesToMultiply);

            for (var i = 0; i < 2; i++)
                TileSet.AddRange(nonRedFives);

            StartCoroutine(BuildBoard());
        }

        public void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 1000f, ~LayerMask.NameToLayer("MahjongTile")))
            {
                _activeTile?.HandleMouseOut();
                _activeTile = null;
                return;
            }

            var objectHit = hit.transform;
            var mahjongTileComponent = objectHit.GetComponent<MahjongTileComponent>();

            if (_activeTile == mahjongTileComponent)
                return;

            _activeTile?.HandleMouseOut();

            if (!mahjongTileComponent)
                return;

            mahjongTileComponent.HandleInput();
            _activeTile = mahjongTileComponent;
        }
    }
}