using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag", order = 1)]
public class anClipMag : IanAudioMag
{
    public GameObject SourcePrefab;
    public ClipData[] Data;
    [Range(0, 1)]
    public float VolumeFlux;
    public AudioClip RandomClip(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Data.Length);
        ClipData d = Data[key];
        gain = d.Gain + UnityEngine.Random.Range(-VolumeFlux, VolumeFlux);
        return d.Clip;
    }
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException("Just directly call AnopiaSourcerer.PlayOneShot(soundId)");
    }
}
public class anOneShotEvent : IanEvent
{
    float VolumeFlux;
    ClipData[] Data;
    List<ClipData> RandomBag;
    public AnopiaSourcerer Sourcerer;
    public GameObject SourcePrefab;
    public anOneShotEvent(MonoBehaviour host, IanAudioMag mag, AudioMixerGroup output) : base(host, mag, output)
    {
        anClipMag Mag = (anClipMag)mag;
        Data = Mag.Data;
        RandomBag = new List<ClipData>(Data);
        VolumeFlux = Mag.VolumeFlux;
        SourcePrefab = Mag.SourcePrefab;
    }
    public override void Play(params object[] args)
    {
        int key = UnityEngine.Random.Range(0, RandomBag.Count);
        float vol = Random.Range(-VolumeFlux, VolumeFlux);
        ClipData d = Data[key];
        Sourcerer.Source.PlayOneShot(d.Clip,d.Gain+vol);
        RandomBag.RemoveAt(key);
        if (RandomBag.Count <= 0)
            RandomBag = new List<ClipData>(Data);
    }
    public override void Stop()
    {
        throw new System.NotImplementedException();
    }
}