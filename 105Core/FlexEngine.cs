using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
public static class FlexEngine
{
    #region CoreBank
    static Dictionary<AudioFileID, ClipBag> _SoundBank = new Dictionary<AudioFileID, ClipBag>();
    static Dictionary<SourceID, GameObject> _SourceBank = new Dictionary<SourceID, GameObject>();
    static FlexEngine()//Load SoundBank
    {
        ClipBag[] m = Resources.LoadAll<ClipBag>("AudioClip_Bags");
        foreach (ClipBag b in m)
            _SoundBank.Add(b.ID, b);
        SourceBag[] rc = Resources.LoadAll<SourceBag>("AudioSource_Bags");
        foreach(SourceBag b in rc)
            foreach(GameObject g in b.AudioSourcesPrefab)
            {
                if (Enum.TryParse(g.name, out SourceID id))
                    _SourceBank.Add(id, g);
                else
                    throw new Exception(g.name + " invalid SourceID");
            }
    }
    #endregion
    #region Fetch
    public static ClipBag GetClips(AudioFileID tag)
    {
        if (_SoundBank.TryGetValue(tag, out ClipBag mag))
            return mag;
        else
            throw new Exception(tag + "sound clips not found");
    }
    public static GameObject GetSource(SourceID tag)
    {
        if (_SourceBank.TryGetValue(tag, out GameObject sourceObject))
            return sourceObject;
        else
            throw new Exception(tag + "source prefab not found");
    }
    static AudioSource SetUpSource(GameObject SoundObject, OutputPan type, AudioMixerGroup output)
    {
        AudioSource a = SoundObject.GetComponent<AudioSource>();
        a.outputAudioMixerGroup = output;
        switch (type)
        {
            case OutputPan.Point:
                a.spatialBlend = 1;
                a.bypassListenerEffects = false;
                a.bypassReverbZones = false;
                break;
            case OutputPan.Stereo:
            case OutputPan.StereoL:
            case OutputPan.StereoR:
                a.spatialBlend = 0;
                a.bypassListenerEffects = true;
                a.bypassReverbZones = true;
                a.panStereo = (int)type;
                break;
        }
        return a;
    }
    public static AudioSource NewSource(Transform sender, OutputPan type, AudioMixerGroup output, SourceID sourceTag = 0)
    {
        GameObject SoundObject = UnityEngine.Object.Instantiate(GetSource(sourceTag), sender.transform);
        AudioSource a = SetUpSource(SoundObject, type, output);
        a.loop = true;//use play one shot for non-loop sounds
        return a;
    }
    public static FlexSourceHandler ClipSource(Vector3 position, ClipData data, AudioMixerGroup output, OutputPan type, SourceID sourceTag, out AudioSource source)
    {
        GameObject SoundObject = UnityEngine.Object.Instantiate(GetSource(sourceTag), position, Quaternion.identity);
        AudioSource a = SetUpSource(SoundObject, type, output);
        FlexSourceHandler sourceObj = SoundObject.AddComponent<FlexSourceHandler>();
        sourceObj.Source = source = a;
        a.loop = false;
        a.clip = data.Clip;
        a.volume = data.Gain;
        return sourceObj;
    }
    #endregion
    public static FlexSourceHandler NewHandler(MonoBehaviour host, OutputPan type, AudioMixerGroup output, SourceID sourceTag = 0)
    {
        AudioSource a = NewSource(host.transform, type, output, sourceTag);
        FlexSourceHandler h = a.gameObject.AddComponent<FlexSourceHandler>();
        h.SetUp(a,sourceTag);
        return h;
    }
    public static FlexSourceHandler PlayClipAtPoint(Vector3 position, ClipData data, AudioMixerGroup output, SourceID sourceTag, Action<FlexSourceHandler> setup = null)
    {
        FlexSourceHandler sourceObj = ClipSource(position, data, output, OutputPan.Point, sourceTag, out AudioSource a);
        setup?.Invoke(sourceObj);
        sourceObj.StartCoroutine(DeleteWhenDone(a,null));
        return sourceObj;
    }
    public static FlexSourceHandler PlayClipAtStereo(MonoBehaviour host, ClipData data, AudioMixerGroup output, Action OnDone)//for music
    {
        FlexSourceHandler sourceObj = ClipSource(host.transform.position, data, output, OutputPan.Stereo, 0, out AudioSource a);
        sourceObj.StartCoroutine(DeleteWhenDone(a, OnDone));
        return sourceObj;
    }
    public static FlexSourceHandler PlayClipAtStereo(MonoBehaviour host, ClipData data, AudioMixerGroup output, OutputPan type = 0)//for ui sounds
    {
        FlexSourceHandler sourceObj = ClipSource(host.transform.position, data, output, type, 0, out AudioSource a);
        a.ignoreListenerPause = false;
        sourceObj.StartCoroutine(DeleteWhenDone(a));
        return sourceObj;
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
        for (; elapsedTime< EffectTime; elapsedTime+=Time.unscaledDeltaTime )
        {
            source.panStereo = Mathf.Lerp(startVal, -startVal, elapsedTime / EffectTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public static void TransitionToSnapshot(this AudioMixer mixer, string SnapshotName, float TimeToReach)
    {
        AudioMixerSnapshot snapshot = mixer.FindSnapshot(SnapshotName);
        AudioMixerSnapshot[] ss = new AudioMixerSnapshot[] { snapshot };
        float[] w = new float[] { 1 };
        mixer.TransitionToSnapshots(ss, w, TimeToReach);
        //the weight is nonsense, dun mess with it
    }
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
public enum OutputPan
{
    NoPreset = -2,
    StereoL = -1,
    Stereo = 0,
    StereoR = 1,
    Point,
}