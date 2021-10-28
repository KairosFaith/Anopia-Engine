using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
public static class AnopiaAudioCore
{
    static Dictionary<string, IanAudioMag> _SoundBank = new Dictionary<string, IanAudioMag>();
    static AnopiaAudioCore()
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
    public static ClipData FetchData(string SoundId, int Key)
    {
        anClipMag mag = (anClipMag)FetchMag(SoundId);
        return mag.Data[Key];
    }
    public static IanEvent NewEvent(MonoBehaviour host, string SoundID, AudioMixerGroup output)
    {
        IanAudioMag mag = FetchMag(SoundID);
        return mag.LoadMag(host, output);
    }
    static AnopiaSourcerer SetupSource(Vector3 position, GameObject prefab, AudioMixerGroup output, AudioClip clip, float volume)
    {
        GameObject newg = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        AnopiaSourcerer s = newg.GetComponent<AnopiaSourcerer>();
        AudioSource a = s.audioSource;
        a.outputAudioMixerGroup = output;
        a.clip = clip;
        a.volume = volume;
        return s;
    }
    public static AnopiaSourcerer[] SetLayers(MonoBehaviour host, string SoundID, AudioMixerGroup output)
    {
        anLayerMag mag = (anLayerMag)FetchMag(SoundID);
        Transform t = host.transform;
        List<AnopiaSourcerer> layers = new List<AnopiaSourcerer>();
        foreach(ClipLayer L in mag.Layers)
        {
            GameObject newg = UnityEngine.Object.Instantiate(L.SourcePrefab, t);
            AnopiaSourcerer s = newg.GetComponent<AnopiaSourcerer>();
            AudioSource a = s.audioSource;
            a.outputAudioMixerGroup = output;
            a.clip = L.Data.Clip;
            a.volume = L.Data.Gain;
            layers.Add(s);
            a.loop = true;
            a.Play();
        }
        return layers.ToArray();
    }
    public static AnopiaSourcerer PlayClipAtPoint(Vector3 position, AudioClip clip, float volume, AudioMixerGroup output, GameObject prefab, Action<AnopiaSourcerer> setup = null, Action OnDone = null)
    {
        AnopiaSourcerer s = SetupSource(position, prefab, output, clip, volume);
        AudioSource a = s.audioSource;
        setup?.Invoke(s);
        a.Play();
        s.StartCoroutine(DeleteWhenDone(a, OnDone));
        return s;
    }
    public static AnopiaSourcerer PlayClipAtSchedule(Transform parent, AudioClip clip, float volume, double startTime, AudioMixerGroup output, GameObject prefab, Action<AnopiaSourcerer> setup = null)
    {
        GameObject newg = UnityEngine.Object.Instantiate(prefab, parent);
        AnopiaSourcerer s = newg.GetComponent<AnopiaSourcerer>();
        AudioSource a = s.audioSource;
        a.outputAudioMixerGroup = output;
        a.clip = clip;
        a.volume = volume;
        setup?.Invoke(s);
        a.PlayScheduled(startTime);
        s.StartCoroutine(DeleteWhenDone(a, startTime+clip.length));
        return s;
    }
    public static void TransitionToSnapshot(this AudioMixer mixer, string SnapshotName, float TimeToReach)
    {
        AudioMixerSnapshot snapshot = mixer.FindSnapshot(SnapshotName);
        AudioMixerSnapshot[] ss = new AudioMixerSnapshot[] { snapshot };
        float[] w = new float[] { 1 };
        mixer.TransitionToSnapshots(ss, w, TimeToReach);
        //the weight is nonsense, dun mess with it
    }
    public static IEnumerator DeleteWhenDone(AudioSource source, Action onDone = null)
    {
        yield return new WaitForSeconds(source.clip.length);
        while (source.isPlaying&&!AudioListener.pause)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        UnityEngine.Object.Destroy(source.gameObject);
    }
    public static IEnumerator DeleteWhenDone(AudioSource source, double stopTime, Action onDone = null)
    {
        while ((AudioSettings.dspTime< stopTime) || source.isPlaying)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        UnityEngine.Object.Destroy(source.gameObject);
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
public abstract class IanAudioMag : ScriptableObject
{
    public abstract IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output);
}
[Serializable]
public class ClipData
{
    public AudioClip Clip;
    [Range(0, 1)]
    public float Gain = 1;
}
public abstract class IanEvent
{
    public IanEvent(MonoBehaviour host, IanAudioMag mag, AudioMixerGroup output){ }
    public abstract void Play(params object[] args);
    public abstract void PlayScheduled(double timecode, params object[] args);
    public abstract void Stop();
}
[Serializable]
public class LerpCurve
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