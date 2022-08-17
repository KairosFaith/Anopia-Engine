using System;
using UnityEngine;
[Serializable]
public class anClipData
{
    public AudioClip Clip;
    [Range(0, 1)]
    public float Gain = 1;
}
[Serializable]
public class anLerpCurve
{
    public AnimationCurve Curve;
    public float LowerLimit;
    public float UpperLimit;
    public float Evaluate(float input)
    {
        float t = Curve.Evaluate(input);
        return Mathf.LerpUnclamped(LowerLimit, UpperLimit, t);
    }
}