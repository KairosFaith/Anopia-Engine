using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AnopiaSourcerer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioDistortionFilter audioDistortionFilter;
    public AudioHighPassFilter audioHighPassFilter;
}