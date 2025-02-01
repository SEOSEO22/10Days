using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneButtons : MonoBehaviour
{
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Button continuousButton;
    [SerializeField] private TextMeshProUGUI continuousText;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_TITLE);
    }

    private void Update()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData"))
        {
            Color color = continuousText.color;
            color.a = 1f;
            continuousText.color = color;

            continuousButton.interactable = true;
        }
        else
        {
            Color color = continuousText.color;
            color.a = 0.6f;
            continuousText.color = color;

            continuousButton.interactable = false;
        }
    }

    public void OnNewGameButtonClicked()
    {
        if (DataManager.Instance != null) DataManager.Instance.DataClear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnContinuousButtonClicked()
    {
        if (DataManager.Instance != null) DataManager.Instance.LoadData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnGameDescriptionButtonClicked()
    {
        descriptionPanel.SetActive(true);
    }

    public void OnGameSettingButtonClicked()
    {
        settingPanel.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        EditorApplication.isPlaying = false;
        // Application.Quit();
    }
}
