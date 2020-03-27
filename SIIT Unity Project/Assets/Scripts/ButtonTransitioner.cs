﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTransitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler

{
    public Color32 m_NormalColor = Color.white;
    public Color32 m_HoverColor = Color.grey;
    public Color32 m_ClickColor = Color.white;

    private Image m_Image = null;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.color = m_HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.color = m_NormalColor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        m_Image.color = m_ClickColor;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        print("Up");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        m_Image.color = m_HoverColor;
    }
}
