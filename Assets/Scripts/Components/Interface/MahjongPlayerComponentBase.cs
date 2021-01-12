using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Components.Interface
{
    public abstract class MahjongPlayerComponentBase : MonoBehaviour
    {
        public MahjongPlayer Player { get; set; }

        public virtual void Initialize(ref List<CardinalPoint> availableWinds)
        {
            if (availableWinds == null || availableWinds.Count == 0)
                throw new ArgumentException("The list of available seat winds must not be empty");

            var hand = transform.GetComponentsInChildren<MahjongTileComponent>()
                .Select(x => x.Tile);

            var randomWindIndex = Random.Range(0, availableWinds.Count);
            var cardinalPoint = availableWinds[randomWindIndex];
            Player = new MahjongPlayer(hand, cardinalPoint);
            availableWinds.Remove(cardinalPoint);
        }

        public abstract Task<MahjongTile> MakeTurn();

        public abstract void Pon();
        public abstract void Chi();
        public abstract void OpenKan();
        public abstract void HiddenKan();
        public abstract void Ron();
        public abstract void Tsumo();
        public abstract void Riichi();
    }
}