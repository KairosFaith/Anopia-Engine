using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
public static partial class anCore
{
    public static T GetOrAddComponent <T>(this MonoBehaviour mb) where T : Component//TODO make this a utillity function
    {
        GameObject go = mb.gameObject;
        T comp = go.GetComponent<T>();
        if (comp == null)
            comp = go.AddComponent<T>();
        return comp;
    }
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
}
public abstract class IanAudioMag : ScriptableObject
{
    //TODO what here????
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