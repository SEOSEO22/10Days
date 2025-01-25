using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffUIButton : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;

    private void Awake()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(false);
        }
    }

    public void OnButtonClickedOnOffUI()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(!target.activeSelf);
        }
    }
}
