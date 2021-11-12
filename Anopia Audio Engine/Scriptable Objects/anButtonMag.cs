using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ButtonMag", menuName = "AnopiaEngine/Button", order = 5)]
public class anButtonMag : IanAudioMag
{
    public ClipData Enter;
    public ClipData Down;
    public ClipData Exit;
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException("Implement to anButton Directly");
    }
}