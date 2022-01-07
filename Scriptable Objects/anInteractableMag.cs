using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "ButtonMag", menuName = "AnopiaEngine/Button", order = 5)]
public class anInteractableMag : IanAudioMag
{
    public ClipData Enter,Down,Drag,Up,Exit;
    public override IanEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new System.NotImplementedException("Implement to anInteractable Directly");
    }
}