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
        public List<Transform> BankSpawnPoints;
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

                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(5);
        }

        private IEnumerator DealTiles()
        {
            foreach (var handSpawnPoint in HandSpawnPoints)
                yield return MoveToParent(handSpawnPoint, 13);
            foreach (var bankSpawnPoint in BankSpawnPoints)
                yield return MoveToParent(bankSpawnPoint, 10);
            yield return MoveToParent(UiPanel, 4,
                new Vector3(1.1f, 0), useScale: true);
        }

        private IEnumerator MoveToParent(Transform parent, int tileNumber,
            Vector3 offset = default, float spacing = default, bool useScale = false)
        {
            var targetRotation = parent.rotation.eulerAngles;

            for (var handIndex = 0; handIndex < tileNumber; handIndex++)
            {
                var parentDirection = parent.rotation * Vector3.right;

                var index = Random.Range(0, TileSpawnPoint.childCount);
                var tile = TileSpawnPoint.GetChild(index);

                var spacingVector = parentDirection * handIndex * spacing;
                var tileWidth = handIndex * parentDirection;
                var targetScale = tile.localScale;

                if (useScale)
                {
                    spacingVector.Scale(parent.localScale);
                    tileWidth.Scale(parent.localScale);
                    targetScale.Scale(parent.localScale);
                }

                var targetPosition =
                    parent.position + offset + tileWidth + spacingVector;

                yield return tile.gameObject.PickUpAndMove(.01f, targetPosition, targetRotation,
                    targetScale);
                tile.SetParent(parent, true);
            }
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