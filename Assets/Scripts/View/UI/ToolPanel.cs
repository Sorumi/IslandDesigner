using System;
using UnityEngine;
using UnityEngine.UI;

public class ToolPanel : MonoBehaviour
{
    public Button Water;
    public Button Tall;

    public Action OnWaterClick;
    public Action OnTallClick;

    public void Init()
    {
        Water.onClick.AddListener(waterClickHandler);
        Tall.onClick.AddListener(tallClickHandler);
    }

    void waterClickHandler()
    {
        if (OnWaterClick != null)
            OnWaterClick();
    }

    void tallClickHandler()
    {
        if (OnTallClick != null)
            OnTallClick();
    }
}