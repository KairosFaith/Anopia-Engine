using UnityEngine;
[CreateAssetMenu(fileName = "ClipMag", menuName = "AnopiaEngine/ClipMag")]
public class anClipMag : IanAudioMag
{
    public AudioClip[] Clips;
    [Range(0, 1)]
    public float MinRandomVolume = 1;
    public AudioClip Randomise(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Clips.Length);
        AudioClip c = Clips[key];
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