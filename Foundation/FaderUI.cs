using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class FaderUI : MonoBehaviour
{
    public AudioMixer Mixer;
    public string ParamName;
    Slider _slider;
    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = 1;
        _slider.minValue = .0001f;
        _slider.onValueChanged.AddListener(SetVol);
    }
    private void OnEnable()
    {
        Mixer.GetFloat(ParamName, out float faderValue);
        _slider.value = Mathf.Pow(10, faderValue / 20);
    }
    public void SetVol(float sliderValue)
    {
        Mixer.SetFloat(ParamName, Mathf.Log10(sliderValue) * 20);
    }
}