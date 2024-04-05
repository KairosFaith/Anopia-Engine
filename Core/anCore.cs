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
        return mag.Clips[Key];
    }
}
public abstract class IanAudioMag : ScriptableObject
{
    //TODO what here????
}
public abstract class IanMusicMag : IanAudioMag
{
    public anTempoData Tempo;
}
[Serializable]
public class TrackData
{
    public AudioMixerGroup Channel;
    public AudioClip Clip;
    [Range(-1, 1)]
    public float Pan;
}
[Serializable]
public class StingerData : TrackData
{
    public int BeatToStart;
}