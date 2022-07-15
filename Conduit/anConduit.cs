using System;
using UnityEngine;
using UnityEngine.Audio;
public static partial class anCore
{
    public static anSourcerer SetupSource(Vector3 position, anSourcerer prefab, AudioMixerGroup output, AudioClip clip, float volume)
    {
        anSourcerer s = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        AudioSource a = s.audioSource;
        a.outputAudioMixerGroup = output;
        a.clip = clip;
        a.volume = volume;
        return s;
    }
    public static anSourcerer PlayClipAtPoint(Vector3 position, AudioClip clip, float volume, AudioMixerGroup output, anSourcerer prefab, Action<anSourcerer> setup = null, Action OnDone = null)
    {
        anSourcerer s = SetupSource(position, prefab, output, clip, volume);
        AudioSource a = s.audioSource;
        setup?.Invoke(s);
        a.Play();
        s.DeleteWhenDone(OnDone);
        return s;
    }
    public static anSourcerer PlayClipScheduled(Transform parent, AudioClip clip, float volume, double startTime, AudioMixerGroup output, anSourcerer prefab, Action<anSourcerer> setup = null)
    {
        anSourcerer s = UnityEngine.Object.Instantiate(prefab, parent);
        AudioSource a = s.audioSource;
        a.outputAudioMixerGroup = output;
        a.clip = clip;
        a.volume = volume;
        setup?.Invoke(s);
        a.PlayScheduled(startTime);
        s.DeleteAfterTime(a, startTime + clip.length);
        return s;
    }
}
public abstract class IanAudioMag : ScriptableObject
{
    public abstract IanEvent LoadMag(anDriver driver, AudioMixerGroup output);
}
public abstract class IanEvent
{
    public abstract bool UsingDriverSource { get; }
    public IanEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) { }
    public abstract void Play(params object[] args);
    public abstract void PlayScheduled(double timecode, params object[] args);//TODO not applicable in FMOD Wwise
    public abstract void Stop();
    public abstract void SetParameter(string name, float value);//, bool ignoreSeekSpeed = false
}