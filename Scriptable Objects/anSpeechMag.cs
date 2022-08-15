using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SpeechMag", menuName = "AnopiaEngine/SpeechMag", order = 5)]
public class anSpeechMag : anClipMag
{
    public bool SingleVoiceOnly = true;
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        return new anSpeechEvent(driver, this, output);
    }
}
public class anSpeechEvent : IanEvent
{
    public override bool UsingDriverSource => true;
    Dictionary<string, anClipData> _SpeechBank = new Dictionary<string, anClipData>();
    public bool SingleVoiceOnly;
    anDriver Driver;
    AudioMixerGroup Output;
    anSourcerer SourcePrefab;
    public anSpeechEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        Output = output;
        anSpeechMag Mag = (anSpeechMag)mag;
        Driver = driver;
        SingleVoiceOnly = Mag.SingleVoiceOnly;
        SourcePrefab = driver.SourcePrefab;
        foreach (anClipData c in Mag.Data)
            _SpeechBank.Add(c.Clip.name, c);
    }
    public override void Play(params object[] args)
    {
        if (SingleVoiceOnly)
            Driver.OneShotSource.audioSource.Stop();
        string msg = (string)args[0];
        if (_SpeechBank.TryGetValue(msg, out anClipData clip))
            Driver.OneShotSource.audioSource.PlayOneShot(clip.Clip,clip.Gain);
        else
            throw new System.Exception(msg + "clip does not exist in the SpeechMag");
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        foreach (object o in args)
        {
            string msg = (string)o;
            if (_SpeechBank.TryGetValue(msg, out anClipData clip))
            {
                anCore.PlayClipScheduled(Driver.transform, clip.Clip, clip.Gain, timecode, Output, SourcePrefab);
                timecode += clip.Clip.length;
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