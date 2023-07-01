using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestDecoys
{
    // A Test behaves as an ordinary method
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    bool sceneLoaded;
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("deskScene", LoadSceneMode.Single);

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }

    [UnityTest]
    public IEnumerator TestDecoysWithEnumeratorPasses()
    {
        yield return new WaitWhile(() => sceneLoaded == false);
        GameObject gameObject = GameObject.Find("Game");
        var game = gameObject.GetComponent<Game>();
        Debug.Log("REEEEE TESTING");
        Debug.Log(game.activeDeck.getRowByType(RowEffected.PlayerHand).Count);
        Assert.AreEqual(14, game.activeDeck.getRowByType(RowEffected.PlayerHand).Count);
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
