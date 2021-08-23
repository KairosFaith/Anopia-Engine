using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
public static class AnopiaAudioCore
{
    static Dictionary<string, IAnopiaAudioMag> _SoundBank = new Dictionary<string, IAnopiaAudioMag>();
    static AnopiaAudioCore()
    {
        IAnopiaAudioMag[] m = Resources.LoadAll<IAnopiaAudioMag>("AudioMags");
        foreach (IAnopiaAudioMag b in m)
            _SoundBank.Add(b.name, b);
    }
    public static IAnopiaAudioMag FetchMag(string SoundID)
    {
        if (_SoundBank.TryGetValue(SoundID, out IAnopiaAudioMag mag))
            return mag;
        else
            throw new Exception(SoundID + " not found");
    }
    public static IAnopiaEvent NewEvent(MonoBehaviour host, string SoundID, AudioMixerGroup output)
    {
        IAnopiaAudioMag mag = FetchMag(SoundID);
        return mag.LoadMag(host, output);
    }
    public static AnopiaSourcerer PlayClipAtPoint(Vector3 position, ClipData data, AudioMixerGroup output, GameObject prefab, Action<AnopiaSourcerer> setup = null)
    {
        GameObject newg = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        AnopiaSourcerer sourceObj = newg.AddComponent<AnopiaSourcerer>();
        sourceObj.SetOutput(output);
        setup?.Invoke(sourceObj);
        sourceObj.StartCoroutine(DeleteWhenDone(sourceObj.Source, null));
        return sourceObj;
    }
    public static AnopiaSourcerer PlayClipAtStereo(MonoBehaviour host, ClipData data, AudioMixerGroup output, float panStereo = 0,Action OnDone = null)
    {
        AudioSource a = NewStereoSource(host, output);
        AnopiaSourcerer sourceObj = a.gameObject.AddComponent<AnopiaSourcerer>();
        sourceObj.Source = a;
        a.panStereo = panStereo;
        sourceObj.StartCoroutine(DeleteWhenDone(a, OnDone));
        return sourceObj;
    }
    public static AnopiaSourcerer NewPointSource(MonoBehaviour host, GameObject prefab, AudioMixerGroup output)
    {
        GameObject newg = UnityEngine.Object.Instantiate(prefab, host.transform);
        AnopiaSourcerer s = newg.AddComponent<AnopiaSourcerer>();
        s.SetOutput(output);
        return s;
    }
    public static AudioSource NewStereoSource(MonoBehaviour host, AudioMixerGroup output)
    {
        GameObject newg = new GameObject(host + " " + output+ " AudioSource");
        AudioSource sor = newg.AddComponent<AudioSource>();
        sor.outputAudioMixerGroup = output;
        return sor;
    }
    public static AnopiaSourcerer[] SetLayers(MonoBehaviour host, string SoundID, AudioMixerGroup output)
    {
        LayerMag mag = (LayerMag)FetchMag(SoundID);
        Transform t = host.transform;
        List<AnopiaSourcerer> layers = new List<AnopiaSourcerer>();
        foreach(ClipLayer l in mag.Layers)
        {
            GameObject newg = UnityEngine.Object.Instantiate(l.SourcePrefab, t);
            AnopiaSourcerer s = newg.AddComponent<AnopiaSourcerer>();
            s.SetData(l.Data);
            s.SetOutput(output);
            layers.Add(s);
            s.Source.Play();
        }
        return layers.ToArray();
    }
    public static void PlayOneShot(this AudioSource source, string SoundID, int key)
    {
        ClipMag mag = (ClipMag)FetchMag(SoundID);
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
    static IEnumerator DeleteWhenDone(AudioSource source, Action onDone = null)
    {
        source.Play();
        yield return new WaitForSecondsRealtime(source.clip.length);
        while (source.isPlaying)
            yield return new WaitForEndOfFrame();
        onDone?.Invoke();
        UnityEngine.Object.Destroy(source.gameObject);
    }
    public static IEnumerator PanToOpposite(AudioSource source)
    {
        float elapsedTime = 0, startVal = source.panStereo, EffectTime = source.clip.length;
        for (; elapsedTime < EffectTime; elapsedTime += Time.unscaledDeltaTime)
        {
            source.panStereo = Mathf.Lerp(startVal, -startVal, elapsedTime / EffectTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
public abstract class IAnopiaAudioMag : ScriptableObject
{
    public abstract IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output);
}
[Serializable]
public class ClipData
{
    public AudioClip Clip;
    [Range(0, 1)]
    public float Gain = 1;
}
public abstract class IAnopiaEvent
{
    public IAnopiaEvent(MonoBehaviour host, IAnopiaAudioMag mag, AudioMixerGroup output){ }
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