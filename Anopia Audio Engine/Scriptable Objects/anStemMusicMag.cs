using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic", order = 8)]
public class anStemMusicMag : IanMusicMag
{
    public override SongForm Structure => SongForm.Stem;
    public StemData[] Stems;
    public GameObject LoopPrefab;
    public Dictionary<AudioMixerGroup, StemData> GetStems()
    {
        Dictionary<AudioMixerGroup, StemData> references = new Dictionary<AudioMixerGroup, StemData>();
        foreach (StemData s in Stems)
            references.Add(s.Channel, s);
        return references;
    }
}
[Serializable]
public class StemData
{
    public AudioMixerGroup Channel;
    public AudioClip Clip;
    [Range(-1,1)]
    public float Pan;
}