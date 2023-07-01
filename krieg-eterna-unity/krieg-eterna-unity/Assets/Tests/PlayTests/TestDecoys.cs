using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

public class TestDecoys
{
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
    private Mouse mouse;
    Row powers;
    Row units;
    Row kings;
    
    private InputTestFixture inputTester;



    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("deskScene", LoadSceneMode.Single);
        // SceneManager.LoadScene("Main");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
    }

    [UnitySetUp]
    public IEnumerator PerTestSetUp()
    {
        inputTester = new InputTestFixture();
        inputTester.Setup();
        mouse = InputSystem.AddDevice<Mouse>();
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
        Row displayRow = deck.getRowByType(RowEffected.ChooseN);
        displayRow.setVisibile(false);
        deck.disactiveAllInDeck(false);
        Game.state = State.FREE;
    }

    [TearDown]
    public void TearDown()
    {

        inputTester.TearDown();
        Debug.Log("TearDown Base");
    }

    [UnityTest]
    public IEnumerator GameLoads()
    {
        deck.dealHand(Game.NUM_POWERS, Game.NUM_UNITS, Game.NUM_KINGS, choosePower, chooseUnit, chooseKing, RowEffected.PlayerHand);
        Assert.AreEqual(14, deck.getRowByType(RowEffected.PlayerHand).Count);

        yield return null;
    }


    [UnityTest]
    public IEnumerator Jester()
    {
        string cardName = "Jester";
        string targetCardName = "Knight";

        Debug.Log("Starting Test:" + cardName);
        choosePower.Add(cardName);
        enemyUnit.Add(targetCardName);
        deck.dealListToRow(enemyPower, enemyUnit, enemyKing, RowEffected.EnemyMelee);
        deck.dealHand(Game.NUM_POWERS, Game.NUM_UNITS, Game.NUM_KINGS, choosePower, chooseUnit, chooseKing, RowEffected.PlayerHand);
        Assert.AreEqual(true, deck.getRowByType(RowEffected.PlayerHand).ContainsCard(cardName));
        Card card = deck.getRowByType(RowEffected.PlayerHand).getCardByName(cardName);
        Card targetCard = deck.getRowByType(RowEffected.EnemyMelee).getCardByName(targetCardName);
        ClickOnCard(card);
        yield return null;
        Assert.AreEqual(false, Mouse.current.leftButton.wasPressedThisFrame);
        Assert.AreEqual(true, card.Equals(game.activeCard));
        yield return null; 
        MouseUnClick();
        yield return null;
        ClickOnCard(targetCard);
        yield return null;
        Assert.AreEqual(true, deck.getRowByType(RowEffected.PlayerHand).Contains(targetCard));
        Assert.AreEqual(true, deck.getRowByType(RowEffected.EnemyMelee).Contains(card));
    }

    private void ClickOnCard(Card c){
        InputSystem.Update();
        MousePositioning(c);
        InputSystem.Update();
        MouseClick();
        game.Update();
    }

    private void MouseClick()
    {
        using (StateEvent.From(Mouse.current, out var eventPtr))
        {
            Mouse.current.leftButton.WriteValueIntoEvent(1f, eventPtr);
            InputSystem.QueueEvent(eventPtr);
            InputState.Change(Mouse.current, eventPtr);
            Assert.AreEqual(1f, Mouse.current.leftButton.ReadValue());
            Assert.AreEqual(true, Mouse.current.leftButton.wasPressedThisFrame);
        }
        
    }
    private void MouseUnClick()
    {
        using (StateEvent.From(Mouse.current, out var eventPtr))
        {
            Mouse.current.leftButton.WriteValueIntoEvent(0f, eventPtr);
            InputSystem.QueueEvent(eventPtr);
            InputState.Change(Mouse.current, eventPtr);
            Assert.AreEqual(0f, Mouse.current.leftButton.ReadValue());
            Assert.AreEqual(false, Mouse.current.leftButton.wasPressedThisFrame);
        }
        
    }

    private void MousePositioning(Card c)
    {
        Vector3 mousepos = Camera.main.WorldToScreenPoint(c.transform.position); // 3D worldpos into 2D
        InputSystem.QueueDeltaStateEvent(Mouse.current.position,  (Vector2)mousepos);
        mouse.WarpCursorPosition(mousepos);
    }

}
