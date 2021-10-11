using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SliderMag", menuName = "AnopiaEngine/Slider", order = 6)]
public class anSliderMag : IanAudioMag
{
    public ClipData Enter;
    public ClipData Down;
    public ClipData Drag;
    public ClipData Up;
    public ClipData Exit;
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException();
    }
}
