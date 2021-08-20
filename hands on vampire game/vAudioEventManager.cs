using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class vAudioEventManager : SingletonMonobehavior<vAudioEventManager>
{
    public AudioMixer SoundEffectMixer;
    public AudioMixerGroup PlayerChannel;
    public AudioMixerGroup EnemyChannel;
    public AudioMixerGroup AmbienceChannel;
    public AudioMixerGroup UiChannel;
    protected override void Awake()
    {
        base.Awake();
        Core.SubscribeEvent("ListenSkillOn", ListenSkillOn);
        Core.SubscribeEvent("ListenSkillOff", ListenSkillOff);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.UnsubscribeEvent("ListenSkillOn", ListenSkillOn);
        Core.UnsubscribeEvent("ListenSkillOff", ListenSkillOff);
    }
    void ListenSkillOn(object sender, object[] args)
    {
        SoundEffectMixer.TransitionToSnapshot("ListenSkillOn", (float)args[0]);
    }
    void ListenSkillOff(object sender, object[] args)
    {
        SoundEffectMixer.TransitionToSnapshot("ListenSkillOff", (float)args[0]);
    }
}