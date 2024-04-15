using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "LayerParamMag", menuName = "AnopiaEngine/LayerParamMag")]
public class anLayerParamMag : IanAudioMag
{
    public AudioLayerData[] Layer;
    public LayerAutomationData[] Automation;
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
        foreach (LayerAutomationData layerData in Automation)
            _Setters.Add(layerData.ParameterName, layerData.AutomationAction);
    }
    public void SetParam(string paramName, float value, anSourcerer[] sourceHandlers)
    {
        Action<float, anSourcerer[]> a = Setters[paramName];
        a.Invoke(value, sourceHandlers);
    }
    public anSourcerer[] Play(Transform parent, AudioMixerGroup Channel)
    {
        List<anSourcerer> SourceHandlers = new List<anSourcerer>();
        foreach (AudioLayerData l in Layer)
        {
            anSourcerer a = Instantiate(l.SourcePrefab, parent);
            a.SetUp(l.Clip, Channel, true);
            a.audioSource.Play();
            SourceHandlers.Add(a);
        }
        return SourceHandlers.ToArray();
    }
}