using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Components;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Components
{
    [RequireComponent(typeof(MahjongPlayerComponent))]
    public class MahjongPlayerHoverEffectsComponent : MonoBehaviour
    {
        public GameObject LeftToYakuUi;
        private MahjongPlayerComponent Player;
        public Func<MahjongTileComponent, bool> CanHover { get; set; }

        private UIDocument HoverUiInstance = null;

        public void Start()
        {
            Player = GetComponent<MahjongPlayerComponent>();
            CanHover = (tile) => tile?.transform?.parent == Player.HandParent;
        }

        public void Update()
        {
            if (MahjongMain.CanHover)
            {
                HandleTileHover(CanHover);
                /*if (Player.Player.IsTenpai(Player.Player.Hand))*/
                ShowMissingTilesUi();
            }
            else
            {
                Player.HoveredHandTile = null;
                MahjongTileComponent.UnhoverAll();
            }
        }

        private void HandleTileHover(Func<MahjongTileComponent, bool> canHover)
        {

            var viewportPoint = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height,
                Input.mousePosition.z);
            var ray = Camera.main.ViewportPointToRay(viewportPoint);

            var isHit = Physics.Raycast(ray, out var hit, 1000f, LayerMask.GetMask("MahjongTile"));
            var mahjongTileComponent = hit.transform?.GetComponent<MahjongTileComponent>();
            if (Player.HoveredHandTile == mahjongTileComponent) return;

            MahjongTileComponent.UnhoverAll();
            Player.HoveredHandTile = null;

            if (canHover(mahjongTileComponent) != Player.HandParent) return;

            var currentSameTiles = MahjongMain.GetSameTiles(mahjongTileComponent);
            foreach (var tile in currentSameTiles) tile.SetHovered();
            Player.HoveredHandTile = mahjongTileComponent;
        }

        private void ShowMissingTilesUi()
        {
            if (!Player.HoveredHandTile) return;

            SpawnUiAtHoverTile();

            var handWithoutHover = Player.Player.Hand.Except(Player.HoveredHandTile.Tile);
            var tilesToYaku = Player.Player.GetTilesFromTenpaiToYaku(handWithoutHover);

            if (tilesToYaku is null or { Count: 0 }) return;


            //


        }

        private void SpawnUiAtHoverTile()
        {
            if (HoverUiInstance) Destroy(HoverUiInstance.gameObject);

            var targetPosition = Camera.main.WorldToViewportPoint(Player.HoveredHandTile.transform.position);
            Debug.Log(targetPosition);
            var ui = Instantiate(LeftToYakuUi).GetComponent<UIDocument>();
            HoverUiInstance = ui;
            var root = ui.rootVisualElement.Q("left-to-yaku-ui");
            root.style.left = new Length(targetPosition.x * 100, LengthUnit.Percent);
            root.style.bottom = new Length(targetPosition.y * 100 + 10, LengthUnit.Percent);
        }
    }

}
