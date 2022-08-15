using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag", order = 1)]
public class anClipMag : IanAudioMag
{
    public anClipData[] Data;
    [Range(0, 1)]
    public float MinRandomVolume = 1;
    public AudioClip RandomClip(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Data.Length);
        anClipData d = Data[key];
        gain = d.Gain + UnityEngine.Random.Range(-MinRandomVolume, MinRandomVolume);
        return d.Clip;
    }
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        return new anOneShotEvent(driver, this, output);
    }
}
public class anOneShotEvent : IanEvent
{
    float volumeFlux;
    anClipData[] AudioData;
    List<anClipData> RandomBag;
    anDriver Driver;
    public override bool UsingDriverSource => true;
    public anOneShotEvent(anDriver driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        anClipMag Mag = (anClipMag)mag;
        Driver = driver;
        AudioData = Mag.Data;
        RandomBag = new List<anClipData>(AudioData);
        volumeFlux = Mag.MinRandomVolume;
    }
    public override void Play(params object[] args)
    {
        int key;
        if (args.Length > 1)
            key = (int)args[1];
        else
            key = UnityEngine.Random.Range(0, RandomBag.Count);
        anClipData d = AudioData[key];
        float vol = d.Gain;
        if (args.Length > 0)//randomise volume unless indicated in args
            vol *= (float)args[0];
        else
            vol += UnityEngine.Random.Range(volumeFlux, 1f);
        Driver.OneShotSource.audioSource.PlayOneShot(d.Clip,vol);
        RandomBag.RemoveAt(key);
        if (RandomBag.Count <= 0)
            RandomBag = new List<anClipData>(AudioData);
    }
    public override void Stop()
    {
        Driver.OneShotSource.audioSource.Stop();
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        throw new System.NotImplementedException("Use ClipEffect instead, to spawn source for each voice");
    }

    public override void SetParameter(string name, float value)
    {
        //no other parameters available, just use play args[0] to set volume
    }
}