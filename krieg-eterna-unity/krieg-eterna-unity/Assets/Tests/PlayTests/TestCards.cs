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

namespace KriegTests
{

    public class TestCards
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
            game.reorganizeGroup();
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


        private static IEnumerable TestCases()
        {
            yield return new TestCase
            {
                cardName = "Jester",
                clicks = new List<Click>{
                new Click("Jester", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
            }
            };
            yield return new TestCase
            {
                cardName = "Retreat",
                clicks = new List<Click>{
                new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
            }
            };
        }


        public struct TestCase
        {
            public string cardName;
            public List<Click> clicks;

            public override string ToString()
            {
                return $"{cardName}";
            }
        }

        public class Click
        {
            public Card card;
            public string cardName;
            public RowEffected dealRow;
            public RowEffected rowAfterClick;
            public RowEffected finalRow;
            public bool click;

            public Click(string cardName, RowEffected dealRow, RowEffected rowAfterClick, RowEffected finalRow, bool click)
            {
                this.card = null;
                this.cardName = cardName;
                this.dealRow = dealRow;
                this.rowAfterClick = rowAfterClick;
                this.finalRow = finalRow;
                this.click = click;
            }

        }


        [UnityTest]
        public IEnumerator TestCard([ValueSource(nameof(TestCases))] TestCase testCase)
        {
            Debug.Log("Starting Test:" + testCase.cardName);
            List<Click> clicks = testCase.clicks;
            //Setup Cards in row
            List<string> dealtCards = new List<string>();
            for (int i = 0; i < clicks.Count; i++)
            {
                string cardName = clicks[i].cardName;
                if (!dealtCards.Contains(cardName))
                {
                    dealtCards.Add(cardName);
                    Card c = deck.dealCardToRow(cardName, clicks[i].dealRow);
                    clicks[i].card = c;
                }
                else
                {
                    clicks[i].card = deck.getRowByType(clicks[i].dealRow).getCardByName(cardName);
                }
            }

            //Do clicks on Cards
            for (int i = 0; i < clicks.Count; i++)
            {
                if (clicks[i].click)
                {
                    Card c = clicks[i].card;
                    ClickOnCard(c);
                    yield return null;
                    MouseUnClick();
                    yield return null;
                    Assert.AreEqual(true, deck.getRowByType(clicks[i].rowAfterClick).Contains(c),
                    $"cardName: {clicks[i].cardName}, Expected Row: {clicks[i].rowAfterClick}, Actual Row: {deck.getCardRow(c).uniqueType}");
                }
            }

            yield return null;

            //Check final Card Locations

            for (int i = 0; i < clicks.Count; i++)
            {
                Card c = clicks[i].card;
                Assert.AreEqual(true, deck.getRowByType(clicks[i].finalRow).Contains(c));
            }
        }

        private void ClickOnCard(Card c)
        {
            InputSystem.Update();
            MousePositioning(c);
            InputSystem.Update();
            MouseClick();
            Assert.AreEqual((Vector2)Camera.main.WorldToScreenPoint(c.transform.position), Mouse.current.position.ReadValue());
            Debug.Log("Mouse Position BeforeUpdate: " + Mouse.current.position.ReadValue());
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
            Debug.Log("Mouse Position: " + mousepos);
            InputSystem.QueueDeltaStateEvent(Mouse.current.position, (Vector2)mousepos);
            mouse.WarpCursorPosition(mousepos);
        }

    }
}
