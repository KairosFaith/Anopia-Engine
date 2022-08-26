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
        return mag.Data[Key];
    }
    public static void SetEffect(this anSourcerer sourcerer, SourceEffect effect, float effectValue)
    {
        switch (effect)
        {
            case SourceEffect.Volume:
                sourcerer.audioSource.volume = effectValue;
                break;            
            case SourceEffect.Pitch:
                sourcerer.audioSource.pitch = effectValue;
                break;            
            case SourceEffect.Distortion:
                sourcerer.audioDistortionFilter.distortionLevel = effectValue;
                break;
            case SourceEffect.HighPass:
                sourcerer.audioHighPassFilter.cutoffFrequency = effectValue;
                break;
        }
    }
}
public abstract class IanAudioMag : ScriptableObject
{
    //public abstract IanEvent LoadMag(anDriver driver, AudioMixerGroup output);
}
public abstract class IanEvent
{
    public abstract bool UsingDriverSource { get; }
    public IanEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output){ }
    public abstract void Play(params object[] args);
    public abstract void PlayScheduled(double timecode, params object[] args);//TODO not applicable in FMOD Wwise
    public abstract void Stop();
    public abstract void SetParameter(string name, float value);//, bool ignoreSeekSpeed = false
}
[Serializable]
public class AudioAutomationData
{
    public string ParameterName;
    public anLerpCurve Smoothing;
    public SourceEffect Effect;
    public float MinInputValue, MaxInputValue;
    public float EvaluateRandom()
    {
        float r = UnityEngine.Random.Range(0f, 1f);
        return Smoothing.Evaluate(r);
    }
    public float EvaluateUnnormalised(float unnormalisedValue)
    {
        return Mathf.InverseLerp(MinInputValue, MaxInputValue, unnormalisedValue);
    }
}
[Serializable]
public class LayerAutomationData : AudioAutomationData
{
    public int TargetSource;
}
public enum SourceEffect
{
    Volume,
    Pitch,
    Distortion,
    HighPass,
}
public abstract class IanMusicMag : IanAudioMag
{
    public anTempoData Tempo;
    public abstract SongForm Structure { get; }
}
public enum SongForm
{
    Linear,
    Stem
}
public abstract class IanSong : MonoBehaviour
{
    public abstract void Setup(IanMusicMag mag, AudioMixerGroup output);
    public abstract void StopOnCue(double stopTime);
    public abstract void StopImmediate();
    public abstract void Play(double startTime);
    public abstract void FadeIn(float t);
    public abstract void FadeOut(float t, Action ondone = null);
    public abstract void Mute(bool toMute);
}