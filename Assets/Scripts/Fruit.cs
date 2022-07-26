using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Fruit : MonoBehaviour
{
    [SerializeField]
    private FruitType _fruitType;
    [SerializeField]
    private Color fruitColor;
    private Blender blender;

    public Color FruitColor
    {
        get { return fruitColor; }
    }

    public Blender Blender
    {
        get { return blender; }
        set { blender = value; }
    }

    public FruitType fruitType
    {
        get { return _fruitType; }
    }

    public delegate void PutInBlender(Fruit fruit);
    public static event PutInBlender OnPutInBlender;

    private void OnMouseDown()
    {
        if (!blender.IsFull() && !blender.ISWorking())
        {
            JumpInBlender();
        }
    }

    private void JumpInBlender()
    {
        blender.AddFruitToBlender(this);
        transform.DOJump(blender.FruitTargetPos.position, 1f, 1, 0.65f).SetEase(Ease.InOutSine).OnComplete(
            () => { OnPutInBlender?.Invoke(this); });
    }
    
    public enum FruitType
    {
        Apple,
        Banana,
        Eggplant,
        Cherry,
        Orange,
        Pear,
        Tomato
    }
}
