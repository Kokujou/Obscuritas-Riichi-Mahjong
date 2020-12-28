using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class ButtonGroup : MonoBehaviour
    {
        private Button _selectedButton;

        public List<Button> Buttons;
        public Button DefaultButton;

        public bool AllowNoSelection = true;

        public Color NormalColor;
        public Color SelectedColor;

        public Action<Button> OnSelectionChange = x => { };

        public void Start()
        {
            if (DefaultButton)
                _selectedButton = DefaultButton;
            else if (!AllowNoSelection)
                throw new NotImplementedException(
                    "If 'Allow No Selection' is false a default button must be specified");

            foreach (var button in Buttons)
            {
                button.onClick.AddListener(() => ToggleSelection(button));
                button.targetGraphic.color = NormalColor;
            }

            if (!_selectedButton) return;

            _selectedButton.targetGraphic.color = SelectedColor;
        }

        public void ToggleSelection(Button button)
        {
            if (_selectedButton)
            {
                var toggleSelectedButton =
                    _selectedButton.GetInstanceID() == button.GetInstanceID();
                UnselectButton(_selectedButton);

                if (toggleSelectedButton)
                {
                    OnSelectionChange?.Invoke(null);
                    return;
                }
            }

            SelectButton(button);

            OnSelectionChange?.Invoke(button);
        }

        public void SelectButton(Button button)
        {
            _selectedButton = button;
            _selectedButton.targetGraphic.color = SelectedColor;
        }

        public void UnselectButton(Button button)
        {
            button.targetGraphic.color = NormalColor;
            _selectedButton = null;
        }
    }
}