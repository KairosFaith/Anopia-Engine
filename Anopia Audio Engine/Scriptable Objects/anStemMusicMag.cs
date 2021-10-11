using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic", order = 8)]
public class anStemMusicMag : IanMusicMag
{
    public override SongForm Structure => SongForm.Stem;
    public StemData[] Stems;
    public Dictionary<AudioMixerGroup,AudioClip> GetStems()
    {
        Dictionary<AudioMixerGroup, AudioClip> references = new Dictionary<AudioMixerGroup, AudioClip>();
        foreach (StemData s in Stems)
            references.Add(s.Channel, s.Clip);
        return references;
    }
}
[Serializable]
public class StemData
{
    public AudioMixerGroup Channel;
    public AudioClip Clip;
}