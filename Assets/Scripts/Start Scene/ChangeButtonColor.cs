using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

public class ChangeButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI backgroundText;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (text != null)
        {
            text.color = Color.white;
            backgroundText.color = Color.black;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (text != null)
        {
            text.color = Color.black;
            backgroundText.color = Color.white;
        }
    }
}
