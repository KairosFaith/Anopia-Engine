using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "SliderMag", menuName = "AnopiaEngine/Slider", order = 6)]
public class SliderMag : IAnopiaAudioMag
{
    public ClipData Enter;
    public ClipData Down;
    public ClipData Drag;
    public ClipData Up;
    public ClipData Exit;
    public override IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException();
    }
}
