using UnityEngine;
using UnityEngine.Audio;
public class AnopiaSourcerer : MonoBehaviour
{
    public AudioSource Source;//TODO public?
    float MasterVolume = 1;
    public float Volume
    {
        set
        {
            Source.volume = value * MasterVolume;
        }
        get => Source.volume * MasterVolume;
    }
    public float Pitch
    {
        set
        {
            Source.pitch = value;
        }
        get => Source.pitch;
    }
    AudioDistortionFilter DistortionEffect;
    public float Distortion
    {
        set
        {
            if(!DistortionEffect)
                DistortionEffect = GetComponent<AudioDistortionFilter>();
            DistortionEffect.distortionLevel = value;
        }
        get => DistortionEffect.distortionLevel;
    }
    public void SetData(ClipData data)
    {
        if(Source==null)
           Source = GetComponent<AudioSource>();
        Source.volume = MasterVolume = data.Gain;
        Source.clip = data.Clip;
    }
    public void SetOutput(AudioMixerGroup output)
    {
        if(Source == null)
           Source = GetComponent<AudioSource>();
        Source.outputAudioMixerGroup = output;
    }
    public void PanToOpposite()
    {
        StartCoroutine(AnopiaAudioCore.PanToOpposite(Source));
    }
}