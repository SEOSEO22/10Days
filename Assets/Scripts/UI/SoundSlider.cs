using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private Slider BGMSoundSlider;
    [SerializeField] private Slider SFXSoundSlider;

    private void Awake()
    {
        BGMSoundSlider.onValueChanged.AddListener(ChangeBGMVolume);
        SFXSoundSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void Start()
    {
        BGMSoundSlider.value = DataManager.Instance.currentSoundData.bgmVolume;
        SFXSoundSlider.value = DataManager.Instance.currentSoundData.sfxVolume;
    }

    void ChangeBGMVolume(float value)
    {
        SoundManager.Instance.ChangeBGMVolume(value);

        DataManager.Instance.currentSoundData.bgmVolume = value;
        DataManager.Instance.SaveSoundData();
    }

    void ChangeSFXVolume(float value)
    {
        SoundManager.Instance.ChangeSFXVolume(value);

        DataManager.Instance.currentSoundData.sfxVolume = value;
        DataManager.Instance.SaveSoundData();
    }
}
