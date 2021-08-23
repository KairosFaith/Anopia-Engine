using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "OneShotEffectMag", menuName = "AnopiaEngine/OneShotEffect", order = 2)]
public class OneShotEffectMag : ClipMag
{
    [Range(0, 1)]
    public float MinPitch = 1;
    [Range(1, 2)]
    public float MaxPitch = 1;
    [Range(0, 1)]
    public float MaxDistortion;
    public override IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        return new AnopiaOneShotEffectsEvent(host, this, output);
    }
}
public class AnopiaOneShotEffectsEvent : IAnopiaEvent
{
    public GameObject SourcePrefab;
    public ClipData[] Data;
    List<ClipData> RandomBag;
    float VolumeFlux;
    public float MinPitch = 1;
    public float MaxPitch = 1;
    public float MaxDistortion;
    AudioMixerGroup Output;
    MonoBehaviour Host;
    public AnopiaOneShotEffectsEvent(MonoBehaviour host, IAnopiaAudioMag mag, AudioMixerGroup output) : base(host, mag, output)
    {
        OneShotEffectMag Mag = (OneShotEffectMag)mag;
        Data = Mag.Data;
        SourcePrefab = Mag.SourcePrefab;
        VolumeFlux = Mag.VolumeFlux;
        MinPitch = Mag.MinPitch;
        MaxDistortion = Mag.MaxDistortion;
        MaxPitch = Mag.MaxPitch;
        Output = output;
        Host = host;
    }
    public override void Play(params object[] args)
    {
        int key = UnityEngine.Random.Range(0, RandomBag.Count);
        float vol = UnityEngine.Random.Range(-VolumeFlux, VolumeFlux);
        float p = UnityEngine.Random.Range(MinPitch, MaxPitch);
        float d = UnityEngine.Random.Range(0, MaxDistortion);
        ClipData toPlay = Data[key];
        Action<AnopiaSourcerer> setup = (s) =>
        {
            s.Volume *= vol;
            s.Pitch = p;
            if(MaxDistortion>0)
            s.Distortion = d;
        };
        Vector3 pos = args.Length <= 0 ? Host.transform.position : (Vector3)args[0];
        AnopiaAudioCore.PlayClipAtPoint(pos, toPlay, Output, SourcePrefab, setup);
        RandomBag.RemoveAt(key);
        if (RandomBag.Count <= 0)
            RandomBag = new List<ClipData>(Data);
    }
    public override void Stop()
    {

    }
}