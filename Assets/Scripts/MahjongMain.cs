using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Services;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class MahjongMain : MonoBehaviour
    {
        public List<Transform> HandSpawnPoints;
        public List<MahjongPlayerComponentBase> MahjongPlayerComponents;
        public List<MahjongTile> TileSet;
        public GameObject MahjongTileTemplate;
        public Transform UiPanel;

        private readonly MahjongBoard _board = new MahjongBoard();
        private GameInputLoopService _inputLoopService;

        public List<MahjongTileComponent> KanDora { get; set; }
            = new List<MahjongTileComponent>(5);

        private IEnumerator DealTiles(float duration)
        {
            var firstDuration = duration / 2f;
            foreach (var handSpawnPoint in HandSpawnPoints)
            {
                var handTiles = RandomSubsetSpawns(13).OrderBy(x => x.Tile.GetTileOrder());
                StartCoroutine(handTiles.SpawnAtParent(handSpawnPoint, firstDuration));
            }

            yield return new WaitForSeconds(firstDuration);

            KanDora = RandomSubsetSpawns(KanDora.Capacity).ToList();
            for (var i = 0; i < KanDora.Count; i++)
            {
                var tile = KanDora[i];
                tile.transform.SetParent(UiPanel, true);
                tile.transform.localScale = Vector3.Scale(tile.transform.localScale, UiPanel.localScale);
                tile.transform.Rotate(UiPanel.localRotation.eulerAngles);
                tile.transform.localPosition = (i * tile.transform.localScale.x / 2f
                                                - tile.transform.localScale.x)
                                               * Vector3.right;
            }

            _board.KanDora.AddRange(KanDora.Select(x => x.Tile));
            _board.UraDora.AddRange(TileSet.RandomSubset(5));
            _board.KanWall.AddRange(TileSet.RandomSubset(4));
            _board.Wall.AddRange(TileSet.RandomSubset(70));
        }

        private IEnumerable<MahjongTileComponent> RandomSubsetSpawns(int count)
        {
            return TileSet.TransformRandomSubset(count, mahjongTile =>
            {
                var mahjongTileComponent = MahjongTileComponent.FromTile(mahjongTile);
                mahjongTileComponent.transform.GetComponent<Rigidbody>().isKinematic = true;
                return mahjongTileComponent;
            });
        }

        private IEnumerator BuildBoard()
        {
            yield return DealTiles(6f);
            yield return new WaitForSeconds(1);
            yield return KanDora[0].FlipDora();

            foreach (var player in MahjongPlayerComponents)
                player.ScanHand();

            StartCoroutine(_inputLoopService.PlayerInputLoop());
        }

        public void Start()
        {
            MahjongTileComponent.MahjongTileTemplate = MahjongTileTemplate;
            _inputLoopService = new GameInputLoopService(_board, MahjongPlayerComponents);

            BuildTileSetFromTiles();
            StartCoroutine(BuildBoard());
        }

        public void BuildTileSetFromTiles()
        {
            var tilesToMultiply = TileSet.Where(x => x.Number != 5).ToList();
            var nonRedFives = TileSet.Where(x => x.Number == 5 && !x.Red).ToList();
            for (var i = 0; i < 3; i++)
                TileSet.AddRange(tilesToMultiply);

            for (var i = 0; i < 2; i++)
                TileSet.AddRange(nonRedFives);
        }
    }
}