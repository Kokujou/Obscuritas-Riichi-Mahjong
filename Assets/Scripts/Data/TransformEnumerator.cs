using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Data
{
    public class TransformEnumerator : IEnumerator<Transform>
    {
        private readonly Transform _parent;
        private int _index = -1;

        public TransformEnumerator(Transform parent)
        {
            _parent = parent;
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _parent.childCount;
        }

        public void Reset()
        {
            _index = -1;
        }

        public Transform Current => _parent.GetChild(_index);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}