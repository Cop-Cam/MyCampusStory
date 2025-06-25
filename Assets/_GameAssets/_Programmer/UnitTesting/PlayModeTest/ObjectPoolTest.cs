using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using MyCampusStory.DesignPatterns;

public class ObjectPoolTests
{
    private GameObject poolGO;
    private ObjectPool pool;
    private GameObject pooledPrefab;
    private Transform poolHolder;

    [SetUp]
    public void SetUp()
    {
        // Set up test GameObjects
        poolGO = new GameObject("ObjectPool");
        pool = poolGO.AddComponent<ObjectPool>();

        poolHolder = new GameObject("PoolHolder").transform;
        pooledPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pooledPrefab.name = "PooledObject";

        pool.Init(poolHolder, pooledPrefab, 2);
    }

    [Test]
    public void B06_ReturnObjectToPool()
    {
        GameObject obj = pool.GetObject();
        obj.SetActive(true);

        pool.ReturnObject(obj);

        Assert.IsFalse(obj.activeInHierarchy, "Returned object should be inactive.");
        Assert.AreEqual(poolHolder, obj.transform.parent, "Returned object should be under poolHolder.");
    }

    [Test]
    public void B07_GetObjectFromPool()
    {
        GameObject obj1 = pool.GetObject();
        GameObject obj2 = pool.GetObject();

        // Both should be from initial pool
        Assert.IsNotNull(obj1);
        Assert.IsNotNull(obj2);
        Assert.AreEqual("PooledObject", obj1.name.Replace("(Clone)", "").Trim(), "Should return correct object.");
        Assert.AreEqual("PooledObject", obj2.name.Replace("(Clone)", "").Trim(), "Should return correct object.");
    }

    [Test]
    public void B08_GetObjectFromEmptyPool()
    {
        // Exhaust the pool
        GameObject obj1 = pool.GetObject();
        obj1.SetActive(true);
        GameObject obj2 = pool.GetObject();
        obj2.SetActive(true);

        // This should create a new one
        GameObject obj3 = pool.GetObject();
        Assert.IsNotNull(obj3, "Should create new object if none available.");
        Assert.AreEqual(3, poolHolder.childCount + 1, "Pool should have grown to include a new object.");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(poolGO);
        Object.DestroyImmediate(poolHolder.gameObject);
        Object.DestroyImmediate(pooledPrefab);
    }
}
