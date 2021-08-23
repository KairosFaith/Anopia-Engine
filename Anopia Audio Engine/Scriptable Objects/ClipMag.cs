using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag", order = 1)]
public class ClipMag : IAnopiaAudioMag
{
    public GameObject SourcePrefab;
    public ClipData[] Data;
    [Range(0, 1)]
    public float VolumeFlux;
    public override IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        return new AnopiaOneShotEvent(host, this, output);
    }
}
public class AnopiaOneShotEvent : IAnopiaEvent
{
    ClipData[] Data;
    List<ClipData> RandomBag;
    AnopiaSourcerer Sourcerer;
    float VolumeFlux;
    public AnopiaOneShotEvent(MonoBehaviour host, IAnopiaAudioMag mag, AudioMixerGroup output) : base(host,mag, output) 
    {
        ClipMag Mag = (ClipMag)mag;
        Data = Mag.Data;
        RandomBag = new List<ClipData>(Data);
        VolumeFlux = Mag.VolumeFlux;
        Sourcerer = AnopiaAudioCore.NewPointSource(host, Mag.SourcePrefab, output);
    }
    public override void Play(params object[] args)
    {
        int key = UnityEngine.Random.Range(0, RandomBag.Count);
        float vol = Random.Range(-VolumeFlux, VolumeFlux);
        Sourcerer.Source.PlayOneShot(Data[key]);
        RandomBag.RemoveAt(key);
        if(RandomBag.Count<=0)
           RandomBag = new List<ClipData>(Data);
    }
    public override void Stop()
    {

    }
}