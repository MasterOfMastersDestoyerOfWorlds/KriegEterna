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
    [UnityTest]
    public IEnumerator TestDecoysWithEnumeratorPasses()
    {
        GameObject gameObject = new GameObject();
        var game = gameObject.AddComponent<Game>();
        yield return null;
        Debug.Log("REEEEE TESTING");
        Debug.Log(game.activeDeck.getRowByType(RowEffected.PlayerHand).Count);
        Assert.AreEqual(14, game.activeDeck.getRowByType(RowEffected.PlayerHand).Count);
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
    }
}
