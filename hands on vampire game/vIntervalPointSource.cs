using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vIntervalPointSource : MonoBehaviour
{
    public AudioFileID Sound;
    FlexSourceHandler Source;
    public float MinTime, MaxTime;
    float countdown;
    void Start()
    {
        Source = FlexEngine.NewHandler(this, OutputPan.Point, vAudioEventManager.Instance.AmbienceChannel, SourceID.DefaultSoundObject);
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            Source.PlayClipAtPoint(Sound);
            countdown = Random.Range(MinTime, MaxTime);
        }
    }
}
