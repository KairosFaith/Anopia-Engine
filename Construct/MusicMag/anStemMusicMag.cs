using System;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic", order = 5)]
public class anStemMusicMag : IanMusicMag
{
    public Stinger Intro;
    public StemData[] Stems;
}
[Serializable]
public class StemData
{
    public AudioMixerGroup Channel;
    public AudioClip Clip;
    [Range(-1,1)]
    public float Pan;
}