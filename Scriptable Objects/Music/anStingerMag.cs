using UnityEngine;
[CreateAssetMenu(fileName = "StingerMag", menuName = "AnopiaEngine/StingerMag")]
public class anStingerMag : IanMusicMag
{
    public StingerData[] Stingers;
    public anSourcerer PlayStinger(int index, double startTime, Transform parent)
    {
        return Stingers[index].PlayStinger(startTime, SourcePrefab2D, parent);
    }
    public void CueStinger(Transform parent, int index, BeatFunc activateOnBeat)
    {
        StingerData stinger = Stingers[index];
        void PlayOnBeat(int beatCount, double timeCode)
        {
            if (beatCount == stinger.BeatToStart)
            {
                stinger.PlayStinger(timeCode, SourcePrefab2D, parent);
                IanSynchro.PlayOnBeat -= PlayOnBeat;
                activateOnBeat?.Invoke(beatCount,timeCode);
            }
        };
        IanSynchro.PlayOnBeat += PlayOnBeat;
        //return PlayOnBeat;
        //TODO return BeatFunc ???
    }
}