using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag")]
public class anClipMag : IanAudioMag
{
    public AudioClip[] Data;
    [Range(0, 1)]
    public float MinRandomVolume = 1;
    public AudioClip Randomise(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Data.Length);
        AudioClip c = Data[key];
        gain = UnityEngine.Random.Range(MinRandomVolume, 1);
        return c;
    }
    public AudioClip PlayOneShot(AudioSource source)
    {
        AudioClip c = source.clip = Randomise(out float gain);
        source.PlayOneShot(c, gain);
        return c;
    }
}