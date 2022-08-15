using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "LayerMag", menuName = "AnopiaEngine/LayerMag", order = 4)]
public class anLayerMag : IanAudioMag
{
    public anClipData[] Layers;
    public LayerAutomationData[] AutomationData;
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        return new anLayerEvent(driver, this, output);
    }
}
public class anLayerEvent : IanEvent
{
    public override bool UsingDriverSource => false;
    Dictionary<string, LayerAutomationData> _settingslogic = new Dictionary<string, LayerAutomationData>();
    public anSourcerer[] SourceObjects;
    public anLayerEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        anLayerMag Mag = (anLayerMag)mag;
        SourceObjects = anCore.SetLayers(driver, output, Mag.Layers);
        foreach(LayerAutomationData lscurve in Mag.AutomationData)
            _settingslogic.Add(lscurve.ParameterName, lscurve);
    }
    public override void SetParameter(string name, float value)
    {
        if (_settingslogic.TryGetValue(name, out LayerAutomationData lerpData))
        {
            float valueToSet = lerpData.EvaluateUnnormalised(value);
            SourceObjects[lerpData.TargetSource].SetEffect(lerpData.Effect, valueToSet);
        }
        else
            throw new Exception(name + "parameter key not registered");
    }
    public override void Play(params object[] args)
    {
        foreach (anSourcerer s in SourceObjects)
            s.audioSource.Play();
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        foreach (anSourcerer s in SourceObjects)
            s.audioSource.PlayScheduled(timecode);
    }
    public override void Stop()
    {
        foreach (anSourcerer s in SourceObjects)
            s.audioSource.Stop();
    }
}