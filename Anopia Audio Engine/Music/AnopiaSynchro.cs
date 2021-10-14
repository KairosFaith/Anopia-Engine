using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public delegate void BeatFunc(int beatCount, double timeCode);
public delegate void BarFunc(double timeCode);
public static class AnopiaSynchro//this is your new update engine
{
    public const string BeatEvent = "OnBeat";
    public const string BarEvent = "OnBar";
    public const string OnSynchroStop = "OnSynchroStop";
    public static BeatFunc PlayOnBeat;
    public static BarFunc PlayOnBar;// to schedule sounds
    public static TempoData Tempo;
    static bool isPlaying;
    static double StartTime;
    public static int CurrentBeatCount => _BeatCount;
    public static int NextBeatCount => scheduleBeat;
    public static double NextBeat => _NextBeat;
    public static double NextBar => _NextBar;
    static int scheduleBeat;
    static int _BeatCount = 0;
    static double _NextBeat,_NextBar;
    // to schedule sounds
    static MonoBehaviour SynchroHost;
    public static void StartSynchro(MonoBehaviour host, TempoData tempo, Action<double>OnStart = null)
    {
        isPlaying = true;
        Tempo = tempo;
        StartTime = AudioSettings.dspTime + Time.deltaTime;//start next frame
        host.StartCoroutine(Synchro(StartTime));
        OnStart?.Invoke(StartTime);
        SynchroHost = host;
    }
    public static void StopSynchro(MonoBehaviour host)
    {
        host.StopAllCoroutines();
        isPlaying = false;
        scheduleBeat = 0;
        _BeatCount = 0;
    }
    static IEnumerator Synchro(double startTime)
    {
        _BeatCount = 1;
        scheduleBeat =1;
        PlayOnBeat?.Invoke(scheduleBeat, startTime);
        PlayOnBar?.Invoke(startTime);
        Core.BroadcastEvent(BeatEvent, SynchroHost, _BeatCount);
        Core.BroadcastEvent(BarEvent, SynchroHost);
        _NextBeat = startTime + Tempo.BeatLength;
        _NextBar = startTime + Tempo.BarLength;
        scheduleBeat++;
        PlayOnBeat?.Invoke(scheduleBeat, _NextBeat);
        PlayOnBar?.Invoke(_NextBar);
        void Beat()
        {
            _BeatCount++;
            if(_BeatCount> Tempo.BeatsPerBar)
            {
                _BeatCount = 1;
                Core.BroadcastEvent(BarEvent, SynchroHost);
            }
            Core.BroadcastEvent(BeatEvent, SynchroHost, _BeatCount);
            _NextBeat += Tempo.BeatLength;
            scheduleBeat++;
            if (scheduleBeat > Tempo.BeatsPerBar)
            {
                _NextBar += Tempo.BarLength;
                PlayOnBar?.Invoke(_NextBar);
                scheduleBeat = 1;
            }
            PlayOnBeat?.Invoke(scheduleBeat, _NextBeat);
        };
        while (isPlaying)
        {
            yield return new WaitForEndOfFrame();
            if (AudioSettings.dspTime > _NextBeat)
                Beat();
        }
        Core.BroadcastEvent(OnSynchroStop, SynchroHost);
        _BeatCount = 0;
        scheduleBeat = 0;
    }
}
public enum BarValue
{
    Quarter = 1,
    Eight,
    Sixteen = 4,
}
[Serializable]
public class TempoData
{
    public int CrotchetBPM;
    public int BeatsPerBar;
    public BarValue TimeSignature;
    public int BPM => CrotchetBPM * (int)TimeSignature;
    public float BeatLength => 60 / (float)BPM;
    public float BarLength => BeatLength * BeatsPerBar;
}
public abstract class IanMusicMag : IanAudioMag
{
    public TempoData Tempo;
    public abstract SongForm Structure { get; }
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException();
    }
}
public enum SongForm
{
    Linear,
    Stem
}
public abstract class IanSong : MonoBehaviour
{
    public abstract void StopCue(double stopTime);
    public abstract void StopImmediate();
    public abstract void Play(double startTime);
    public abstract void FadeIn(float t);
    public abstract void FadeOut(float t, Action ondone = null);
    //WARNING Pausing and unpausing will mess up the Synchro!!!!
    public abstract void Pause();
    public abstract void UnPause();
    public abstract void Mute(bool toMute);
}