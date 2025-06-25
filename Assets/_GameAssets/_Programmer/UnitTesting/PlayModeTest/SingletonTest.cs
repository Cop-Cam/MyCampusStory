using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using MyCampusStory.DesignPatterns;

public class SingletonTests : MonoBehaviour
{
    private class TestSingleton : Singleton<TestSingleton> { }

    [UnityTest]
    public IEnumerator B01_SingletonInitialization()
    {
        GameObject obj = new GameObject("TestSingleton");
        TestSingleton instance = obj.AddComponent<TestSingleton>();

        yield return null;

        Assert.NotNull(TestSingleton.Instance);
        Assert.AreEqual(instance, TestSingleton.Instance);

        Object.Destroy(obj);
    }

    [UnityTest]
    public IEnumerator B02_SingletonDestroyDuplicate()
    {
        GameObject obj1 = new GameObject("TestSingleton1");
        TestSingleton instance1 = obj1.AddComponent<TestSingleton>();

        yield return null;

        GameObject obj2 = new GameObject("TestSingleton2");
        TestSingleton instance2 = obj2.AddComponent<TestSingleton>();

        yield return null;

        Assert.AreEqual(instance1, TestSingleton.Instance);
        Assert.AreNotEqual(instance2, TestSingleton.Instance);
        Assert.IsFalse(instance2); // Should be destroyed

        Object.Destroy(obj1);
    }

    [UnityTest]
    public IEnumerator B03_SingletonPersistAcrossScenes()
    {
        GameObject obj = new GameObject("TestSingleton");
        TestSingleton instance = obj.AddComponent<TestSingleton>();

        yield return null;

        Assert.NotNull(TestSingleton.Instance);
        Assert.AreEqual(instance, TestSingleton.Instance);

        // Simulate scene change
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;

        Assert.NotNull(TestSingleton.Instance);
        Assert.AreEqual(instance, TestSingleton.Instance);

        Object.Destroy(obj);
    }
}
