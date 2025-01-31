using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI survivedDayText;

    private void Awake()
    {
        if (survivedDayText != null && DataManager.Instance != null)
        {
            int dayCount = (DataManager.Instance.currentGameData.dayCountData.dayCount / 2) + 1;
            if (dayCount > 10) dayCount = 10;

            survivedDayText.text = "생존 일수 : " + dayCount.ToString("D2");
        }
        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);
        if (DataManager.Instance != null) DataManager.Instance.DataClear();
    }

    public void OnBackToStartSceneButtonClicked()
    {
        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);
        if (DataManager.Instance != null) DataManager.Instance.DataClear();

        SceneManager.LoadScene(0);
    }
}
