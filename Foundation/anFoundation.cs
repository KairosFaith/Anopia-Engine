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
public enum anBarValue
{
    Quarter = 1,
    Eight,
    Sixteen = 4,
}
[Serializable]
public class anTempoData
{
    public int CrotchetBPM = 65;
    public int BeatsPerBar = 4;
    public anBarValue TimeSignature = anBarValue.Quarter;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}