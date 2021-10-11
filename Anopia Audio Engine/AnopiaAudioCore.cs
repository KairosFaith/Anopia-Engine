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
        IanAudioMag[] m = Resources.LoadAll<IanAudioMag>("AudioMags");
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
    public static AnopiaSourcerer PlayClipAtPoint(Vector3 position, AudioClip clip, float volume, AudioMixerGroup output, GameObject prefab, Action<AnopiaSourcerer> setup = null)
    {
        GameObject newg = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        AnopiaSourcerer sourceObj = newg.AddComponent<AnopiaSourcerer>();
        sourceObj.SetOutput(output);
        sourceObj.SetData(clip,volume);
        setup?.Invoke(sourceObj);
        AudioSource a = sourceObj.Source;
        a.Play();
        sourceObj.StartCoroutine(DeleteWhenDone(a));
        return sourceObj;
    }
    //just in case - for specified clip
    public static AnopiaSourcerer PlayClipAtPoint(Vector3 position, string SoundId, int Key, AudioMixerGroup output)
    {
        GameObject newg = new GameObject(SoundId + " " + Key + " AudioSource", typeof(AudioSource));
        AnopiaSourcerer sourceObj = newg.AddComponent<AnopiaSourcerer>();
        sourceObj.SetOutput(output);
        ClipData d = FetchData(SoundId, Key);
        sourceObj.SetData(d);
        AudioSource a = sourceObj.Source;
        a.Play();
        sourceObj.StartCoroutine(DeleteWhenDone(a));
        return sourceObj;
    }
    public static AnopiaSourcerer PlayClipAtStereo(MonoBehaviour host, ClipData data, AudioMixerGroup output, float panStereo = 0,Action OnDone = null)
    {
        AnopiaSourcerer sourceObj = NewStereoSource(output);
        AudioSource a = sourceObj.Source;
        a.panStereo = panStereo;
        a.clip = data.Clip;
        a.volume = data.Gain;
        a.Play();
        sourceObj.StartCoroutine(DeleteWhenDone(a, OnDone));
        return sourceObj;
    }
    public static AnopiaSourcerer PlayClipScheduled(MonoBehaviour host, AudioClip clip, double startTime, AudioMixerGroup output)
    {
        AnopiaSourcerer sourceObj = NewStereoSource(output);
        sourceObj.PlayScheduled(clip, startTime);
        sourceObj.StartCoroutine(DeleteWhenDone(sourceObj.Source, startTime+clip.length));
        return sourceObj;
    }
    public static AnopiaSourcerer NewPointSource(MonoBehaviour host, GameObject prefab, AudioMixerGroup output)
    {
        GameObject newg = UnityEngine.Object.Instantiate(prefab, host.transform);
        AnopiaSourcerer s = newg.AddComponent<AnopiaSourcerer>();
        s.SetOutput(output);
        return s;
    }
    public static AnopiaSourcerer NewStereoSource(MonoBehaviour host, AudioMixerGroup output)
    {
        AnopiaSourcerer sor = NewStereoSource(output);
        sor.transform.SetParent(host.transform);
        return sor;
    }
    public static AnopiaSourcerer NewStereoSource(AudioMixerGroup output)
    {
        GameObject newg = new GameObject(output+ " AudioSource");
        AudioSource sor = newg.AddComponent<AudioSource>();
        sor.outputAudioMixerGroup = output;
        sor.spatialBlend = 0;
        sor.bypassListenerEffects = true;
        sor.bypassReverbZones = true;
        sor.bypassEffects = true;
        sor.ignoreListenerPause = true;//TODO IgnoreListenerPause is Game Dependent
        AnopiaSourcerer sourceObj = sor.gameObject.AddComponent<AnopiaSourcerer>();
        sourceObj.Source = sor;
        return sourceObj;
    }
    public static AnopiaSourcerer[] SetLayers(MonoBehaviour host, string SoundID, AudioMixerGroup output)
    {
        anClipMag mag = (anClipMag)FetchMag(SoundID);
        Transform t = host.transform;
        GameObject prefab = mag.SourcePrefab;
        List<AnopiaSourcerer> layers = new List<AnopiaSourcerer>();
        foreach(ClipData d in mag.Data)
        {
            GameObject newg = UnityEngine.Object.Instantiate(prefab, t);
            //newg.transform.localPosition = Vector3.zero;
            AnopiaSourcerer s = newg.AddComponent<AnopiaSourcerer>();
            s.SetData(d);
            s.SetOutput(output);
            layers.Add(s);
            s.Source.loop = true;
            s.Source.Play();
        }
        return layers.ToArray();
    }
    public static void PlayOneShot(this AudioSource source, string SoundID, int key)
    {
        anClipMag mag = (anClipMag)FetchMag(SoundID);
        source.PlayOneShot(mag.Data[key]);
    }
    public static void PlayOneShot(this AudioSource source, ClipData data)
    {
        source.PlayOneShot(data.Clip, data.Gain);
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
    public static IEnumerator DeleteWhenDone(AudioSource source, double stopTime)
    {
        while ((AudioSettings.dspTime< stopTime) || source.isPlaying)
            yield return new WaitForEndOfFrame();
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
    public static IEnumerator PanToOpposite(AudioSource source)
    {//remove soon if not using
        float elapsedTime = 0, startVal = source.panStereo, EffectTime = source.clip.length;
        for (; elapsedTime < EffectTime; elapsedTime += Time.deltaTime)
        {
            source.panStereo = Mathf.Lerp(startVal, -startVal, elapsedTime / EffectTime);
            yield return new WaitForEndOfFrame();
        }
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
    public abstract void Stop();
}
[Serializable]
public class AudioCurve
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