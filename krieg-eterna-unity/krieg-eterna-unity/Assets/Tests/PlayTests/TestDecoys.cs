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
    GameObject gameObject;
    Game game;
    Deck deck;
    List<string> choosePower;
    List<string> chooseUnit;
    List<string> chooseKing;
    List<string> chooseUnitGraveyard;
    List<string> choosePowerGraveyard;
    List<string> enemyPower;
    List<string> enemyUnit;
    List<string> enemyKing;
    Row powers;
    Row units;
    Row kings;


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

    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        Debug.Log("UnitySetup Base");
        yield return new WaitWhile(() => sceneLoaded == false);
        gameObject = GameObject.Find("Game");
        game = gameObject.GetComponent<Game>();
        deck = game.activeDeck;
        deck.resetDeck();
        choosePower = new List<string>();
        chooseUnit = new List<string>();
        chooseKing = new List<string>();
        chooseUnitGraveyard = new List<string>();
        choosePowerGraveyard = new List<string>();
        enemyPower = new List<string>();
        enemyUnit = new List<string>();
        enemyKing = new List<string>();
        powers = deck.getRowByType(RowEffected.PowerDeck);
        units = deck.getRowByType(RowEffected.UnitDeck);
        kings = deck.getRowByType(RowEffected.KingDeck);
    }

    [TearDown]
    public void TearDown()
    {
        Debug.Log("TearDown Base");
    }

    [UnityTest]
    public IEnumerator GameLoads()
    {
        deck.dealHand(Game.NUM_POWERS, Game.NUM_UNITS, Game.NUM_KINGS, choosePower, chooseUnit, chooseKing, powers, units, kings, RowEffected.PlayerHand);
        Assert.AreEqual(14, deck.getRowByType(RowEffected.PlayerHand).Count);
        yield return null;
    }
}
