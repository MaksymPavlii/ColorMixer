using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Blender blender;
    [SerializeField]
    private Transform fruitsParent;
    [SerializeField]
    private List<LevelData> levelsConfiguration;

    private LevelData curLevel;
    private ObjectPool<Fruit> pool;
    private int defaultCapacity = 15;
    private int maxCapacity = 30;
    private Fruit.FruitType requiredType;

    private void Awake()
    {
        curLevel = levelsConfiguration.Find(x => x.LevelID == GameManager.levelID);
        Fruit.OnPutInBlender += SpawnFruit;
        if(blender != null)
        blender.TargetColor = curLevel.RequiredColor;
    }

    private void Start()
    {
        InitFruitsPool();
        SpawnCharacter();
    }

    private void InitFruitsPool()
    {
        pool = new ObjectPool<Fruit>(() =>
        {
            return Instantiate(curLevel.Fruits.Find(x => x.fruitType == requiredType), fruitsParent);
        }, fruit =>
        {
            fruit.gameObject.SetActive(true);
        }, fruit =>
        {
            fruit.gameObject.SetActive(false);
        }, fruit =>
        {
            Destroy(fruit.gameObject);
        }, false, defaultCapacity, maxCapacity);

        foreach (var fruit in curLevel.Fruits)
        {
            SpawnFruit(fruit);
        }
    }

    private void SpawnCharacter()
    {
        Instantiate(curLevel.Character);
    }

    private void SpawnFruit(Fruit fruit)
    {       
        requiredType = fruit.fruitType;
        fruit = pool.Get();
        fruit.Blender = blender;
        blender.Initialize(ReleaseFruit);
    }

    private void ReleaseFruit(Fruit fruit)
    {
        pool.Release(fruit);
    }

    private void OnDestroy()
    {
        Fruit.OnPutInBlender -= SpawnFruit;
    }
}
