using UnityEngine;
using UnityEngine.UI;
public class anMenuNarrator : MonoBehaviour
{
    public static anMenuNarrator Instance;
    public Selectable SelectOnStart;
    public string SoundID;
    public anSpeechMag SpeechMag;
    public AudioSource Source;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SelectOnStart.Select();
    }
    public void NarrateMessage(string msg)
    {
        SpeechMag.PlaySpeech(Source, msg);
    }
}