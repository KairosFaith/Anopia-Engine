using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ClipObjectMag", menuName = "AnopiaEngine/ClipObjectMag")]
public class anClipObjectMag : anClipMag
{
    public anSourcerer SourcePrefab3D;
    [Range(.1f, 1f)]
    public float MinPitch = 1;
    [Range(1f, 2f)]
    public float MaxPitch = 1;
    [HideInInspector]
    public bool UseDistortion, UseHighPass;
    [HideInInspector]
    public float MaxDistortion, MaxHighPass = 10;
    anSourcerer InstantiateSourcerer(Vector3 position, AudioMixerGroup channel)
    {
        anSourcerer a = Instantiate(SourcePrefab3D, position, Quaternion.identity);
        a.Channel = channel;
        a.SetRandomPitch(MinPitch, MaxPitch);
        if (UseDistortion)
            a.Distortion = UnityEngine.Random.Range(0, MaxDistortion);
        if (UseHighPass)
            a.HighPass = UnityEngine.Random.Range(10, MaxHighPass);
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
        a.audioSource.clip = clip;
        a.PlayScheduled(timecode, gain);
        a.DeleteAfterTime(timecode + clip.length);
        return a;
    }
}