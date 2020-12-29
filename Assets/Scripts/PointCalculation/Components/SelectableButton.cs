using System;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    [RequireComponent(typeof(Button))]
    public class SelectableButton : MonoBehaviour
    {
        private bool _selected;

        public Color NormalColor;
        public Color SelectedColor;

        private Button _button;
        public Func<bool> IsSelected = () => true;

        public bool Selected => IsSelected();

        public void Start()
        {
            _button = GetComponent<Button>();
        }

        public void Update()
        {
            _button.targetGraphic.color = Selected ? SelectedColor : NormalColor;
        }
    }
}