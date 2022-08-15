using UnityEngine;
[CreateAssetMenu(fileName = "ADSRMag", menuName = "AnopiaEngine/ADSR", order = 2)]
public class anADSRMag : ScriptableObject
{
    //sustain gain affects attack and release too
    public AudioClip Attack;
    public AudioClip Sustain;
    public AudioClip Release;
    [Range(0, 1)]
    public float Gain = 1;
}