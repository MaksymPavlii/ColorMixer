using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Material materialToReset;
    [SerializeField]
    private UIManager UIManager;

    public static int levelID = 0;
    private bool levelComplete = false;

    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        Blender.OnBlenderFinished += CheckWin;
    }

    private void OnApplicationQuit()
    {
        materialToReset.SetFloat("_Fill", 0);
    }

    private void CheckWin(float colorRatio)
    {
        if(colorRatio >= 90)
        {
            levelComplete = true;
            UIManager.ShowWinScreen(colorRatio);
        }
        else
        {
            levelComplete = false;
            UIManager.ShowLoseScreen(colorRatio);
        }
    }

    public void LoadNextLevel()
    {
        if (levelComplete)
        {
            levelID++;
            if (levelID > 2) levelID = 0;
            materialToReset.SetFloat("_Fill", 0);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            materialToReset.SetFloat("_Fill", 0);
            SceneManager.LoadScene("MainScene");
        }
    }
    private void OnDestroy()
    {
        Blender.OnBlenderFinished -= CheckWin;
    }
}
