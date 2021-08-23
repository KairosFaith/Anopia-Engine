using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ButtonMag", menuName = "AnopiaEngine/Button", order = 5)]
public class ButtonMag : IAnopiaAudioMag
{
    public ClipData Enter;
    public ClipData Down;
    public ClipData Exit;
    public override IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException();
    }
}