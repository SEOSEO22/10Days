using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHPBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] Vector3 offset = new Vector3(0f, -0.7f, 0f);
    [SerializeField] float maxHP = 30f;
    private Slider HPBar;

    private void Awake()
    {
        HPBar = GetComponent<Slider>();
        target = transform.parent.parent.gameObject;
    }

    private void OnEnable()
    {
        SetHPFull();
    }

    private void LateUpdate()
    {
        SetHPBarLocation();
    }

    public float GetHP()
    {
        return HPBar.value;
    }

    public void SetHPFull()
    {
        HPBar.value = 1f;
    }

    public void Damaged(float damage)
    {
        HPBar.value = Mathf.Clamp(HPBar.value - (damage / maxHP), 0f, 1f);
    }

    private void SetHPBarLocation()
    {
        Vector3 hpTransform =
            Camera.main.WorldToScreenPoint(target.transform.position + offset);
        HPBar.GetComponent<RectTransform>().position = hpTransform;
    }
}
