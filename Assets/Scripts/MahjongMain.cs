using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class MahjongMain : MonoBehaviour
    {
        public List<Transform> HandSpawnPoints;
        public List<MahjongPlayerComponentBase> MahjongPlayerComponents;

        public GameObject MahjongTileTemplate;
        public List<MahjongTile> TileSet;
        public Transform UiPanel;

        public MahjongBoard Board = new MahjongBoard();

        public readonly Dictionary<CardinalPoint, MahjongPlayerComponentBase> PlayerComponents =
            new Dictionary<CardinalPoint, MahjongPlayerComponentBase>();

        public MahjongPlayerComponentBase CurrentPlayer =>
            PlayerComponents[Board.CurrentRoundWind];

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

            KanDora = RandomSubsetSpawns(KanDora.Capacity);
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

            Board.KanDora.AddRange(KanDora.Select(x => x.Tile));
            Board.UraDora.AddRange(TileSet.RandomSubset(5));
            Board.KanWall.AddRange(TileSet.RandomSubset(4));
            Board.Wall.AddRange(TileSet.RandomSubset(70));
        }

        private List<MahjongTileComponent> RandomSubsetSpawns(int count)
        {
            var subset = new List<MahjongTileComponent>(count);
            for (var i = 0; i < count; i++)
            {
                var index = Random.Range(0, TileSet.Count);
                var mahjongTile = TileSet[index];

                var mahjongTileComponent = MahjongTileComponent.FromTile(mahjongTile);
                mahjongTileComponent.transform.GetComponent<Rigidbody>().isKinematic = true;

                subset.Add(mahjongTileComponent);
            }

            return subset;
        }

        private IEnumerator BuildBoard()
        {
            InitializePlayers();

            yield return DealTiles(6f);
            yield return new WaitForSeconds(1);
            yield return KanDora[0].FlipDora();

            foreach (var player in MahjongPlayerComponents)
                player.ScanHand();

            StartCoroutine(PlayerInputLoop());
        }

        private IEnumerator PlayerInputLoop()
        {
            Board.CurrentRoundWind = CardinalPoint.East;

            while (Board.CurrentRound <= Board.MaxRounds)
            {
                Board.CurrentRound++;
                var currentPlayer = CurrentPlayer;

                yield return currentPlayer.MakeTurn();

                Board.CurrentRoundWind = Board.CurrentRoundWind.Next();
            }
        }

        private void InitializePlayers()
        {
            var availableWinds = new List<CardinalPoint>
                {CardinalPoint.South, CardinalPoint.East, CardinalPoint.West, CardinalPoint.North};

            var randomWindIndex = Random.Range(0, 4);
            var playerCardinal = availableWinds[randomWindIndex];
            foreach (var player in MahjongPlayerComponents)
            {
                player.Initialize(playerCardinal, Board);
                Board.Players.Add(player.Player.CardinalPoint, player.Player);
                PlayerComponents.Add(player.Player.CardinalPoint, player);
                playerCardinal = playerCardinal.Next();
            }
        }

        public void Start()
        {
            MahjongTileComponent.MahjongTileTemplate = MahjongTileTemplate;
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