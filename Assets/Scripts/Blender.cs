using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public class Blender : MonoBehaviour
{
    [SerializeField]
    private Transform fruitsTargetPosition;
    [SerializeField]
    private Material liquidMaterial;
    private Animator blenderAnimator;
    private List<Fruit> fruitsInBlender = new();
    private Coroutine timerRoutine;
    private float shakeDuration = 4f;
    private Color liquidColor;
    private Color targetColor;
    private bool isWorking = false;

    public Transform FruitTargetPos
    {
        get { return fruitsTargetPosition; }
    }
    public List<Fruit> FruitsInBlender
    {
        get { return fruitsInBlender; }
    }
    public Color TargetColor
    {
        get { return targetColor; }
        set { targetColor = value; }
    }

    public delegate void BlenderFinished(float colorRatio);
    public static event BlenderFinished OnBlenderFinished;

    private Action<Fruit> releaseAction;

    public void Initialize(Action<Fruit> _releaseAction)
    {
        releaseAction = _releaseAction;
    }

    private void Awake()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            blenderAnimator = GetComponentInChildren<Animator>();
        }
        BlenderStartButton.OnClicked += StartBlender;
    }

    private void ShakeBlender()
    {
        Vector3 shakeStrength = new Vector3(0.0125f, 0.0025f, 0.0125f);

        transform.DOShakePosition(shakeDuration, shakeStrength, 10, 90, false, false);
    }

    public bool IsFull()
    {
        if (fruitsInBlender.Count < 4)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool ISWorking()
    {
        return isWorking;
    }

    private void OpenBlender()
    {
        if (!blenderAnimator.GetBool("Opened"))
        {
            blenderAnimator.SetBool("Opened", true);
            blenderAnimator.Play("OpenCapAnimation");
        }
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);

        timerRoutine = StartCoroutine(CloseBlenderTimer());
    }

    public void AddFruitToBlender(Fruit fruit)
    {
        fruitsInBlender.Add(fruit);
        OpenBlender();
    }

    private IEnumerator CloseBlenderTimer()
    {
        yield return new WaitForSeconds(1.5f);
        CloseBlender();
    }

    private void CloseBlender()
    {
        if (blenderAnimator.GetBool("Opened"))
        {
            blenderAnimator.SetBool("Opened", false);
            blenderAnimator.Play("CloseCapAnimation");
        }
    }

    private IEnumerator FillWithLiquid()
    {
        foreach (var fruit in fruitsInBlender)
        {
            releaseAction(fruit);
        }
        ChangeLiqudColor();
        RaiseLiquidLevel();
        yield return new WaitForSeconds(shakeDuration);
        CompareLiquidColors();
    }

    private void ChangeLiqudColor()
    {
        liquidColor = CalculateAverageLiquidColor();
        Color newSurfaceColor = liquidColor * 1.17f;
        liquidMaterial.DOColor(liquidColor, "_LiquidColor", 0);
        liquidMaterial.DOColor(newSurfaceColor, "_SurfaceColor", 0);
    }

    private Color CalculateAverageLiquidColor()
    {
        int divider = fruitsInBlender.Count;
        Color targetColor = fruitsInBlender[0].FruitColor;
        for (int i = 1; i < fruitsInBlender.Count; i++)
        {
            targetColor += fruitsInBlender[i].FruitColor;
        }
        fruitsInBlender.Clear();
        return targetColor / divider;
    }

    private void RaiseLiquidLevel()
    {
        DOVirtual.Float(0, 1f, 4, v => liquidMaterial.SetFloat("_Fill", v));
    }

    private void StartBlender()
    {
        isWorking = true;
        if (fruitsInBlender.Capacity > 0)
        {
            if (timerRoutine != null)
            {
                StopCoroutine(timerRoutine);
            }
            CloseBlender();
            ShakeBlender();
            StartCoroutine(FillWithLiquid());
        }
    }

    private void CompareLiquidColors()
    {
        float ratio = 0;
        int rgbComponentsNumber = 3;
        int percentileMultiplyer = 100;

        for (int i = 0; i < 3; i++)
        {
            if(liquidColor[i] / targetColor[i] < 1)
            {
                ratio += liquidColor[i] / targetColor[i];
            }
            else
            {
                ratio += targetColor[i] / liquidColor[i];
            }
        }
        OnBlenderFinished?.Invoke(MathF.Round(ratio / rgbComponentsNumber * percentileMultiplyer));
    }

    private void OnDestroy()
    {
        BlenderStartButton.OnClicked -= StartBlender;
    }
}
