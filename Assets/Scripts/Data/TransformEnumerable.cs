using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Data
{
    public class TransformEnumerable : IEnumerable<Transform>
    {
        private readonly Transform _parent;

        public TransformEnumerable(Transform parent)
        {
            _parent = parent;
        }

        public IEnumerator<Transform> GetEnumerator()
        {
            return new TransformEnumerator(_parent);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}