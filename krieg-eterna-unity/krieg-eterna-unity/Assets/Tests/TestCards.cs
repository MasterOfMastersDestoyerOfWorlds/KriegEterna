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
            List<TestCaseCollection> caseCollections = new List<TestCaseCollection>(){
                new TestDecoys(),
                new TestMelee(),
                new TestRanged(),
                new TestSiege(),
                new TestWeather(),
                new TestSpys(),
                new TestPowers(),
                new TestKings(),
            };
            for (int j = 0; j < caseCollections.Count; j++)
            {
                TestCaseCollection collection = caseCollections[j];
                for (int i = 0; i < collection.getCases().Count; i++)
                {
                    TestCase t = collection.getCases()[i];
                    t.player = RowEffected.Player;
                    yield return t;
                    TestCase enemyCase = new TestCase
                    {
                        testName = "Enemy" + t.testName,
                        playerHandCount = t.playerHandCount,
                        enemyHandCount = t.enemyHandCount,
                        player = RowEffected.Enemy,
                        round = t.round,
                        clicks = t.clicks,
                        scoreRows = t.scoreRows,
                    };
                    yield return enemyCase;
                }
            }

        }


        public struct TestCase
        {
            public string testName;
            public int playerHandCount;
            public int enemyHandCount;

            public RowEffected player;

            public RoundType round;
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
            public bool setRevealed = false;
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
            public Click(bool setRevealed, string cardName, RowEffected dealRow, RowEffected rowAfterClick, RowEffected finalRow, bool click)
            {
                this.card = null;
                this.name = cardName;
                this.dealRow = dealRow;
                this.rowAfterClick = rowAfterClick;
                this.finalRow = finalRow;
                this.click = click;
                this.setRevealed = setRevealed;
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
            deck = Game.activeDeck;
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
            Row displayRow = deck.getRowByType(RowEffected.PlayerChooseN);
            displayRow.setVisibile(false);
            deck.disactiveAllInDeck(false);
            Game.lastPlayedCard = null;
            Game.roundEndCards = new List<Card>();
            Game.state = State.BLOCKED;
            Game.playerPassed = false;
            Game.enemyPassed = true;
            Game.enemyDiscarded = Game.NUM_DISCARD_START;
            Game.player = RowEffected.Player;
            Game.round = RoundType.RoundOne;
            Game.reorganizeGroup();
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
            TestBot enemyController = null;
            if (testCase.player == RowEffected.Enemy)
            {
                Game.playerPassed = true;
                Game.enemyPassed = false;
                Game.player = RowEffected.Enemy;
                enemyController = new TestBot();
                Game.enemyController = enemyController;

            }

            Debug.Log("Starting Test:" + testCase.testName);
            List<Click> clicks = testCase.clicks;
            //Setup Cards in row
            List<string> dealtCards = new List<string>();
            for (int i = 0; i < clicks.Count; i++)
            {
                if (!clicks[i].isRowTarget)
                {
                    string cardName = clicks[i].name;
                    RowEffected dealRow = CardModel.getRowFromSide(testCase.player, clicks[i].dealRow);
                    if (!dealtCards.Contains(cardName))
                    {
                        dealtCards.Add(cardName);
                        Card c = deck.dealCardToRow(cardName, dealRow);
                        clicks[i].card = c;
                        c.beenRevealed = clicks[i].setRevealed;
                    }
                    else
                    {
                        if (dealRow != RowEffected.PlayerChooseN)
                        {
                            clicks[i].card = deck.getRowByType(dealRow).getCardByName(cardName);
                        }
                        else
                        {
                            clicks[i].card = deck.getCardByName(cardName);
                        }
                    }
                }
            }


            Game.state = State.FREE;

            //Do clicks on Cards
            for (int i = 0; i < clicks.Count; i++)
            {
                RowEffected dealRow = CardModel.getRowFromSide(testCase.player, clicks[i].dealRow);
                RowEffected rowAfterClick = CardModel.getRowFromSide(testCase.player, clicks[i].rowAfterClick);
                if (clicks[i].click && !clicks[i].isRowTarget)
                {
                    Card c = clicks[i].card;
                    Card clickCard = c;
                    if (CardModel.isDisplayRow(dealRow))
                    {
                        clickCard = deck.getRowByType(dealRow).getCardByName(clicks[i].name);
                    }
                    if (testCase.player == RowEffected.Enemy)
                    {
                        enemyController.nextMoveName = clicks[i].name;
                    }
                    else
                    {
                        InputSystem.Update();
                        MousePositioning(clickCard.transform);
                        ClickOnCard(clickCard.transform);
                        yield return null;
                        MouseUnClick();
                    }
                    yield return null;

                    Debug.Log(deck.getRowByType(rowAfterClick));
                    Debug.Log(rowAfterClick);
                    Debug.Log(c);
                    Debug.Log(deck.getCardRow(c));
                    Assert.AreEqual(true, deck.getRowByType(rowAfterClick).ContainsIncludeAttachments(c),
                    $"row after click cardName: {clicks[i].name}, Expected Row: {rowAfterClick}  {deck.getRowByType(rowAfterClick)}, Actual Row: {deck.getCardRow(c).uniqueType} {deck.getRowByType(deck.getCardRow(c).uniqueType)}");
                }
                else if (clicks[i].isRowTarget)
                {
                    InputSystem.Update();
                    ClickRow click = (ClickRow)clicks[i];
                    Row row = deck.getRowByType(CardModel.getRowFromSide(testCase.player, click.targetRow));
                    if (testCase.player == RowEffected.Enemy)
                    {
                        enemyController.nextMoveName = System.Enum.GetName(typeof(RowEffected), CardModel.getRowFromSide(testCase.player, click.targetRow));
                    }
                    else
                    {
                        MousePositioning(row.target.transform);
                        ClickOnCard(row.target.transform);
                        yield return null;
                        MouseUnClick();
                    }
                    yield return null;
                }
            }
            if (testCase.player == RowEffected.Enemy)
            {
                enemyController.nextMoveName = "";
            }
            yield return null;

            //Check final Card Locations

            for (int i = 0; i < clicks.Count; i++)
            {
                RowEffected finalRow = CardModel.getRowFromSide(testCase.player, clicks[i].finalRow);
                if (!clicks[i].isRowTarget)
                {
                    Card c = clicks[i].card;
                    Assert.AreEqual(true, deck.getRowByType(finalRow).ContainsIncludeAttachments(c),
                    $"final row cardName: {clicks[i].name}, Expected Row: {finalRow} {deck.getRowByType(finalRow)}, Actual Row: {deck.getCardRow(c).uniqueType} {deck.getRowByType(deck.getCardRow(c).uniqueType)}");
                }
            }
            Assert.AreEqual(State.FREE, Game.state);
            Assert.AreEqual(testCase.round, Game.round);
            Assert.AreEqual(false, deck.getRowByType(RowEffected.PlayerChooseN).isVisible(), "ChooseN row is Visible!");
            Assert.AreEqual(false, deck.getRowByType(RowEffected.EnemyChooseN).isVisible(), "ChooseN row is Visible!");

            RowEffected playerHand = CardModel.getRowFromSide(testCase.player, RowEffected.PlayerHand);
            RowEffected enemyHand = CardModel.getRowFromSide(testCase.player, RowEffected.EnemyHand);
            Assert.AreEqual(testCase.playerHandCount, deck.getRowByType(playerHand).Count, $"{deck.getRowByType(playerHand)}");
            Assert.AreEqual(testCase.enemyHandCount, deck.getRowByType(enemyHand).Count, $"{deck.getRowByType(enemyHand)}");


            if (testCase.scoreRows != null)
            {
                foreach ((RowEffected, int) rowPair in testCase.scoreRows)
                {
                    RowEffected row = CardModel.getRowFromSide(testCase.player, rowPair.Item1);
                    Assert.AreEqual(rowPair.Item2, deck.getRowByType(row).scoreRow(deck, CardModel.getPlayerFromRow(row)), $"{deck.getRowByType(row)}");
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
