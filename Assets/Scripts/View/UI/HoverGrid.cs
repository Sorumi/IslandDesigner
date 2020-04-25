using System;
using UnityEngine;
using UnityEngine.UI;

public class HoverGrid : MonoBehaviour
{
    RectTransform rectTransform;

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPosition(Vector2 position)
    {
        rectTransform.position = new Vector3(position.x, 0.01f, position.y);
    }
}