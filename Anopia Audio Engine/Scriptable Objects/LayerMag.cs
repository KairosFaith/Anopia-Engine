using System;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "LayerMag", menuName = "AnopiaEngine/LayeredLoops", order = 4)]
public class LayerMag : IAnopiaAudioMag
{
    public ClipLayer[] Layers;
    public override IAnopiaEvent LoadMag(MonoBehaviour host, AudioMixerGroup output)
    {
        throw new NotImplementedException("LayerEvent Coming Soon....");
    }
}
[Serializable] 
public class ClipLayer
{
    public ClipData Data;
    public GameObject SourcePrefab;
}