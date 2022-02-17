using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 3)]
public class anADSRMag : IanAudioMag
{
    //sustain gain affects attack and release too
    public GameObject SourcePrefab;
    public AudioClip Attack;
    public AudioClip Sustain;
    public AudioClip Release;
    [Range(0, 1)]
    public float Gain = 1;
    public override IanEvent LoadMag(MonoBehaviour driver, AudioMixerGroup output)
    {
        return new anADSREvent(driver, this, output);
    }
}
public class anADSREvent : IanEvent
{
    public AudioClip Attack;
    public AudioClip Release;
    public anSourcerer Sourcerer;
    public anADSREvent(MonoBehaviour driver, IanAudioMag mag, AudioMixerGroup output) : base(driver, mag, output)
    {
        anADSRMag Mag = (anADSRMag)mag;
        Attack = Mag.Attack;
        Release = Mag.Release;
        GameObject newg = UnityEngine.Object.Instantiate(Mag.SourcePrefab, driver.transform);
        Sourcerer = newg.GetComponent<anSourcerer>();
        AudioSource a = Sourcerer.audioSource;
        a.clip = Mag.Sustain;
        a.volume = Mag.Gain;
        a.loop = true;
    }
    public override void Play(params object[] args)
    {
        AudioSource s = Sourcerer.audioSource;
        s.PlayOneShot(Attack);
        s.Play();
    }
    public override void Stop()
    {
        AudioSource s = Sourcerer.audioSource;
        s.Stop();
        s.PlayOneShot(Release);
    }
    public override void PlayScheduled(double timecode, params object[] args)
    {
        throw new System.NotImplementedException();
    }

    public override void SetParameter(string name, float value, params object[] args)
    {

    }
}