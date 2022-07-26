using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LevelData", menuName = "LevelData", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int levelID;
    [SerializeField]
    private List<Fruit> fruits;
    [SerializeField]
    private Color requiredColor;
    [SerializeField]
    private GameObject character;

    public List<Fruit> Fruits
    {
        get { return fruits; }
    }
    public int LevelID
    {
        get { return levelID; }
    }
    public Color RequiredColor
    {
        get { return requiredColor; }
    }
    public GameObject Character
    {
        get { return character; }
    }
}
