using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] GameObject target;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            target.SetActive(!target.activeSelf);
        }
    }
}
