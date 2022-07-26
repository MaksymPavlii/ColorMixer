using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlenderStartButton : MonoBehaviour
{
    private bool isActive = true;

    public delegate void Clicked();
    public static event Clicked OnClicked;

    public void OnMouseDown()
    {
        if (isActive)
        {
            OnClicked?.Invoke();
        }
        isActive = false;
    }
}

