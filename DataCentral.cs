using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "DataCentral", menuName = "DataCentral", order = 3)]
public class DataCentral : ScriptableObject
{
    public AudioMixer MainMixer;
    public AudioMixer SfxMixer;
    public AudioMixer MusicMixer;
    public AudioMixerGroup[] MusicChannels;
    public AudioMixerGroup PlayerChannel;
    public AudioMixerGroup EnemyChannel;
    public AudioMixerGroup EnemyCueChannel;
    public AudioMixerGroup NonDiegetic;
    public LayerMask EnemyLayerMask;
    public LayerMask GroundLayer;
}
