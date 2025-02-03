using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHPBar : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] Vector3 offset = new Vector3(0f, -0.7f, 0f);
    [SerializeField] float maxHP = 30f;
    [SerializeField] private float invincibleTime = 1.5f;

    private Slider HPBar;
    private bool isDamaging = false;

    private void Awake()
    {
        HPBar = GetComponent<Slider>();
        target = transform.parent.parent.gameObject;
    }

    private void OnEnable()
    {
        Color color = target.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        target.GetComponent<SpriteRenderer>().color = color;
        isDamaging = false;

        SetHPFull();
    }

    private void LateUpdate()
    {
        SetHPBarLocation();
    }
    public void SetHPFull()
    {
        HPBar.value = 1f;
    }

    public void Damaged(float damage)
    {
        if (target.CompareTag("Build Item") && isDamaging) return;
        if (target.activeSelf == false) return;

        isDamaging = true;

        HPBar.value = Mathf.Clamp(HPBar.value - (damage / maxHP), 0f, 1f);
        if (HPBar.value <= 0.01f)
        {
            target.GetComponent<Harvest>().HarvestObject(target);
            return;
        }

        StopCoroutine(SetDamageColor());
        StartCoroutine(SetDamageColor());

        isDamaging = false;
    }

    private void SetHPBarLocation()
    {
        Vector3 hpTransform =
            Camera.main.WorldToScreenPoint(target.transform.position + offset);
        HPBar.GetComponent<RectTransform>().position = hpTransform;
    }

    private IEnumerator SetDamageColor()
    {
        Color color = target.GetComponent<SpriteRenderer>().color;

        // 피격 시 투명도 감소
        color.a = .4f;
        target.GetComponent<SpriteRenderer>().color = color;

        if (target.CompareTag("Build Item"))
        {
            // 무적 시간 동안 대기
            yield return new WaitForSeconds(invincibleTime);
        }
        else
        {
            yield return new WaitForSeconds(.5f);
        }

        // 투명도 원상복구
        color.a = 1f;
        target.GetComponent<SpriteRenderer>().color = color;
    }
}
