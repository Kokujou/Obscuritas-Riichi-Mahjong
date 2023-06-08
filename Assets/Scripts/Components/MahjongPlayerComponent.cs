using ObscuritasRiichiMahjong.Assets.Scripts.Components;
using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Global;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    [RequireComponent(typeof(MahjongPlayerHoverEffectsComponent))]
    public class MahjongPlayerComponent : MahjongPlayerComponentBase
    {
        public MahjongTileComponent HoveredHandTile;
        public GameObject ChiSelectionUiTemplate;

        private bool ShouldRequestChiSelection = false;
        private bool ShouldRequestRiichiSelection = false;

        private bool TurnCancelled = false;

        public override void ScanHand()
        {
            HandParent.localRotation = Quaternion.Euler(45, 0, 0);
            Player.Hand.AddRange(HandParent.GetComponentsInChildren<MahjongTileComponent>().Select(x => x.Tile));
        }

        public override IEnumerator MakeTurn(MahjongTileComponent lastDrawn)
        {
            TurnCancelled = false;
            yield return CheckForRiichi();
            yield return CheckForTsumo();

            while (!TurnCancelled && !RiichiMode && (!Input.GetMouseButtonDown(0) || !HoveredHandTile))
                yield return null;
            if (TurnCancelled) yield break;

            MahjongTileComponent.UnhoverAll();
            if (RiichiMode) HoveredHandTile = lastDrawn;
            LastDiscardedTile = HoveredHandTile;
            yield return DiscardTile(HoveredHandTile);
        }

        public IEnumerator CheckForTsumo()
        {
            if (!Player.CanTsumo) yield break;

            yield return SpawnActionButtons(CallType.Tsumo, CallType.Skip);
        }

        public IEnumerator CheckForRiichi()
        {
            if (!Player.CanRiichi || RiichiMode) yield break;

            MahjongMain.CanHover = false;

            yield return SpawnActionButtons(CallType.Riichi, CallType.Skip);

            if (ShouldRequestRiichiSelection)
                yield return RequestRiichiSelection();

            MahjongMain.CanHover = true;
        }

        public override IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile)
        {
            var possibleCalls = Player.GetAvailableCallTypes(lastDiscardedTile.Tile);
            if (possibleCalls.Count <= 1) yield break;
            if (RiichiMode && !possibleCalls.Any(x => x == CallType.Ron || x == CallType.HiddenKan)) yield break;

            MahjongMain.CanHover = false;
            yield return SpawnActionButtons(possibleCalls.ToArray());

            if (ShouldRequestChiSelection) yield return RequestChiSelection(lastDiscardedTile);

            MahjongMain.CanHover = true;
        }

        public IEnumerator SpawnActionButtons(params CallType[] callTypes)
        {
            var actionButtons = callTypes.Select(possibleCall => Instantiate(
                    PrefabCollection.Instance.ActionButtonDictionary[possibleCall],
                    SceneObjectCollection.Instance.ActionButtonPanel).GetComponent<ActionButtonComponent>()
                .Initialize(this)).ToList();

            while (true)
            {
                yield return null;
                var submittedActionButton = actionButtons.FirstOrDefault(x => x.Submitted);
                if (submittedActionButton is not null)
                {
                    this.LastCallType = submittedActionButton.MoveType;
                    break;
                }
            }
        }

        public IEnumerator Riichi()
        {
            TurnCancelled = true;
            var handTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var nonTenpaiTiles = new List<MahjongTileComponent>();

            if (nonTenpaiTiles.Count == 1) yield return base.Riichi(nonTenpaiTiles[0]);
            else ShouldRequestRiichiSelection = true;
        }

        public IEnumerator Chi()
        {
            var handTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var chis = handTiles.Append(MahjongMain.LastDiscard).GetChisWithTile(MahjongMain.LastDiscard)
                .ToList();

            if (chis.Count == 1)
                yield return base.Chi();
            else ShouldRequestChiSelection = true;
        }

        public IEnumerator RequestChiSelection(MahjongTileComponent lastDiscard)
        {
            var handTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>().Append(lastDiscard);
            var chis = handTiles.GetChisWithTile(lastDiscard).ToList();
            var chiSelectionUi = Instantiate(ChiSelectionUiTemplate).GetComponent<ChiSelectionComponent>();
            foreach (var chi in chis) chi.Remove(lastDiscard.Tile);
            chiSelectionUi.Initialize(chis.Select(chi => (chi[0], chi[1])));

            yield return new WaitUntil(() => chiSelectionUi.SelectedChi is not (null, null) || chiSelectionUi.Aborted);

            Destroy(chiSelectionUi.gameObject);

            if (!chiSelectionUi.Aborted && chiSelectionUi.SelectedChi is not (null, null))
            {
                yield return base.Chi((
                    handTiles.First(x => x.Tile == chiSelectionUi.SelectedChi.Item1),
                    handTiles.First(x => x.Tile == chiSelectionUi.SelectedChi.Item2)));
                yield break;
            }

            yield return ReactOnDiscard(lastDiscard);
        }

        public IEnumerator RequestRiichiSelection()
        {
            var handTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var nonTenpaiTiles = Player.GetNonTenpaiTiles();
            MahjongMain.CanHover = true;

            foreach (var tile in handTiles) tile.SetInactive();
            foreach (var tile in handTiles.Where(x => nonTenpaiTiles.Contains(x.Tile))) tile.SetActive();

            var hoverComponent = GetComponent<MahjongPlayerHoverEffectsComponent>();
            hoverComponent.CanHover = (tile) => nonTenpaiTiles.Any(x => x == tile?.Tile);

            while (!Input.GetMouseButtonDown(0) || !HoveredHandTile) yield return null;
            hoverComponent.CanHover = tile => tile?.transform?.parent == HandParent.transform;

            yield return base.Riichi(HoveredHandTile);
        }
    }
}