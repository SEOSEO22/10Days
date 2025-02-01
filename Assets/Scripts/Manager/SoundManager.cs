using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //BGM 종류들
    public enum EBgm
    {
        BGM_TITLE,
        BGM_DAY,
        BGM_NIGHT,
        BGM_DEAD,
        BGM_ESCAPE
    }

    //SFX 종류들
    public enum ESfx
    {
        SFX_PLAYER,
        SFX_UI,
        SFX_PICKUP,
        SFX_GRASS,
        SFX_WOOD,
        SFX_IRON,
        SFX_CLOCK
    }

    //audio clip 담을 수 있는 배열
    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    //플레이하는 AudioSource
    AudioSource audioBgm;
    AudioSource audioSfx;

    void Awake()
    {
        #region Singleton
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        audioBgm = GameObject.Find("BGM Player").GetComponent<AudioSource>();
        audioSfx = GameObject.Find("Sfx Player").GetComponent<AudioSource>();

        if (DataManager.Instance.IsSoundFileExist())
            DataManager.Instance.LoadSoundData();
    }

    private void Start()
    {
        audioBgm.volume = DataManager.Instance.currentSoundData.bgmVolume;
        audioSfx.volume = DataManager.Instance.currentSoundData.sfxVolume;
    }

    // EBgm 열거형을 매개변수로 받아 해당하는 배경 음악 클립을 재생
    public void PlayBGM(EBgm bgmIdx)
    {
        //enum int형으로 형변환 가능
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    // 현재 재생 중인 배경 음악 정지
    public void StopBGM()
    {
        audioBgm.Stop();
    }

    // ESfx 열거형을 매개변수로 받아 해당하는 효과음 클립을 재생
    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }

    public void ChangeBGMVolume(float value)
    {
        audioBgm.volume = value;
    }

    public void ChangeSFXVolume(float value)
    {
        audioSfx.volume = value;
    }
}
