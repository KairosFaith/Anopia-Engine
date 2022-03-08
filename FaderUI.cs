using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class FaderUI : MonoBehaviour
{
    public AudioMixer Mixer;
    public string ParamName;
    public Slider slider;
    void Awake()
    {
        slider.maxValue = 1;
        slider.minValue = .0001f;
        slider.onValueChanged.AddListener(SetVol);
    }
    private void OnEnable()
    {
        Mixer.GetFloat(ParamName, out float faderValue);
        slider.value = Mathf.Pow(10, faderValue / 20);
    }
    public void SetVol(float sliderValue)
    {
        Mixer.SetFloat(ParamName, Mathf.Log10(sliderValue) * 20);
    }
}