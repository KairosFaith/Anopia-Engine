using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SpeechMag", menuName = "AnopiaEngine/Speech", order = 10)]
public class anSpeechMag : IanAudioMag
{
    public AudioClip[] Clips;
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        return new anSpeechEvent(driver, this, output);
    }
}
public class anSpeechEvent : IanEvent
{
    public override bool UsingDriverSource => true;
    Dictionary<string, AudioClip> _SpeechBank = new Dictionary<string, AudioClip>();
    anDriver Driver;
    AudioMixerGroup Output;
    anSourcerer SourcePrefab;
    public anSpeechEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        Output = output;
        anSpeechMag Mag = (anSpeechMag)mag;
        Driver = driver;
        SourcePrefab = driver.SourcePrefab;
        foreach (AudioClip c in Mag.Clips)
            _SpeechBank.Add(c.name, c);
    }
    public override void Play(params object[] args)
    {
        string msg = (string)args[0];
        if (_SpeechBank.TryGetValue(msg, out AudioClip clip))
        {
            Driver.OneShotSource.audioSource.clip = clip;
            Driver.OneShotSource.audioSource.Play();
        }
        else
            throw new System.Exception(msg + "clip does not exist in the SpeechMag");
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        foreach (object o in args)
        {
            string msg = (string)o;
            if (_SpeechBank.TryGetValue(msg, out AudioClip clip))
            {
                anCore.PlayClipScheduled(Driver.transform, clip, 1, timecode, Output, SourcePrefab);
                timecode += clip.length;
            }
            else
                throw new System.Exception(msg + "clip does not exist in the SpeechMag");
        }
    }
    public override void SetParameter(string name, float value)
    {
        
    }

    public override void Stop()
    {

    }
}