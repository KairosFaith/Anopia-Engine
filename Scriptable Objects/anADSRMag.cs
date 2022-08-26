using UnityEngine;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 3)]
public class anADSRMag : IanAudioMag
{
    public AudioClip Attack;
    public AudioClip Sustain;
    public AudioClip Release;
}