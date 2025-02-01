using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //BGM ������
    public enum EBgm
    {
        BGM_TITLE,
        BGM_DAY,
        BGM_NIGHT,
        BGM_DEAD,
        BGM_ESCAPE
    }

    //SFX ������
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

    //audio clip ���� �� �ִ� �迭
    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    //�÷����ϴ� AudioSource
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

    // EBgm �������� �Ű������� �޾� �ش��ϴ� ��� ���� Ŭ���� ���
    public void PlayBGM(EBgm bgmIdx)
    {
        //enum int������ ����ȯ ����
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    // ���� ��� ���� ��� ���� ����
    public void StopBGM()
    {
        audioBgm.Stop();
    }

    // ESfx �������� �Ű������� �޾� �ش��ϴ� ȿ���� Ŭ���� ���
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
