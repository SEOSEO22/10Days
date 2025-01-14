using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCode : MonoBehaviour
{
    [SerializeField] GameObject timer;
    [SerializeField] GameObject hpBar;

    public void SetDay()
    {
        timer.transform.Find("Timer Image").GetComponent<Image>().fillAmount = 0.99f;
    }

    public void SetObjectDamaged()
    {
        hpBar.GetComponent<ObjectHPBar>().Damaged(10);
    }
}
