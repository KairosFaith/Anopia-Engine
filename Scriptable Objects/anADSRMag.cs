using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 3)]
public class anADSRMag : IanAudioMag
{
    //sustain gain affects attack and release too
    public AudioClip Attack;
    public AudioClip Sustain;
    public AudioClip Release;
    [Range(0, 1)]
    public float Gain = 1;
    //TODO custom inspector?
    public AudioAutomationData[] AutomationData;
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        return new anADSREvent(driver, this, output);
    }
}
public class anADSREvent : IanEvent
{
    public AudioClip Attack;
    public AudioClip Release;
    public anSourcerer Sourcerer;
    Dictionary<string, AudioAutomationData> _settingslogic = new Dictionary<string, AudioAutomationData>();
    public anADSREvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        anADSRMag Mag = (anADSRMag)mag;
        Attack = Mag.Attack;
        Release = Mag.Release;
        Sourcerer = Object.Instantiate(driver.SourcePrefab, driver.transform);
        AudioSource a = Sourcerer.audioSource;
        a.clip = Mag.Sustain;
        a.volume = Mag.Gain;
        a.loop = true;
        foreach (AudioAutomationData lscurve in Mag.AutomationData)
            _settingslogic.Add(lscurve.ParameterName, lscurve);
    }
    public override void Play(params object[] args)
    {
        AudioSource s = Sourcerer.audioSource;
        s.PlayOneShot(Attack);
        s.Play();
    }
    public override void Stop()
    {
        AudioSource s = Sourcerer.audioSource;
        s.Stop();
        s.PlayOneShot(Release);
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        Sourcerer.audioSource.PlayScheduled(timecode);
    }
    public override void SetParameter(string name, float value)
    {
        if (_settingslogic.TryGetValue(name, out AudioAutomationData lerpData))
        {
            float valueToSet = lerpData.EvaluateUnnormalised(value);
            Sourcerer.SetEffect(lerpData.Effect, valueToSet);
        }
        else
            throw new System.Exception(name + "parameter key not registered");
    }
}