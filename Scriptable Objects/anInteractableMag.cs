using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ButtonMag", menuName = "AnopiaEngine/Button", order = 5)]
public class anInteractableMag : IanAudioMag
{
    public ClipData Enter,Down,Interact,Up,Exit;
    public override IanEvent LoadMag(anDriver driver, AudioMixerGroup output)
    {
        throw new System.NotImplementedException("Implement to anInteractable Directly");
    }
}