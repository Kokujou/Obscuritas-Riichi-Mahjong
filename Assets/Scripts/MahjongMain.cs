using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Assets.Scripts.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong
{
    public class MahjongMain : MonoBehaviour
    {
        private static MahjongTileComponent _activeTile;
        public List<Transform> HandSpawnPoints;

        public GameObject MahjongTileTemplate;
        public List<MahjongTile> TileSet;
        public Transform UiPanel;

        public MahjongBoard Board;

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
                tile.transform.localPosition =
                    ((i * tile.transform.localScale.x / 2f)
                    - tile.transform.localScale.x)
                    * Vector3.right;
            }

        }

        private List<MahjongTileComponent> RandomSubsetSpawns(int count)
        {
            var subset = new List<MahjongTileComponent>(count);
            for (var i = 0; i < count; i++)
            {
                var index = Random.Range(0, TileSet.Count);
                var mahjongTile = TileSet[index];

                var mahjongTileComponent = MahjongTileComponent.AddToObject(
                    Instantiate(MahjongTileTemplate), mahjongTile);
                mahjongTileComponent.transform.GetComponent<Rigidbody>().isKinematic = true;

                subset.Add(mahjongTileComponent);
            }

            return subset;
        }

        private IEnumerator BuildBoard()
        {
            yield return DealTiles(6f);
            yield return new WaitForSeconds(1);
            yield return KanDora[0].InterpolationAnimation(.5f, targetRotation: Vector3.zero);

            var mahjongPlayers = FindObjectsOfType<MahjongPlayerComponentBase>();
            var availableWinds = new List<CardinalPoint>
                {CardinalPoint.South, CardinalPoint.East, CardinalPoint.West, CardinalPoint.North};

            foreach (var player in mahjongPlayers)
            {
                player.Initialize(ref availableWinds, Board);
                Board.Players.Add(player.Player.CardinalPoint, player.Player);
                PlayerComponents.Add(player.Player.CardinalPoint, player);
            }

            Task.Run(async () =>
            {
                Board.CurrentRoundWind = CardinalPoint.East;

                while (Board.CurrentRound <= Board.MaxRounds)
                {
                    Board.CurrentRound++;
                    var currentPlayer = CurrentPlayer;
                    await currentPlayer.MakeTurn();

                    Board.CurrentRoundWind++;
                }
            });
        }

        // Start is called before the first frame update
        public void Start()
        {
            Board = new MahjongBoard();

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
                if (!_activeTile)
                    return;

                _activeTile.HandleMouseOut();
                _activeTile = null;
                return;
            }

            var objectHit = hit.transform;
            var mahjongTileComponent = objectHit.GetComponent<MahjongTileComponent>();

            if (_activeTile == mahjongTileComponent) return;

            _activeTile?.HandleMouseOut();

            if (!mahjongTileComponent)
                return;

            mahjongTileComponent.HandleInput();
            _activeTile = mahjongTileComponent;
        }
    }
}