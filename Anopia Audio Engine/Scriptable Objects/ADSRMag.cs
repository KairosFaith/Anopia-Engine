using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 3)]
public class ADSRMag : IAnopiaAudioMag
{
    //sustain gain affects attack and release too
    public GameObject SourcePrefab;
    public ClipData Attack;
    public ClipData Sustain;
    public ClipData Release;
    public override IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        return new AnopiaADSREvent(host, this, output);
    }
}
public class AnopiaADSREvent : IAnopiaEvent
{
    public ClipData Attack;
    public ClipData Release;
    public AnopiaSourcerer Sourcerer;
    public AnopiaADSREvent(MonoBehaviour host, IAnopiaAudioMag mag, AudioMixerGroup output) : base(host, mag, output)
    {
        ADSRMag Mag = (ADSRMag)mag;
        Attack = Mag.Attack;
        Release = Mag.Release;
        Sourcerer = AnopiaAudioCore.NewPointSource(host, Mag.SourcePrefab, output);
        Sourcerer.SetData(Mag.Sustain);
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
        s.PlayOneShot(Release);
        s.Stop();
    }
}