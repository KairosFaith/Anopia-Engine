using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class tAudioEventManager : SingletonMonobehavior<tAudioEventManager>
{
    public AudioMixerGroup PlayerChannel;
    public AnopiaMusicController MainMusicController;
    public float MusicSnapshotTime;
    protected override void Awake()
    {
        base.Awake();
        Core.SubscribeEvent("Checkpoint", Checkpoint);
        Core.SubscribeEvent("FinalEncounter", FinalEncounter);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Core.UnsubscribeEvent("Checkpoint", Checkpoint);
        Core.UnsubscribeEvent("FinalEncounter", FinalEncounter);
    }
    void Checkpoint(object sender, object[] args)
    {
        MainMusicController.SetSnapshot((string)args[0], MusicSnapshotTime); 
    }
    void FinalEncounter(object sender, object[] args)
    {
        MainMusicController.ChangeMusic("FinalEncounter");
        MainMusicController.SetSnapshot("Boss Area", MusicSnapshotTime);
    }
    void Start()
    {
        MainMusicController.SetSnapshot("Starting Area", 0);
        MainMusicController.ChangeMusic("Starting Area");
    }
}