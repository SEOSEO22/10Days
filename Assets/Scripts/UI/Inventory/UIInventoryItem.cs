using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityTxt;
    [SerializeField] private Image borderImage;

    public event Action<UIInventoryItem> OnItemEntered, OnItemExited, OnItemDroppedOn, 
        OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClicked;

    private bool empty = true;
    private Color initSlotColor;

    private void Awake()
    {
        initSlotColor = borderImage.color;
        ResetData();
        DeSelect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = false;
    }

    public void DeSelect()
    {
        borderImage.color = initSlotColor;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        empty = false;
    }

    public void Select()
    {
        borderImage.color = initSlotColor * .8f;
    }

    public void OnBeginDrag()
    {
        if (empty) return;

        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerEnter(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;

        if (pointerData != null)
        {
            OnItemEntered?.Invoke(this);
        }
    }

    public void OnPointerExit(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;

        if (pointerData != null)
        {
            OnItemExited?.Invoke(this);
        }
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (empty) return;

        PointerEventData pointerData = (PointerEventData)data;

        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClicked?.Invoke(this);
        }
    }
}
