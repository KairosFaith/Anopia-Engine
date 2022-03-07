using UnityEngine;
using UnityEngine.UI;
public class anMenuNarrator : MonoBehaviour
{
    public static anMenuNarrator Instance;
    public Selectable SelectOnStart;
    public string SoundID;
    public anDriver Driver;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SelectOnStart.Select();
        Driver.SetDriver(SoundID);
    }
    public void NarrateMessage(string msg)
    {
        Driver.Play(SoundID, msg);
    }
}