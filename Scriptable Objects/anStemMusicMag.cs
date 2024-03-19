using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic")]
public class anStemMusicMag : IanMusicMag
{
    public override SongForm Structure => SongForm.Stem;
    public StemData[] Stems;
    public AudioClip Impact;
    //TODO leave this in for now???
    //Fade curves???
    public anSourcerer[] Setup()
    {
        List<anSourcerer> SourceHandlers = new List<anSourcerer>();
        foreach(StemData s in Stems)
        {
            anSourcerer a = anCore.Setup2DLoopSource(s.Clip, s.Channel);
            a.audioSource.panStereo = s.Pan;
            SourceHandlers.Add(a);
        }
        return SourceHandlers.ToArray();
    }
    public anSourcerer[] Play(double startTime, AudioMixerGroup impactChannel)
    {
        anSourcerer[] SourceHandlers = Setup();
        foreach (anSourcerer s in SourceHandlers)
            s.audioSource.PlayScheduled(startTime);
        if (Impact != null)
        {
            anSourcerer i = anCore.Setup2DSource(Impact, impactChannel);
            i.audioSource.PlayScheduled(startTime);
        }
        return SourceHandlers;
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