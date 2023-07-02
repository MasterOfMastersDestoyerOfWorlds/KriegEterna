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


        public static IEnumerable TestCases()
        {
            yield return new TestCase
            {
                testName = "Jester",
                clicks = new List<Click>{
                    new Click("Jester", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            };

            yield return new TestCase
            {
                testName = "Retreat",
                clicks = new List<Click>{
                    new Click("Retreat", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            };

            yield return new TestCase
            {
                testName = "Sack",
                clicks = new List<Click>{
                    new Click("Sack", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.EnemyMelee, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            };

            yield return new TestCase
            {
                testName = "Shipwreck",
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true),
                    new Click("Soldier2", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            };
            yield return new TestCase
            {
                testName = "ShipwreckHalfAvailable",
                clicks = new List<Click>{
                    new Click("Shipwreck", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerRanged, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                    new Click("Soldier", RowEffected.PlayerRanged, RowEffected.PlayerHand, RowEffected.PlayerHand, true)
                }
            };
            yield return new TestCase
            {
                testName = "Death",
                clicks = new List<Click>{
                    new Click("Death", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                }
            };
            yield return new TestCase
            {
                testName = "Death2",
                clicks = new List<Click>{
                    new Click("Death2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new Click("Knight", RowEffected.EnemyMelee, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, true),
                }
            };
            yield return new TestCase
            {
                testName = "Feast2",
                clicks = new List<Click>{
                    new Click("Feast2", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            };
            yield return new TestCase
            {
                testName = "Feast3",
                clicks = new List<Click>{
                    new Click("Feast3", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PlayerMelee, true),
                    new Click("Knight", RowEffected.PlayerMelee, RowEffected.PlayerMelee, RowEffected.PlayerMelee, true),
                }
            };
            yield return new TestCase
            {
                testName = "Grail",
                clicks = new List<Click>{
                    new Click("Grail", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Soldier", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            };

            yield return new TestCase
            {
                testName = "Crusader",
                clicks = new List<Click>{
                    new Click("Grail", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Soldier", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                    new Click("Crusader", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            };

            yield return new TestCase
            {
                testName = "GrailStrengthTestFail",
                clicks = new List<Click>{
                    new Click("Grail", RowEffected.PlayerHand, RowEffected.PlayerHand, RowEffected.PowerGraveyard, true),
                    new ClickRow("UnitGraveyard", RowEffected.UnitGraveyard),
                    new Click("Knight3", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, false),
                    new Click("Mortar", RowEffected.UnitGraveyard, RowEffected.UnitGraveyard, RowEffected.PlayerHand, false),
                }
            };
        }


        public struct TestCase
        {
            public string testName;
            public List<Click> clicks;

            public override string ToString()
            {
                return $"{testName}";
            }
        }

        public class Click
        {
            public Card card;
            public string name;
            public RowEffected dealRow;
            public RowEffected rowAfterClick;
            public RowEffected finalRow;
            public bool click;

            public bool isRowTarget;

            public Click(string cardName, RowEffected dealRow, RowEffected rowAfterClick, RowEffected finalRow, bool click)
            {
                this.card = null;
                this.name = cardName;
                this.dealRow = dealRow;
                this.rowAfterClick = rowAfterClick;
                this.finalRow = finalRow;
                this.click = click;
                this.isRowTarget = false;
            }
        }

        public class ClickRow : Click
        {
            public RowEffected targetRow;
            public ClickRow(string cardName, RowEffected dealRow, RowEffected rowAfterClick, RowEffected finalRow, bool click) : base(cardName, dealRow, rowAfterClick, finalRow, click)
            {
            }
            public ClickRow(string rowName, RowEffected row) : base(rowName, row, row, row, true)
            {
                this.name = rowName;
                this.targetRow = row;
                this.isRowTarget = true;
            }

        }



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





        [UnityTest]
        public IEnumerator TestCard([ValueSource(nameof(TestCases))] TestCase testCase)
        {

            Debug.Log("Starting Test:" + testCase.testName);
            List<Click> clicks = testCase.clicks;
            //Setup Cards in row
            List<string> dealtCards = new List<string>();
            for (int i = 0; i < clicks.Count; i++)
            {
                if (!clicks[i].isRowTarget)
                {
                    string cardName = clicks[i].name;
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
            }

            //Do clicks on Cards
            for (int i = 0; i < clicks.Count; i++)
            {
                if (clicks[i].click && !clicks[i].isRowTarget)
                {
                    Card c = clicks[i].card;
                    InputSystem.Update();
                    MousePositioning(c.transform);
                    ClickOnCard(c.transform);
                    yield return null;
                    MouseUnClick();
                    yield return null;
                    Assert.AreEqual(true, deck.getRowByType(clicks[i].rowAfterClick).ContainsIncludeAttachments(c),
                    $"cardName: {clicks[i].name}, Expected Row: {clicks[i].rowAfterClick}, Actual Row: {deck.getCardRow(c).uniqueType}");
                }
                else if (clicks[i].isRowTarget)
                {
                    InputSystem.Update();
                    ClickRow click = (ClickRow)clicks[i];
                    Row row = deck.getRowByType(click.targetRow);
                    MousePositioning(row.target.transform);
                    ClickOnCard(row.target.transform);
                    yield return null;
                    MouseUnClick();
                    yield return null;
                }
            }

            yield return null;

            //Check final Card Locations

            for (int i = 0; i < clicks.Count; i++)
            {
                if (!clicks[i].isRowTarget)
                {
                    Card c = clicks[i].card;
                    Assert.AreEqual(true, deck.getRowByType(clicks[i].finalRow).ContainsIncludeAttachments(c),
                    $"cardName: {clicks[i].name}, Expected Row: {clicks[i].finalRow}, Actual Row: {deck.getCardRow(c).uniqueType}");
                }
            }
        }

        private void ClickOnCard(Transform transform)
        {
            MouseClick();
            Assert.AreEqual((Vector2)Camera.main.WorldToScreenPoint(transform.position), Mouse.current.position.ReadValue());
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

        private void MousePositioning(Transform transform)
        {
            Vector3 mousepos = Camera.main.WorldToScreenPoint(transform.position);
            using (StateEvent.From(Mouse.current, out var eventPtr))
            {
                Mouse.current.position.WriteValueIntoEvent((Vector2)mousepos, eventPtr);
                InputSystem.QueueDeltaStateEvent(Mouse.current.position, eventPtr);
                InputState.Change(Mouse.current, eventPtr);
            }

            Assert.AreEqual((Vector2)Camera.main.WorldToScreenPoint(transform.position), Mouse.current.position.ReadValue());

        }

    }
}
