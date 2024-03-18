using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipObjectMag", menuName = "AnopiaEngine/ClipObjectMag", order = 2)]
public class anClipObjectMag : anClipMag
{
    public anSourcerer SourcererPrefab;
    public float MinPitch = 1, MaxPitch =1;//TODO use MinMax Slider
    public bool UseDistortion, UseHighPass;
    public float MaxDistortion, MaxHighPass = 10;
    anSourcerer InstantiateSourcerer(Vector3 position, AudioMixerGroup channel)
    {
        anSourcerer a = Instantiate(SourcererPrefab, position, Quaternion.identity);
        a.SetChannel(channel);
        a.SetRandomPitch(MinPitch, MaxPitch);
        if (UseDistortion)
            a.audioDistortionFilter.distortionLevel = UnityEngine.Random.Range(0, MaxDistortion);
        if (UseHighPass)
            a.audioHighPassFilter.cutoffFrequency = UnityEngine.Random.Range(10, MaxHighPass);
        return a;
    }
    public anSourcerer PlayClipAtPoint(Vector3 position, AudioMixerGroup channel)
    {
        anSourcerer a = InstantiateSourcerer(position, channel);
        AudioClip c = PlayOneShot(a.audioSource);
        Destroy(a.gameObject, c.length);
        return a;
    }
    public anSourcerer PlayClipAtPointScheduled(Vector3 position, double timecode, AudioMixerGroup channel)
    {
        anSourcerer a = InstantiateSourcerer(position, channel);
        AudioClip clip = Randomise(out float gain);
        a.PlayScheduled(timecode, gain);
        a.DeleteAfterTime(timecode + clip.length);
        return a;
    }
}