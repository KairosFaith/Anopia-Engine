using UnityEngine;
public class vUIManager : MonoBehaviour
{
    public AudioFileID MenuSounds;
    public AudioFileID ButtonSounds;
    public FlexSourceHandler UiSource;
    // Start is called before the first frame update
    void Start()
    {
        UiSource = FlexEngine.NewHandler(this, OutputPan.StereoR, vAudioEventManager.Instance.UiChannel);
        UiSource.Source.ignoreListenerPause = true;
    }
    public void Pause()
    {
        AudioListener.pause = true;
        Time.timeScale = 0;
        FlexSourceHandler h = UiSource.PlayClipAtStereo(MenuSounds, 0,OutputPan.StereoR);
        h.SweepPan();
    }
    public void Resume()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
        FlexSourceHandler h = UiSource.PlayClipAtStereo(MenuSounds, 1, OutputPan.StereoL);
        h.SweepPan();
    }
    public void ButtonFunc()
    {
        UiSource.PlayOneShot(ButtonSounds);
    }
}
