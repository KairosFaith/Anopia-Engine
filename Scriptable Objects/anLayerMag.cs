using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "LayerMag", menuName = "AnopiaEngine/LayerMag")]
public class anLayerMag : IanAudioMag
{
    public AudioLayerData[] LayerData;
    public LayerAutomationData[] AutomationData;
    Dictionary<string, Action<float, anSourcerer[]>> Setters
    {
        get
        {
            if (_Setters == null)
                InitActionBank();
            return _Setters;
        }
    }
    Dictionary<string, Action<float, anSourcerer[]> > _Setters;
    public void InitActionBank()
    {
        _Setters = new Dictionary<string, Action<float, anSourcerer[]>>();
        foreach (LayerAutomationData layerData in AutomationData)
            _Setters.Add(layerData.ParameterName, layerData.AutomationAction);
    }
    public void SetParam(string paramName, float value, anSourcerer[] sourceHandlers)
    {
        Action<float, anSourcerer[]> a = Setters[paramName];
        a.Invoke(value, sourceHandlers);
    }
    public anSourcerer[] Setup(Transform parent)
    {
        List<anSourcerer> SourceHandlers = new List<anSourcerer>();
        foreach (AudioLayerData l in LayerData)
        {
            anSourcerer a = Instantiate(l.SourcePrefab, parent);
            a.audioSource.clip = l.Clip;
            SourceHandlers.Add(a);
        }
        return SourceHandlers.ToArray();
    }
    public anSourcerer[] Play(Transform parent)
    {
        anSourcerer[] SourceHandlers = Setup(parent);
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.Play();
        return SourceHandlers;
    }
}
[Serializable]
public class AudioLayerData
{
    public AudioClip Clip;
    public anSourcerer SourcePrefab;
}
[Serializable]
public class AudioAutomationData
{
    public string ParameterName;
    public float MinInputValue, MaxInputValue;
    public SourceEffect Effect;
    public anLerpCurve Smoothing;
    protected System.Reflection.PropertyInfo pInfo;
    public AudioAutomationData()
    {
        pInfo = typeof(anSourcerer).GetProperty(Effect.ToString());
    }
    public float ProcessValue(float unnormalisedValue)
    {
        float t = Mathf.InverseLerp(MinInputValue, MaxInputValue, unnormalisedValue);
        return Smoothing.Evaluate(t);
    }
    public void SetSourcererValue(anSourcerer handler, float value)
    {
        pInfo.SetValue(handler, value);
    }
}
[Serializable]
public class LayerAutomationData : AudioAutomationData
{
    public int TargetSourceIndex;
    public void AutomationAction(float value, anSourcerer[] handlers)
    {
        float e = ProcessValue(value);
        SetSourcererValue(handlers[TargetSourceIndex], e);
    }
}