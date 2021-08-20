using System;
using UnityEngine;
public class FlexSourceHandler : MonoBehaviour
{
    public AudioSource Source;
    SourceID TemplateID;
    float MasterVolume;
    #region source object properties, add more where needed
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
    #endregion
    public void SetUp(AudioSource source, SourceID id)
    {
        Source = source;
        TemplateID = id;
    }
    public void AssignClipData(AudioFileID id, int key)
    {
        ClipBag b = FlexEngine.GetClips(id);
        ClipData d = b.Data[key];
        Source.clip = d.Clip;
        Source.volume = MasterVolume = d.Gain;
    }
    public void Play(AudioFileID id, int key)
    {
        //Source.Stop();
        AssignClipData(id, key);
        Source.Play();
    }
    public void PlayOneShot(AudioFileID id)//random
    {
        ClipBag b = FlexEngine.GetClips(id);
        AudioClip c = b.RandomClip(out float gain);
        Source.PlayOneShot(c, gain);
    }
    public void PlayOneShot(AudioFileID id, int key)
    {
        ClipBag b = FlexEngine.GetClips(id);
        ClipData d = b.Data[key];
        Source.PlayOneShot(d.Clip, d.Gain);
    }
    public void PlayClipAtPoint(AudioFileID id, Action<FlexSourceHandler> settings = null)
    {
        PlayClipAtPoint(id, transform.position, settings);
    }
    public void PlayClipAtPoint(AudioFileID id, Vector3 position, Action<FlexSourceHandler> settings = null)
    {
        ClipData c = FlexEngine.GetClips(id).RandomClip();
        FlexEngine.PlayClipAtPoint(position, c,Source.outputAudioMixerGroup, TemplateID, settings);
    }
    public FlexSourceHandler PlayClipAtStereo(AudioFileID id, int key,OutputPan type = 0)
    {
        ClipBag b = FlexEngine.GetClips(id);
        ClipData d = b.Data[key];
        return FlexEngine.PlayClipAtStereo(this, d, Source.outputAudioMixerGroup, type);
    }
    //effects for oneshot source, add more where needed
    public void SweepPan()
    {
        StartCoroutine(FlexEngine.PanToOpposite(Source));
    }
}