using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    [SerializeField] GameObject timer;

    public void SetDay()
    {
        timer.GetComponent<Image>().fillAmount = 0.99f;
    }
}
