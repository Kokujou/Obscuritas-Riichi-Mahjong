using ObscuritasRiichiMahjong.Components.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Global
{
    public class SceneObjectCollection : MonoBehaviour
    {
        public static SceneObjectCollection Instance;

        public List<Transform> HandSpawnPoints;
        public List<MahjongPlayerComponentBase> MahjongPlayerComponents;

        public Transform KanDoraPanel;
        public Transform ActionButtonPanel;

        public SceneObjectCollection()
        {
            Instance = this;
        }
    }
}