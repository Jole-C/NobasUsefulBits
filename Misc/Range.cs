using UnityEngine;

namespace Noba.Ranges
{
    [System.Serializable]
    public struct Range
    {
        public float min;
        public float max;

        public float RandomValue => Random.Range(min, max);
    }
}

