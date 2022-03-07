using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "ClipObjectMag", menuName = "AnopiaEngine/ClipObjectMag", order = 2)]
public class anClipObjectMag : anClipMag
{
    [Range(0, 1)]
    public float MinPitch = 1;
    [Range(1, 2)]
    public float MaxPitch = 1;
    public AutomationMode PitchAutomationMode;
    [HideInInspector]
    public bool UseDistortion, UseHighPass;
    [HideInInspector]
    public AutomationMode DistortionAutomationMode, HighPassAutomationMode;
    [HideInInspector]
    public AudioAutomationData PitchCurve, DistortionCurve, HighPassCurve;
    //custom inspector
    public Dictionary<AudioAutomationData, AutomationMode> AutomationToLoad = new Dictionary<AudioAutomationData, AutomationMode>();
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        PitchCurve.Effect = SourceEffect.Pitch;
        DistortionCurve.Effect = SourceEffect.Distortion;
        HighPassCurve.Effect = SourceEffect.HighPass;
        AutomationToLoad.Add(PitchCurve, PitchAutomationMode);
        AutomationToLoad.Add(DistortionCurve, DistortionAutomationMode);
        AutomationToLoad.Add(HighPassCurve, HighPassAutomationMode);
        return new anClipObjectEvent(driver, this, output);
    }
}
public class anClipObjectEvent : IanEvent
{
    public override bool UsingDriverSource => false;
    public anSourcerer SourcePrefab;
    public ClipData[] Data;
    List<ClipData> RandomBag = new List<ClipData>();
    float volumeFlux;
    AudioMixerGroup Output;
    MonoBehaviour Driver;
    Dictionary<string, AudioAutomationData> _settingsRandom = new Dictionary<string, AudioAutomationData>();
    Dictionary<string, AudioAutomationData> _settingslogic = new Dictionary<string, AudioAutomationData>();
    Dictionary<SourceEffect, float> _LatchSettings = new Dictionary<SourceEffect, float>();
    public anClipObjectEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        anClipObjectMag Mag = (anClipObjectMag)mag;
        Data = Mag.Data;
        SourcePrefab = driver.SourcePrefab;
        Output = output;
        Driver = driver;
        volumeFlux = Mag.MinRandomVolume;

        foreach(KeyValuePair<AudioAutomationData, AutomationMode> pair in Mag.AutomationToLoad)
        {
            AudioAutomationData c = pair.Key;
            if(pair.Value == AutomationMode.Latch)
            {
                _settingslogic.Add(c.ParameterName, c);
                _LatchSettings.Add(c.Effect, c.Smoothing.Evaluate(0));
            }
            else
                _settingsRandom.Add(c.ParameterName, c);
        }
    }
    void SetupPlay(out int key, out Vector3 pos, out AudioClip clip, out float vol, out Action<anSourcerer> setup, params object[] args)
    {
        if (RandomBag.Count <= 0)
            RandomBag = new List<ClipData>(Data);
        key = UnityEngine.Random.Range(0, RandomBag.Count);
        ClipData toPlay = Data[key];
        clip = toPlay.Clip;
        vol = toPlay.Gain;
        if (args.Length > 1)
            vol *= (float)args[1];//scale volume
        else
            vol *= UnityEngine.Random.Range(volumeFlux, 1f);
        setup = (s) =>
        {
            foreach (KeyValuePair<SourceEffect, float> p in _LatchSettings)
                s.SetEffect(p.Key, p.Value);
            foreach (AudioAutomationData l in _settingsRandom.Values)
                s.SetEffect(l.Effect, l.EvaluateRandom());
        };
        pos = args.Length > 0 ? Driver.transform.position : (Vector3)args[0];
    }
    public override void Play(params object[] args)//vector3, volume
    {
        SetupPlay(out int keytoremove, out Vector3 pos, out AudioClip clip, out float vol, out Action<anSourcerer> setup, args);
        anCore.PlayClipAtPoint(pos, clip, vol, Output, SourcePrefab, setup);
        RandomBag.RemoveAt(keytoremove);
    }
    public override void SetParameter(string name, float value)
    {
        if (_settingslogic.TryGetValue(name, out AudioAutomationData lerpData))
        {
            float valueToSet = lerpData.EvaluateUnnormalised(value);
            _LatchSettings[lerpData.Effect] = valueToSet;
        }
        else
            throw new Exception(name + "parameter key not registered");
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        SetupPlay(out int keytoremove, out Vector3 pos, out AudioClip clip, out float vol, out Action<anSourcerer> setup, args);
        anCore.PlayClipScheduled(Driver.transform, clip, vol,timecode, Output, SourcePrefab, setup);
        RandomBag.RemoveAt(keytoremove);
    }
    public override void Stop()
    {

    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(anClipObjectMag))]
public class ClipEffect_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        anClipObjectMag script = (anClipObjectMag)target;

        if (script.PitchAutomationMode == AutomationMode.Latch)
        {
            script.PitchCurve.ParameterName = EditorGUILayout.TextField("Pitch Parameter Name", script.PitchCurve.ParameterName);
            script.PitchCurve.Smoothing.Curve = EditorGUILayout.CurveField("Pitch Curve", script.PitchCurve.Smoothing.Curve);
        }
        script.UseDistortion = EditorGUILayout.Toggle("Use Distortion", script.UseDistortion);
        if (script.UseDistortion) // if bool is true, show other fields
        {
            EditorGUILayout.MinMaxSlider("Distortion Range", ref script.DistortionCurve.Smoothing.LowerLimit, ref script.DistortionCurve.Smoothing.UpperLimit, 0f, 1f);
            script.DistortionAutomationMode = (AutomationMode)EditorGUILayout.EnumPopup("Distortion Automation Mode", script.DistortionAutomationMode);
            if (script.DistortionAutomationMode == AutomationMode.Latch)
            {
                script.DistortionCurve.ParameterName = EditorGUILayout.TextField("Distortion Parameter Name", script.DistortionCurve.ParameterName);
                script.DistortionCurve.Smoothing.Curve = EditorGUILayout.CurveField("Distortion Curve", script.DistortionCurve.Smoothing.Curve);
            }
        }
        script.UseHighPass = EditorGUILayout.Toggle("Use HighPass", script.UseHighPass);
        if (script.UseHighPass)
        {
            EditorGUILayout.MinMaxSlider("HighPass Range", ref script.HighPassCurve.Smoothing.LowerLimit, ref script.HighPassCurve.Smoothing.UpperLimit, 10f, 22000f);
            script.HighPassAutomationMode = (AutomationMode)EditorGUILayout.EnumPopup("HighPass Automation Mode", script.HighPassAutomationMode);
            if (script.HighPassAutomationMode == AutomationMode.Latch)
            {
                script.HighPassCurve.ParameterName = EditorGUILayout.TextField("HighPass Parameter Name", script.HighPassCurve.ParameterName);
                script.HighPassCurve.Smoothing.Curve = EditorGUILayout.CurveField("HighPass Curve", script.HighPassCurve.Smoothing.Curve);
            }
        }
    }
}
#endif
public enum AutomationMode
{
    Random,
    Latch
}