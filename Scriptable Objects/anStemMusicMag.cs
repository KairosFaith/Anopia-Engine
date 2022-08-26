using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic", order = 8)]
public class anStemMusicMag : IanMusicMag
{
    public override SongForm Structure => SongForm.Stem;
    public StemData[] Stems;
    public anSourcerer LoopPrefab;
    //Add Impact crash on start
    public AudioClip Impact;
    public anSourcerer OneShotPrefab;

}
[Serializable]
public class StemData
{
    public AudioMixerGroup Channel;
    public AudioClip Clip;
    [Range(-1,1)]
    public float Pan;
}