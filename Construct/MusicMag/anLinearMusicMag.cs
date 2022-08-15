using System;
using UnityEngine;
[CreateAssetMenu(fileName = "LinearMusicMag", menuName = "AnopiaEngine/LinearMusic", order = 4)]
public class anLinearMusicMag : IanMusicMag
{
    public SongSection Intro;
    public SongSection[] Sections;
    public SongSection Final;
}
[Serializable]
public class SongSection : Stinger
{
    public AudioClip Section;
}