using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
public class ThreadSynchro : IanSynchro
{
    public const double DeltaMargin = 0.1f;//TODO still experimenting
    bool SynchroActive;
    CancellationTokenSource _Cts;
    public override void StartSynchro(double startTime)
    {
        _Cts = new CancellationTokenSource();
        _ = Synchro(startTime);
    }
    public override void StopSynchro()
    {
        SynchroActive = false;
        _Cts.Cancel();
        _Cts.Dispose();
    }
    void OnApplicationQuit()
    {
        if(SynchroActive)
            StopSynchro();
    }
    async Task Synchro(double startTime)
    {
        SynchroActive = true;
        CurrentBeatCount = 0;
        CurrentBar = NextBeat = startTime;
        while (SynchroActive)
            if ((NextBeat - AudioSettings.dspTime) <= DeltaMargin)
                Beat(NextBeat);
            else
                await Task.Delay(1, _Cts.Token);
        SynchroActive = false;
    }
    void Beat(double beatTimeCode)
    {
        if (CurrentBeatCount == Tempo.BeatsPerBar)
        {
            CurrentBeatCount = 1;
            CurrentBar = beatTimeCode;
        }
        else
            CurrentBeatCount++;
        CurrentBeat = NextBeat;
        NextBeat += Tempo.BeatLength;
        PlayOnBeat?.Invoke(CurrentBeatCount, beatTimeCode);
    }
}