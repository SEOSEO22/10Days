using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowCostItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityTxt;

    public Image ItemImage => itemImage;
    public TMP_Text QuantityText => quantityTxt;

    public void SetData(Sprite sprite, int currentQuantity, int needQuantity)
    {
        itemImage.sprite = sprite;
        quantityTxt.text = currentQuantity + " / " + needQuantity;
    }
}
