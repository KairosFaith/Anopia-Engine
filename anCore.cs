using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
public static class anCore
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
    public static ClipData FetchData(string SoundId, int Key)
    {
        anClipMag mag = (anClipMag)FetchMag(SoundId);
        return mag.Data[Key];
    }
    public static IanEvent NewEvent(MonoBehaviour driver, string SoundID, AudioMixerGroup output)
    {
        IanAudioMag mag = FetchMag(SoundID);
        return mag.LoadMag(driver, output);
    }
    static anSourcerer SetupSource(Vector3 position, anSourcerer prefab, AudioMixerGroup output, AudioClip clip, float volume)
    {
        anSourcerer s = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        AudioSource a = s.audioSource;
        a.outputAudioMixerGroup = output;
        a.clip = clip;
        a.volume = volume;
        return s;
    }
    public static anSourcerer[] SetLayers(MonoBehaviour driver, string SoundID, AudioMixerGroup output)
    {
        anLayerMag mag = (anLayerMag)FetchMag(SoundID);
        Transform t = driver.transform;
        List<anSourcerer> layers = new List<anSourcerer>();
        foreach(LayerData L in mag.Layers)
        {
            anSourcerer s = UnityEngine.Object.Instantiate(L.SourcePrefab, t);
            AudioSource a = s.audioSource;
            a.outputAudioMixerGroup = output;
            a.clip = L.Clip;
            a.volume = L.Gain;
            layers.Add(s);
            a.loop = true;
            a.Play();
        }
        return layers.ToArray();
    }
    public static anSourcerer PlayClipAtPoint(Vector3 position, AudioClip clip, float volume, AudioMixerGroup output, anSourcerer prefab, Action<anSourcerer> setup = null, Action OnDone = null)
    {
        anSourcerer s = SetupSource(position, prefab, output, clip, volume);
        AudioSource a = s.audioSource;
        setup?.Invoke(s);
        a.Play();
        s.StartCoroutine(DeleteWhenDone(a, OnDone));
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
    public abstract IanEvent LoadMag(MonoBehaviour driver, AudioMixerGroup output);
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
    public abstract void Play(params object[] args);//TODO remove args
    public abstract void PlayScheduled(double timecode, params object[] args);//TODO remove as not applicable in FMOD Wwise
    public abstract void Stop();
    public abstract void SetParameter(string name, float value, params object[] args);//, bool ignoreSeekSpeed = false
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