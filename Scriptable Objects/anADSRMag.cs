using UnityEngine;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 3)]
public class anADSRMag : IanAudioMag
{
    public AudioClip Attack;
    public AudioClip Sustain;
    public AudioClip Release;
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