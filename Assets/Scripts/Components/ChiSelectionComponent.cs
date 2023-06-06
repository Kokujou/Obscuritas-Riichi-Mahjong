using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObscuritasRiichiMahjong
{
    [RequireComponent(typeof(UIDocument))]
    public class ChiSelectionComponent : MonoBehaviour
    {
        public ValueTuple<MahjongTile, MahjongTile> SelectedChi = (null, null);
        public bool Aborted = false;

        public void Initialize(IEnumerable<ValueTuple<MahjongTile, MahjongTile>> chisWithoutDiscard)
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var tileArea = root.Q<VisualElement>("tile-area");
            foreach (var chi in chisWithoutDiscard) tileArea.Add(CreateChiElement(chi));
            var backButton = root.Q<VisualElement>("back-button");
            backButton.RegisterCallback<ClickEvent>((e) => { Aborted = true; });
        }

        private VisualElement CreateChiElement(ValueTuple<MahjongTile, MahjongTile> chi)
        {
            var chiElement = new VisualElement();
            chiElement.AddToClassList("chi");
            chiElement.RegisterCallback<ClickEvent>((_) => { SelectedChi = chi; });
            foreach (var tile in chi.ToList<MahjongTile>()) chiElement.Add(CreateMahjongTileElement(tile));
            return chiElement;
        }

        private VisualElement CreateMahjongTileElement(MahjongTile tile)
        {
            var tileElement = new VisualElement();
            tileElement.AddToClassList("tile");
            tileElement.style.backgroundImage = new StyleBackground((Texture2D)tile.Material.mainTexture);
            return tileElement;
        }


    }
}
