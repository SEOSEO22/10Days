using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneButtons : MonoBehaviour
{
    [SerializeField] private Image descriptionImage;

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnContinuousButtonClicked()
    {

    }

    public void OnGameDescriptionButtonClicked()
    {
        descriptionImage.gameObject.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        EditorApplication.isPlaying = false;
        // Application.Quit();
    }
}
