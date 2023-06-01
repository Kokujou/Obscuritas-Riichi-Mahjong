using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Global;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongPlayerComponent : MahjongPlayerComponentBase
    {
        public MahjongTileComponent HoveredHandTile;

        public void Update()
        {
            if (!MahjongMain.IsPaused) HandleTileHover();
            else
            {
                HoveredHandTile = null;
                MahjongTileComponent.UnhoverAll();
            }
        }

        private void HandleTileHover()
        {
            var viewportPoint = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height,
                Input.mousePosition.z);
            var ray = Camera.main.ViewportPointToRay(viewportPoint);

            var isHit = Physics.Raycast(ray, out var hit, 1000f, LayerMask.GetMask("MahjongTile"));
            var mahjongTileComponent = hit.transform?.GetComponent<MahjongTileComponent>();
            if (HoveredHandTile == mahjongTileComponent) return;

            MahjongTileComponent.UnhoverAll();
            HoveredHandTile = null;

            if (mahjongTileComponent?.transform?.parent != HandParent) return;

            var currentSameTiles = MahjongMain.GetSameTiles(mahjongTileComponent);
            foreach (var tile in currentSameTiles) tile.SetHovered();
            HoveredHandTile = mahjongTileComponent;
        }

        public override void ScanHand()
        {
            HandParent.localRotation = Quaternion.Euler(45, 0, 0);
            Player.Hand.AddRange(HandParent.GetComponentsInChildren<MahjongTileComponent>().Select(x => x.Tile));
        }

        public override IEnumerator MakeTurn()
        {
            while (!Input.GetMouseButtonDown(0) || !HoveredHandTile) yield return null;

            MahjongTileComponent.UnhoverAll();
            LastDiscardedTile = HoveredHandTile;
            yield return DiscardTile(HoveredHandTile);
        }

        public override IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile)
        {
            var possibleCalls = new List<CallType> { CallType.Skip };

            if (Player.CanPon(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.Pon);
            if (Player.CanChi(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.Chi);
            if (Player.CanKan(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.OpenKan);
            if (Player.CanRon(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.Ron);


            /**
             * 
             */
            possibleCalls.Add(CallType.Chi);

            if (possibleCalls.Count <= 1)
                yield break;

            MahjongMain.IsPaused = true;
            var actionButtons = possibleCalls.Select(possibleCall => Instantiate(
                    PrefabCollection.Instance.ActionButtonDictionary[possibleCall],
                    SceneObjectCollection.Instance.ActionButtonPanel).GetComponent<ActionButtonComponent>()
                .Initialize(this, lastDiscardedTile)).ToList();


            while (true)
            {
                if (actionButtons.Any(x => x.Submitted))
                {
                    foreach (var actionButton in actionButtons) Destroy(actionButton.gameObject);
                    MahjongMain.IsPaused = false;
                    yield break;
                }

                yield return null;
            }
        }

        public IEnumerator Chi(MahjongTileComponent lastDiscard)
        {
            IEnumerable<MahjongTileComponent> exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var chis = exposedTiles.GetChisWithTile(lastDiscard)
                .DistinctBy(chi => string.Join("|", chi.Select(component => component.Tile.ToString()))).ToList();

            if (chis.Count == 1)
            {
                yield return base.Chi(lastDiscard);
                yield break;
            }

            var tilesToHover = chis.SelectMany(x => x).DistinctBy(x => x.Tile.Name);
            yield return RequestChiTripletSelection();
        }

        public IEnumerator RequestChiTripletSelection()
        {
            foreach (var tile in GetComponentsInChildren<MahjongTileComponent>()) tile.SetInactive();

            while (true)
            {

            }
        }

    }
}