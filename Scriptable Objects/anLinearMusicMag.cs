using System;
using UnityEngine;
[CreateAssetMenu(fileName = "LinearMusicMag", menuName = "AnopiaEngine/LinearMusic", order = 7)]
public class anLinearMusicMag : IanMusicMag
{
    public AudioClip Intro,MainSection;
    public SongSection[] Sections;
    public SongSection Final;
    public GameObject LoopPrefab, OneShotPrefab;
    public override SongForm Structure =>SongForm.Linear;
}
[Serializable]
public class SongSection
{
    public AudioClip Stinger, Loop;
    //stingers should be at most 1 bar before new section
    public int BeatTostart;
}