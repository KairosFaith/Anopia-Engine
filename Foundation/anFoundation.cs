using System;
using UnityEngine;
using UnityEngine.Audio;
public static partial class anCore
{
    public static void TransitionToSnapshot(this AudioMixer mixer, string SnapshotName, float TimeToReach)
    {
        AudioMixerSnapshot snapshot = mixer.FindSnapshot(SnapshotName);
        AudioMixerSnapshot[] ss = new AudioMixerSnapshot[] { snapshot };
        float[] w = new float[] { 1 };
        mixer.TransitionToSnapshots(ss, w, TimeToReach);
        //the weight is nonsense, dun mess with it
    }
}
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
    public int CrotchetBPM;
    public int BeatsPerBar;
    public anBarValue TimeSignature;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}