using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag", order = 1)]
public class anClipMag : IanAudioMag
{
    public AudioClip[] Data;
    [Range(0, 1)]
    public float MinRandomVolume = 1;
    public AudioClip Randomise(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Data.Length);
        AudioClip c = Data[key];
        gain = UnityEngine.Random.Range(-MinRandomVolume, 1);
        return c;
    }
}