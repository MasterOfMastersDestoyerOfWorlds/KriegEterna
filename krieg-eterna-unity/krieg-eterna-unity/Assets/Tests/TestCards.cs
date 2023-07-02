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

            foreach (TestCase t in TestDecoys.cases)
            {
                yield return t;
            }

            foreach (TestCase t in TestMelee.cases)
            {
                yield return t;
            }

            foreach (TestCase t in TestRanged.cases)
            {
                yield return t;
            }

            foreach (TestCase t in TestSiege.cases)
            {
                yield return t;
            }
            
            foreach (TestCase t in TestWeather.cases)
            {
                yield return t;
            }
            
            foreach (TestCase t in TestSpys.cases)
            {
                yield return t;
            }
            
            foreach (TestCase t in TestPowers.cases)
            {
                yield return t;
            }


        }


        public struct TestCase
        {
            public string testName;
            public int playerHandCount;
            public int enemyHandCount;
            public List<Click> clicks;
            public List<(RowEffected, int)> scoreRows;

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
            public bool inDisplayRow = false;
            public bool isRowTarget;

            public Click(string cardName, RowEffected dealRow, RowEffected rowAfterClick, RowEffected finalRow, bool click)
            {
                this.card = null;
                this.name = cardName;
                this.dealRow = dealRow;
                this.rowAfterClick = rowAfterClick;
                this.finalRow = finalRow;
                this.click = click;
                this.inDisplayRow = false;
                this.isRowTarget = false;
            }

            public Click(string cardName, RowEffected dealRow, RowEffected rowAfterClick, RowEffected finalRow, bool click, bool inDisplayRow)
            {
                this.card = null;
                this.name = cardName;
                this.dealRow = dealRow;
                this.rowAfterClick = rowAfterClick;
                this.finalRow = finalRow;
                this.click = click;
                this.inDisplayRow = inDisplayRow;
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
                    $"row after click cardName: {clicks[i].name}, Expected Row: {clicks[i].rowAfterClick}, Actual Row: {deck.getCardRow(c).uniqueType}");
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
                    $"final row cardName: {clicks[i].name}, Expected Row: {clicks[i].finalRow}, Actual Row: {deck.getCardRow(c).uniqueType}");
                }
            }
            Assert.AreEqual(State.FREE, Game.state);
            
            Assert.AreEqual(testCase.playerHandCount, deck.getRowByType(RowEffected.PlayerHand).Count);
            Assert.AreEqual(testCase.enemyHandCount, deck.getRowByType(RowEffected.EnemyHand).Count);

            if(testCase.scoreRows != null){
                foreach((RowEffected, int) rowPair in testCase.scoreRows){
                    Assert.AreEqual(rowPair.Item2, deck.getRowByType(rowPair.Item1).scoreRow(deck));
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
