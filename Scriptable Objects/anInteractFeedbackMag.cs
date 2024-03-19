using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "UIMag", menuName = "AnopiaEngine/UI Feedback")]
public class anInteractFeedbackMag : IanAudioMag
{
    public AudioClip Enter,Down,Interact,Up,Exit;
}