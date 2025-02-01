using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffUIButton : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    private bool isActive = false;

    public void OnButtonClickedOnOffUI()
    {
        isActive = targets[0].activeSelf;

        if (isActive)
        {
            foreach (GameObject target in targets)
            {
                target.SetActive(false);
            }

            isActive = false;
        }
        else
        {
            foreach (GameObject target in targets)
            {
                target.SetActive(true);
            }

            isActive = true;
        }

    }
}
