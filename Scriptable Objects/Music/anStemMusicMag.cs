using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StemMusicMag", menuName = "AnopiaEngine/StemMusic")]
public class anStemMusicMag : IanMusicMag
{
    public anTrackData[] Stems;
    public anSourcerer[] PlayStems(double startTime, Transform parent, bool loop = true)
    {
        List<anSourcerer> sources = new List<anSourcerer>();
        foreach (anTrackData track in Stems)
        {
            anSourcerer sourcerer = Instantiate(SourcePrefab2D, parent);
            sourcerer.SetUp(track, loop);
            sources.Add(sourcerer);
        }
        foreach (anSourcerer s in sources)
            s.PlayScheduled(startTime);
        return sources.ToArray();
    }
}