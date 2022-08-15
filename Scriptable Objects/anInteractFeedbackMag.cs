using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "UIMag", menuName = "AnopiaEngine/UI Feedback", order = 6)]
public class anInteractFeedbackMag : IanAudioMag
{
    public anClipData Enter,Down,Interact,Up,Exit;
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        throw new System.NotImplementedException("Implement to anInteractable Directly");
    }
}