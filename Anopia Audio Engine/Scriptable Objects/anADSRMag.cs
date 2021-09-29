using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 3)]
public class anADSRMag : IanAudioMag
{
    //sustain gain affects attack and release too
    public GameObject SourcePrefab;
    public ClipData Attack;
    public ClipData Sustain;
    public ClipData Release;
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        return new anADSREvent(host, this, output);
    }
}
public class anADSREvent : IanEvent
{
    public ClipData Attack;
    public ClipData Release;
    public AnopiaSourcerer Sourcerer;
    public anADSREvent(MonoBehaviour host, IanAudioMag mag, AudioMixerGroup output) : base(host, mag, output)
    {
        anADSRMag Mag = (anADSRMag)mag;
        Attack = Mag.Attack;
        Release = Mag.Release;
        Sourcerer = AnopiaAudioCore.NewPointSource(host, Mag.SourcePrefab, output);
        Sourcerer.SetData(Mag.Sustain);
        Sourcerer.Source.loop = true;
    }
    public override void Play(params object[] args)
    {
        AudioSource s = Sourcerer.Source;
        s.PlayOneShot(Attack);
        s.Play();
    }
    public override void Stop()
    {
        AudioSource s = Sourcerer.Source;
        s.Stop();
        s.PlayOneShot(Release);
    }
}