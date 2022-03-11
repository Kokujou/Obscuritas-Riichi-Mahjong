using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Global;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Services;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class MahjongMain : MonoBehaviour
    {
        private readonly MahjongBoard _board = new();
        private GameInputLoopService _inputLoopService;

        private readonly List<MahjongTileComponent> _kanDora = new(5);

        public static IEnumerable<MahjongTileComponent> GetSameTiles(MahjongTileComponent tile)
        {
            var referenceSet = new List<MahjongTileComponent> { tile };

            var players = FindObjectsOfType<MahjongPlayerComponentBase>();
            foreach (var player in players)
            {
                referenceSet.AddRange(player.ExposedTilesParent.GetComponentsInChildren<MahjongTileComponent>());
                referenceSet.AddRange(player.DiscardedTilesParent.GetComponentsInChildren<MahjongTileComponent>());
            }

            return referenceSet.Where(x => x.Tile == tile.Tile);
        }

        private IEnumerator DealTiles(float duration)
        {
            var firstDuration = duration / 2f;
            var leftoverTiles = PrefabCollection.Instance.TileSet;
            foreach (var handSpawnPoint in SceneObjectCollection.Instance.HandSpawnPoints)
            {
                var handTiles = leftoverTiles.RandomSubsetSpawns(13, out leftoverTiles)
                    .OrderBy(x => x.Tile.GetTileOrder());
                StartCoroutine(handTiles.SpawnAtParent(handSpawnPoint, firstDuration));
            }

            yield return new WaitForSeconds(firstDuration);

            _kanDora.AddRange(leftoverTiles.RandomSubsetSpawns(_kanDora.Capacity, out leftoverTiles).ToList());
            var kanDoraPanel = SceneObjectCollection.Instance.KanDoraPanel;
            for (var i = 0; i < _kanDora.Count; i++)
            {
                var tile = _kanDora[i];
                tile.transform.SetParent(kanDoraPanel, true);
                tile.transform.localScale = Vector3.Scale(tile.transform.localScale, kanDoraPanel.localScale);
                tile.transform.Rotate(kanDoraPanel.localRotation.eulerAngles);
                tile.transform.localPosition = (i * tile.transform.localScale.x / 2f
                                                - tile.transform.localScale.x)
                                               * Vector3.right;
            }

            _board.KanDora.AddRange(_kanDora.Select(x => x.Tile));
            _board.UraDora.AddRange(leftoverTiles.RandomSubset(5, out leftoverTiles));
            _board.KanWall.AddRange(leftoverTiles.RandomSubset(4, out leftoverTiles));
            _board.Wall.AddRange(leftoverTiles.RandomSubset(70, out leftoverTiles));
        }

        private IEnumerator BuildBoard()
        {
            yield return DealTiles(6f);
            yield return new WaitForSeconds(1);
            yield return _kanDora[0].FlipDora();

            foreach (var player in SceneObjectCollection.Instance.MahjongPlayerComponents)
                player.ScanHand();

            StartCoroutine(_inputLoopService.PlayerInputLoop());
        }

        public void Start()
        {
            MahjongTileComponent.MahjongTileTemplate = PrefabCollection.Instance.MahjongTileTemplate;
            _inputLoopService =
                new GameInputLoopService(_board, SceneObjectCollection.Instance.MahjongPlayerComponents);

            StartCoroutine(BuildBoard());
        }
    }
}