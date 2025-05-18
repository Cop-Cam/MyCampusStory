using NUnit.Framework;
using UnityEngine;
using MyCampusStory.DesignPatterns;

/// <summary>
/// A dummy component to test Singleton<T>
/// </summary>
public class DummySingleton : Singleton<DummySingleton> { }

[TestFixture]
public class SingletonTests
{
    [SetUp]
    public void SetUp()
    {
        // Destroy any existing instance to ensure a clean slate
        DummySingleton existingInstance = Object.FindObjectOfType<DummySingleton>();
        if (existingInstance != null)
        {
            Object.DestroyImmediate(existingInstance.gameObject);
        }

        // Manually create a new GameObject for the Singleton
        GameObject go = new GameObject("DummySingleton");
        go.AddComponent<DummySingleton>();
    }


    [Test]
    public void Singleton_Instance_Created_When_Accessed()
    {
        // Act
        DummySingleton instance = Singleton<DummySingleton>.Instance;
        
        // Assert
        Assert.IsNotNull(instance);
    }

    [Test]
    public void Singleton_Only_One_Instance_Exists()
    {
        // Arrange
        DummySingleton firstInstance = Singleton<DummySingleton>.Instance;
        DummySingleton secondInstance = Singleton<DummySingleton>.Instance;
        
        // Assert
        Assert.AreSame(firstInstance, secondInstance);
    }

    [Test]
    public void Singleton_Does_Not_Create_New_Instance_If_One_Already_Exists()
    {
        // Arrange
        GameObject go = new GameObject("DummySingleton");
        DummySingleton preExistingInstance = go.AddComponent<DummySingleton>();

        // Act
        DummySingleton instance = Singleton<DummySingleton>.Instance;

        // Assert
        Assert.AreSame(preExistingInstance, instance);
    }
}
