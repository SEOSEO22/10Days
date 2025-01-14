using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHPBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float xPos = 0f;
    [SerializeField] private float yPos = .7f;
    private Slider HPBar;

    private void Start()
    {
        HPBar = GetComponent<Slider>();
        target = transform.parent.parent.gameObject;
    }

    private void Update()
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
        HPBar.value -= (damage / 100f);
    }

    private void SetHPBarLocation()
    {
        Vector3 hpTransform =
            Camera.main.WorldToScreenPoint(new Vector3(target.transform.position.x + xPos, target.transform.position.y - yPos, 0));
        HPBar.GetComponent<RectTransform>().position = hpTransform;
    }
}
