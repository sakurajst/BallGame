﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    RectTransform parent = null;
    Vector2 basePosition = Vector2.zero;
    public Vector2 deltaPos;
    [SerializeField] float diameter = 150;
    [SerializeField] float cutDis = 1;
    bool isMove = false;
    RectTransform rt;
    [SerializeField] RectTransform arrow;
    [SerializeField] Image arrowImage;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        parent = transform.parent.GetComponent<RectTransform>();
        basePosition = new Vector2(parent.anchoredPosition.x + parent.rect.width * 0.5f, parent.anchoredPosition.y + parent.rect.height * 0.5f);
    }

    void Update()
    {
        if (!isMove)
        {
            if (rt.anchoredPosition.magnitude < cutDis)
                rt.anchoredPosition = Vector2.zero;
            else
                rt.anchoredPosition -= (rt.anchoredPosition).normalized * cutDis * Time.deltaTime * 50;
        }

        deltaPos = (rt.anchoredPosition) / diameter;

        if (rt.anchoredPosition.sqrMagnitude >= 1)
        {
            arrow.anchoredPosition = rt.anchoredPosition.normalized * (diameter + rt.rect.width) * 0.5f;
            float angle = Mathf.Atan2(rt.anchoredPosition.y, rt.anchoredPosition.x) * 180 / Mathf.PI - 40;
            arrow.localRotation = Quaternion.Euler(0, 0, angle);
            arrowImage.color = new Color(1, 1, 1, rt.anchoredPosition.sqrMagnitude / (diameter * diameter * 0.25f));
        }
    }

    public void OnMouseDrag()
    {
        isMove = true;
        Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - basePosition;

        if (delta.sqrMagnitude > diameter * diameter)
        {
            delta.Normalize();
            delta *= diameter;
        }

        rt.anchoredPosition = delta * 0.5f;
    }

    private void OnMouseDown()
    {
        isMove = true;
    }

    public void OnMouseUp()
    {
        isMove = false;
    }
}
