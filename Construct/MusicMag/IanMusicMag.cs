using UnityEngine;
public abstract class IanMusicMag : ScriptableObject
{
    public anTempoData Tempo;
    public anSourcerer LoopPrefab, OneShotPrefab;
}
public class Stinger
{
    public AudioClip Transition;
    public int BeatToStart;
}