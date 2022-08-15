using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
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
    public static IEnumerator FadeValue(float fadeTime, float startingValue, float targetValue, Action<float> ChangeValue, Action ondone = null)
    {
        for (float lerp = 0; lerp < 1; lerp += Time.unscaledDeltaTime / fadeTime)
        {
            float newValue = Mathf.Lerp(startingValue, targetValue, lerp);
            ChangeValue(newValue);
            yield return new WaitForEndOfFrame();
        }
        ondone?.Invoke();
    }
}
[Serializable]
public class anLerpCurve
{
    public AnimationCurve Curve;
    public float LowerLimit;
    public float UpperLimit;
    public float Evaluate(float t)
    {
        float lerpt = Curve.Evaluate(t);
        return Mathf.LerpUnclamped(LowerLimit, UpperLimit, lerpt);
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
    public int CrotchetBPM = 120;
    public int BeatsPerBar = 4;
    public anBarValue TimeSignature;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}