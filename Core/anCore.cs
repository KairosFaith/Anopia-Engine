using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
public static partial class anCore
{
    static Dictionary<string, IanAudioMag> _SoundBank = new Dictionary<string, IanAudioMag>();
    static anCore()
    {
        string FilePath = "AudioMags";
        IanAudioMag[] m = Resources.LoadAll<IanAudioMag>(FilePath);
        foreach (IanAudioMag b in m)
            _SoundBank.Add(b.name, b);
    }
    public static IanAudioMag FetchMag(string SoundID)
    {
        if (_SoundBank.TryGetValue(SoundID, out IanAudioMag mag))
            return mag;
        else
            throw new Exception(SoundID + " not found");
    }
    public static AudioClip FetchData(string SoundId, int Key)
    {
        anClipMag mag = (anClipMag)FetchMag(SoundId);
        return mag.Clips[Key];
    }
}
public abstract class IanAudioMag : ScriptableObject
{
    //TODO put what here???
}
public abstract class IanMusicMag : IanAudioMag
{
    public anSourcerer SourcePrefab2D;
    public anTempoData Tempo;
}
[Serializable]
public class AudioLayerData
{
    public AudioClip Clip;
    public anSourcerer SourcePrefab;
}
[Serializable]
public class AudioAutomationData
{
    //[HideInInspector]//TODO put enum in inspector only, ideally store string instead
    public SourceEffect Effect;
    public anLerpCurve Smoothing;
    protected System.Reflection.PropertyInfo pInfo;
    public float Evaluate(float unnormalisedValue)
    {
        return Smoothing.Evaluate(unnormalisedValue);
    }
    public void SetSourcererValue(anSourcerer handler, float value)
    {
        if (pInfo == null)
            pInfo = typeof(anSourcerer).GetProperty(Effect.ToString());
        pInfo.SetValue(handler, value);
    }
    public enum SourceEffect//TODO put this in editor only somehow
    {
        Volume,
        Pitch,
        Distortion,
        HighPass,
    }
}
[Serializable]
public class LayerAutomationData : AudioAutomationData
{
    public string ParameterName;
    public int TargetSourceIndex;
    public void AutomationAction(float value, anSourcerer[] handlers)
    {
        float e = Evaluate(value);
        SetSourcererValue(handlers[TargetSourceIndex], e);
    }
}
[Serializable]
public class StingerData : anTrackData
{
    public int BeatToStart;
    /// <summary>
    /// plays a stinger at the given time, and deletes the source after the clip has finished playing
    /// </summary>
    public anSourcerer PlayStinger(double startTime, anSourcerer sourcePrefab, Transform parent)
    {
        anSourcerer s = GameObject.Instantiate(sourcePrefab, parent);
        s.SetUp(this, false);
        s.PlayScheduled(startTime);
        s.DeleteAfterTime(startTime);
        return s;
    }
}
[Serializable]
public class VoiceLineData
{
    public string Line;
    public AudioClip Clip;
}