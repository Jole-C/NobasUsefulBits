using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Basic range of two curves.
/// </summary>

[Serializable]
public struct CurveRange
{
    public AnimationCurve curveA;
    public AnimationCurve curveB;

    public AnimationCurve RandomCurve => Random.value > 0.5 ? curveA : curveB;
}
