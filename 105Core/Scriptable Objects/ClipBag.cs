using UnityEngine;
using System;
[CreateAssetMenu(fileName = "ClipBag", menuName = "FlexEngine/ClipBag", order = 1)]
public class ClipBag : ScriptableObject
{
    public AudioFileID ID 
    {
        get
        {
            if (Enum.TryParse(name, out AudioFileID id))
                return id;
            else
                throw new Exception(name +" invalid AudioFileID");
        }
    }
    public ClipData[] Data;
    public AudioClip RandomClip(out float Gain)
    {
        ClipData d = RandomClip();
        Gain = d.Gain;
        return d.Clip;
    }
    public ClipData RandomClip()
    {
        int key = UnityEngine.Random.Range(0, Data.Length);
        return Data[key];
    }
}
[Serializable]
public class ClipData
{
    public AudioClip Clip;
    [Range(0, 1)]
    public float Gain = 1;
}