using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "UIMag", menuName = "AnopiaEngine/UI Feedback", order = 6)]
public class anInteractFeedbackMag : IanAudioMag
{
    public AudioClip Enter,Down,Interact,Up,Exit;
}