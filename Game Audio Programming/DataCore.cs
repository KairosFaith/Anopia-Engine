using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public static class DataCore
{
    public static AudioMixer MainMixer;
    public static AudioMixerGroup PlayerChannel;
    public static AudioMixerGroup EnemyChannel;
    public static LayerMask EnemyLayerMask;
    static DataCore()
    {
        DataCentral central = Resources.Load<DataCentral>("DataCore/DataCentral");
        MainMixer = central.MainMixer;
        AnopiaMusicController.Mixer = central.MusicMixer;
        AnopiaMusicController.Channels = central.MusicChannels;
        EnemyLayerMask = central.EnemyLayerMask;
    }
}