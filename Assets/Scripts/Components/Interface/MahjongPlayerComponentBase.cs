using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Components.Interface
{
    public abstract class MahjongPlayerComponentBase : MonoBehaviour
    {
        public Transform HandParent;
        public Transform BankParent;
        public Transform DiscardedTilesParent;
        public Transform ExposedTilesParent;

        public TextMesh SeatWindPanel;

        public MahjongBoard Board { get; set; }
        public MahjongPlayer Player { get; set; }

        public virtual void Initialize(ref List<CardinalPoint> availableWinds, MahjongBoard board)
        {
            if (availableWinds == null || availableWinds.Count == 0)
                throw new ArgumentException("The list of available seat winds must not be empty");

            Board = board;

            var hand = transform.GetComponentsInChildren<MahjongTileComponent>()
                .Select(x => x.Tile);

            var randomWindIndex = Random.Range(0, availableWinds.Count);
            var cardinalPoint = availableWinds[randomWindIndex];
            Player = new MahjongPlayer(hand, cardinalPoint);
            availableWinds.Remove(cardinalPoint);

            SeatWindPanel.text = $"{cardinalPoint.ToString().First()}";
        }

        public virtual void DiscardTile(MahjongTileComponent tile)
        {
        }

        public void DrawTile()
        {
            var firstFromBank = BankParent.GetComponentsInChildren<MahjongTileComponent>().Last();

            Player.Wall.Remove(firstFromBank.Tile);
            Player.Hand.Add(firstFromBank.Tile);

            StartCoroutine(new List<Transform> {firstFromBank.transform}.MoveToParent(HandParent,
                1f,
                Vector3.right * 1.1f, useScale: true));
        }

        public abstract Task<MahjongTileComponent> MakeTurn();

        public abstract void Pon();
        public abstract void Chi();
        public abstract void OpenKan();
        public abstract void HiddenKan();
        public abstract void Ron();
        public abstract void Tsumo();
        public abstract void Riichi();
    }
}