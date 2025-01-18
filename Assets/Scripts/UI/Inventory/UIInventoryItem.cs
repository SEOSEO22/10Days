using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler,
    IBeginDragHandler, IEndDragHandler, IDropHandler
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

    public void OnPointerEnter(PointerEventData pointerData)
    {
        if (pointerData != null)
        {
            OnItemEntered?.Invoke(this);
        }
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClicked?.Invoke(this);
        }
    }

    public void OnPointerExit(PointerEventData pointerData)
    {
        if (pointerData != null)
        {
            OnItemExited?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty) return;

        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }
}
