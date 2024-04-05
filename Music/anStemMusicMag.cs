using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic")]
public class anStemMusicMag : IanMusicMag
{
    public TrackData[] Stems;
    public anSourcerer[] Setup()
    {
        List<anSourcerer> SourceHandlers = new List<anSourcerer>();
        foreach(TrackData s in Stems)
        {
            anSourcerer a = anCore.SetupTrackSource(s);
            SourceHandlers.Add(a);
        }
        return SourceHandlers.ToArray();
    }
    public anSourcerer[] Play(double startTime)
    {
        anSourcerer[] SourceHandlers = Setup();
        foreach (anSourcerer s in SourceHandlers)
            s.PlayScheduled(startTime);
        return SourceHandlers;
    }
}