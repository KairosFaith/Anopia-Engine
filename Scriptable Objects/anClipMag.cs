using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag", order = 1)]
public class anClipMag : IanAudioMag
{
    public ClipData[] Data;
    [Range(0, 1)]
    public float VolumeFluctuation;
    public AudioClip RandomClip(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Data.Length);
        ClipData d = Data[key];
        gain = d.Gain + UnityEngine.Random.Range(-VolumeFluctuation, VolumeFluctuation);
        return d.Clip;
    }
    public override IanEvent LoadMag(MonoBehaviour driver, AudioMixerGroup output)
    {
        return new anOneShotEvent(driver, this, output);
    }
}
public class anOneShotEvent : IanEvent
{
    float VolumeFlux;
    ClipData[] AudioData;
    List<ClipData> RandomBag;
    public AudioSource audioSource;//assign using driver or directly
    public anOneShotEvent(MonoBehaviour driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        anClipMag Mag = (anClipMag)mag;
        AudioData = Mag.Data;
        RandomBag = new List<ClipData>(AudioData);
        VolumeFlux = Mag.VolumeFluctuation;
        audioSource.outputAudioMixerGroup = output;
    }
    public override void Play(params object[] args)
    {
        int key;
        if (args.Length > 1)
            key = (int)args[1];
        else
            key = UnityEngine.Random.Range(0, RandomBag.Count);
        ClipData d = AudioData[key];
        float vol = d.Gain;
        if (args.Length > 0)//randomise volume unless indicated in args
            vol *= (float)args[0];
        vol += UnityEngine.Random.Range(-VolumeFlux, VolumeFlux);
        audioSource.PlayOneShot(d.Clip,vol);
        RandomBag.RemoveAt(key);
        if (RandomBag.Count <= 0)
            RandomBag = new List<ClipData>(AudioData);
    }
    public override void Stop()
    {
        audioSource.Stop();
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        throw new System.NotImplementedException("Use ClipEffect instead, to spawn source for each voice");
    }

    public override void SetParameter(string name, float value, params object[] args)
    {

    }
}