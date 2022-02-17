using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SpeechMag", menuName = "AnopiaEngine/Speech", order = 10)]
public class anSpeechMag : IanAudioMag
{
    public AudioClip[] Clips;
    public anSourcerer SourcePrefab;
    public override IanEvent LoadMag(MonoBehaviour driver, AudioMixerGroup output)
    {
        return new anSpeechEvent(driver, this, output);
    }
}
public class anSpeechEvent : IanEvent
{
    Dictionary<string, AudioClip> _SpeechBank = new Dictionary<string, AudioClip>();
    MonoBehaviour Driver;
    AudioMixerGroup Output;
    anSourcerer SourcePrefab;
    public anSpeechEvent(MonoBehaviour driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        Output = output;
        anSpeechMag Mag = (anSpeechMag)mag;
        Driver = driver;
        SourcePrefab = Mag.SourcePrefab;
        foreach (AudioClip c in Mag.Clips)
            _SpeechBank.Add(c.name, c);
    }
    public override void Play(params object[] args)
    {
        double timecode = AudioSettings.dspTime + Time.deltaTime;
        foreach(object o in args)
            if(_SpeechBank.TryGetValue((string)o, out AudioClip clip))
            {
                anCore.PlayClipScheduled(Driver.transform, clip, 1, timecode, Output, SourcePrefab);
                timecode += clip.length;
            }
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        if (_SpeechBank.TryGetValue((string)args[0], out AudioClip clip))
            anCore.PlayClipScheduled(Driver.transform, clip, 1, timecode, Output, SourcePrefab);
    }

    public override void SetParameter(string name, float value, params object[] args)
    {

    }

    public override void Stop()
    {

    }
}