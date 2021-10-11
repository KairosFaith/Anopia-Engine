using System;
using UnityEngine;
[CreateAssetMenu(fileName = "LinearMusicMag", menuName = "AnopiaEngine/LinearMusic", order = 7)]
public class anLinearMusicMag : IanMusicMag
{
    public AudioClip Intro,MainSection;
    public SongSection[] Sections;
    public override SongForm Structure =>SongForm.Linear;
}
[Serializable]
public class SongSection
{
    public AudioClip Stinger, Loop;//stinger should be 1 bar max
    public int BeatTostart;
}