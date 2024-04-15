using System;
using UnityEngine;
using UnityEngine.Audio;
[Serializable]
public class anLerpCurve
{
    public AnimationCurve Curve;
    public float MinInputValue, MaxInputValue, OutputValueA, OutputValueB;
    public float Evaluate(float input)
    {
        float inputT = Mathf.InverseLerp(MinInputValue, MaxInputValue, input);
        float outputT = Curve.Evaluate(inputT);
        return Mathf.LerpUnclamped(OutputValueA, OutputValueB, outputT);
    }
}
[Serializable]
public class anTrackData
{
    public AudioMixerGroup Channel;
    public AudioClip Clip;
    [Range(-1, 1)]
    public float Pan;
}