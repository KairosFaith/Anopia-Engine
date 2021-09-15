using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public static class DataCore
{
    public static AudioMixer MainMixer;
    public static AudioMixer SfxMixer;
    public static AudioMixerGroup PlayerChannel;
    public static AudioMixerGroup EnemyChannel;
    public static AudioMixerGroup EnemyCueChannel;
    public static AudioMixerGroup NonDiegetic;
    public static LayerMask EnemyLayerMask;
    public static LayerMask GroundLayer;
    static DataCore()
    {
        DataCentral central = Resources.Load<DataCentral>("DataCore/DataCentral");
        MainMixer = central.MainMixer;
        AnopiaMusicController.Mixer = central.MusicMixer;
        AnopiaMusicController.Channels = central.MusicChannels;
        EnemyLayerMask = central.EnemyLayerMask;
        PlayerChannel = central.PlayerChannel;
        EnemyChannel = central.EnemyChannel;
        EnemyCueChannel = central.EnemyCueChannel;
        NonDiegetic = central.NonDiegetic;
        SfxMixer = central.SfxMixer;
        GroundLayer = central.GroundLayer;
    }
}