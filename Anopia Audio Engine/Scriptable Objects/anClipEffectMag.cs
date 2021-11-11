using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "ClipEffectMag", menuName = "AnopiaEngine/ClipEffect", order = 2)]
public class anClipEffectMag : anClipMag
{
    public GameObject SourcePrefab;
    [Range(0, 1)]
    public float MinPitch = 1;
    [Range(1, 2)]
    public float MaxPitch = 1;
    [HideInInspector]
    public bool UseDistortion;
    [HideInInspector]
    public float MinDistortion, MaxDistortion;
    [HideInInspector]
    public bool UseHighPass;
    [HideInInspector]
    public float MinHighPass = 10, MaxHighPass = 22000;
    //TODO add more effects
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        return new anClipEffectsEvent(host, this, output);
    }
}
public class anClipEffectsEvent : IanEvent
{
    public GameObject SourcePrefab;
    public ClipData[] Data;
    List<ClipData> RandomBag = new List<ClipData>();
    float VolumeFlux;
    public float MinPitch = 1;
    public float MaxPitch = 1;
    //TODO add more effects
    public bool UseDistortion;
    public float MinDistortion;
    public float MaxDistortion;
    public bool UseHighPass;
    public float MinHighPass;
    public float MaxHighPass;
    AudioMixerGroup Output;
    MonoBehaviour Host;
    public anClipEffectsEvent(MonoBehaviour host, IanAudioMag mag, AudioMixerGroup output) : base(host, mag, output)
    {
        anClipEffectMag Mag = (anClipEffectMag)mag;
        Data = Mag.Data;
        SourcePrefab = Mag.SourcePrefab;
        VolumeFlux = Mag.VolumeFluctuation;
        MinPitch = Mag.MinPitch;
        UseDistortion = Mag.UseDistortion;
        if (UseDistortion)
        {
            MinDistortion = Mag.MinDistortion;
            MaxDistortion = Mag.MaxDistortion;
        }
        UseHighPass = Mag.UseHighPass;
        if (UseHighPass)
        {
            MinHighPass = Mag.MinHighPass;
            MaxHighPass = Mag.MaxHighPass;
        }
        //TODO add more effects
        MaxPitch = Mag.MaxPitch;
        Output = output;
        Host = host;
    }
    void SetupPlay(out int key, out Vector3 pos, out AudioClip clip, out float vol, out Action<AnopiaSourcerer> setup, params object[] args)
    {
        if (RandomBag.Count <= 0)
            RandomBag = new List<ClipData>(Data);
        key = UnityEngine.Random.Range(0, RandomBag.Count);
        ClipData toPlay = Data[key];
        clip = toPlay.Clip;
        vol = toPlay.Gain;
        if (args.Length > 0)
            vol *= (float)args[0];//scale volume
        vol += UnityEngine.Random.Range(-VolumeFlux, VolumeFlux);
        float p = UnityEngine.Random.Range(MinPitch, MaxPitch);
        setup = (s) =>
        {
            AudioSource a = s.audioSource;
            a.pitch = p;
        };
        if (UseDistortion)
        {
            float d = UnityEngine.Random.Range(MinDistortion, MaxDistortion);
            setup += (s) =>
            {
                s.audioDistortionFilter.distortionLevel = d;
            };
        }
        if (UseHighPass)
        {
            float hpass = UnityEngine.Random.Range(MinHighPass, MaxHighPass);
            setup += (s) =>
            {
                s.audioHighPassFilter.cutoffFrequency = hpass;
            };
        }
        //TODO add more effects
        pos = args.Length < 2 ? Host.transform.position : (Vector3)args[1];
    }
    public override void Play(params object[] args)//vector3, volume
    {
        SetupPlay(out int keytoremove, out Vector3 pos, out AudioClip clip, out float vol, out Action<AnopiaSourcerer> setup, args);
        AnopiaAudioCore.PlayClipAtPoint(pos, clip, vol, Output, SourcePrefab, setup);
        RandomBag.RemoveAt(keytoremove);
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        SetupPlay(out int keytoremove, out Vector3 pos, out AudioClip clip, out float vol, out Action<AnopiaSourcerer> setup, args);
        AnopiaAudioCore.PlayClipAtSchedule(Host.transform, clip, vol,timecode, Output, SourcePrefab, setup);
        RandomBag.RemoveAt(keytoremove);
    }
    public override void Stop()
    {

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
        script.UseDistortion = EditorGUILayout.Toggle("Use Distortion", script.UseDistortion);
        if (script.UseDistortion) // if bool is true, show other fields
        {
            EditorGUILayout.MinMaxSlider(new GUIContent(),ref script.MinDistortion, ref script.MaxDistortion, 0, 1);
            script.MinDistortion = EditorGUILayout.FloatField(new GUIContent("Min Distortion"), script.MinDistortion);
            script.MaxDistortion = EditorGUILayout.FloatField(new GUIContent("Max Distortion"), script.MaxDistortion);
        }
        script.UseHighPass = EditorGUILayout.Toggle("Use HighPass", script.UseHighPass);
        if (script.UseHighPass)
        {
            EditorGUILayout.MinMaxSlider(new GUIContent(),ref script.MinHighPass, ref script.MaxHighPass, 10, 22000);
            script.MinHighPass = EditorGUILayout.FloatField(new GUIContent("Min HighPass Hz"), script.MinHighPass);
            script.MaxHighPass = EditorGUILayout.FloatField(new GUIContent("Max HighPass Hz"), script.MaxHighPass);
        }
        //TODO add more effects
    }
}
#endif