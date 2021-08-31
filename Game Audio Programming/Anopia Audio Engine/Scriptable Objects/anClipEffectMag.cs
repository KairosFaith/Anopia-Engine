using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "OneShotEffectMag", menuName = "AnopiaEngine/OneShotEffect", order = 2)]
public class anClipEffectMag : anClipMag
{
    [Range(0, 1)]
    public float MinPitch = 1;
    [Range(1, 2)]
    public float MaxPitch = 1;
    //TODO add more effects
    public bool Distortion;
    [HideInInspector]
    public float MinDistortion;
    [HideInInspector]
    public float MaxDistortion;
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        return new anClipEffectsEvent(host, this, output);
    }
}
public class anClipEffectsEvent : IanEvent
{
    public GameObject SourcePrefab;
    public ClipData[] Data;
    List<ClipData> RandomBag;
    float VolumeFlux;
    public float MinPitch = 1;
    public float MaxPitch = 1;
    //TODO add more effects
    public bool Distortion;
    public float MinDistortion;
    public float MaxDistortion;
    AudioMixerGroup Output;
    MonoBehaviour Host;
    public anClipEffectsEvent(MonoBehaviour host, IanAudioMag mag, AudioMixerGroup output) : base(host, mag, output)
    {
        anClipEffectMag Mag = (anClipEffectMag)mag;
        Data = Mag.Data;
        SourcePrefab = Mag.SourcePrefab;
        VolumeFlux = Mag.VolumeFlux;
        MinPitch = Mag.MinPitch;
        if (Mag.Distortion)
        {
            MinDistortion = Mag.MinDistortion;
            MaxDistortion = Mag.MaxDistortion;
            Distortion = Mag.Distortion;
        }
        //TODO add more effects
        MaxPitch = Mag.MaxPitch;
        Output = output;
        Host = host;
    }
    public override void Play(params object[] args)
    {
        int key = UnityEngine.Random.Range(0, RandomBag.Count);
        float vol = UnityEngine.Random.Range(-VolumeFlux, VolumeFlux);
        float p = UnityEngine.Random.Range(MinPitch, MaxPitch);
        ClipData toPlay = Data[key];
        Action<AnopiaSourcerer> setup = (s) =>
        {
            s.Volume += vol;
            s.Pitch = p;
        };
        if (MaxDistortion > 0)
        {
            float d = UnityEngine.Random.Range(MinDistortion, MaxDistortion);
            setup += (s) =>
            {
                s.Distortion = d;
            };
        }
        Vector3 pos = args.Length <= 0 ? Host.transform.position : (Vector3)args[0];
        AnopiaAudioCore.PlayClipAtPoint(pos, toPlay, Output, SourcePrefab, setup);
        RandomBag.RemoveAt(key);
        if (RandomBag.Count <= 0)
            RandomBag = new List<ClipData>(Data);
    }
    public override void Stop()
    {
        throw new NotImplementedException();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(anClipEffectMag))]
public class ClipEffect_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        anClipEffectMag script = (anClipEffectMag)target;
        if (script.Distortion) // if bool is true, show other fields
            EditorGUILayout.MinMaxSlider(ref script.MinDistortion, ref script.MaxDistortion, 0,1);
        //TODO add more effects
    }
}
#endif