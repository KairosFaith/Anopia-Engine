using UnityEngine;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR")]
public class anADSRMag : IanAudioMag
{
    public AudioClip Attack, Sustain, Release;
    public void Play(AudioSource source)
    {
        source.clip = Sustain;
        source.Play();
        source.PlayOneShot(Attack);
    }
    public void Stop(AudioSource source)
    {
        source.Stop();
        source.PlayOneShot(Release);
    }
}