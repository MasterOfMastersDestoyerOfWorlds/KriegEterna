using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

// TODO - check if SetActive method of GameObject objects works - yes? Correct system of transparent object
// TODO - areas size matched to deck size so player can disactivate card clicking into deck area at the edges

// TODO - Drag and drop system
// TODO - Allign cards with value - increasing. Sorting method to replace cards.
// TODO - Switching players canvas - image like in real gwent
// TODO - Hide active card after second click on it
// TODO - BUG when players put all cards in first tour

public class Game : MonoBehaviour
{
    public static Card activeCard;
    public static Card lastPlayedCard;
    private static int activePlayerNumber;
    public static State state = State.FREE;

    public static Deck activeDeck;

    public static List<Card> roundEndCards;

    public static RoundType round;

    private static bool hasChosenStart;

    public static int turnsLeft;
    public static bool enemyPassed;
    public static bool playerPassed;
    public static RowEffected player;
    private GameObject playerDownNameTextObject;
    private Text playerDownNameText;

    private GameObject playerUpNameTextObject;
    private Text playerUpNameText;

    private GameObject player1Object;
    private GameObject player2Object;
    private Player player1;
    private Player player2;



    private GameObject giveUpButtonObject;

    public static readonly int NUM_POWERS = 4;
    public static readonly int NUM_UNITS = 9;
    public static readonly int NUM_KINGS = 1;

    void Awake()
    {
        GameObject camera = GameObject.Instantiate(Resources.Load("Prefabs/Main Camera") as GameObject, new Vector3(0f, 0f, -100f), transform.rotation);
        camera.tag = "MainCamera";
        GameObject deckObject = GameObject.Instantiate(Resources.Load("Prefabs/Deck") as GameObject, transform.position, transform.rotation);
        activeDeck = deckObject.GetComponent<Deck>();
        player2Object = GameObject.Instantiate(Resources.Load("Prefabs/Player") as GameObject, transform.position, transform.rotation);
        player1Object = GameObject.Instantiate(Resources.Load("Prefabs/Player") as GameObject, transform.position, transform.rotation);
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();


        GameObject.Instantiate(Resources.Load("Prefabs/Canvas") as GameObject, transform.position, transform.rotation);


        playerDownNameTextObject = GameObject.Find("DownPlayerName");
        playerDownNameText = playerDownNameTextObject.GetComponent<Text>();

        playerUpNameTextObject = GameObject.Find("UpPlayerName");
        playerUpNameText = playerUpNameTextObject.GetComponent<Text>();

        List<string> choosePower = new List<string>();
        choosePower.Add("Redemption");
        choosePower.Add("Frost");
        choosePower.Add("Spy");
        choosePower.Add("Jester");
        List<string> chooseUnit = new List<string>();
        chooseUnit.Add("Juggernaut");
        List<string> chooseKing = new List<string>();
        chooseKing.Add("TraitorKing");
        List<string> chooseUnitGraveyard = new List<string>();
        chooseUnitGraveyard.Add("Crusader");
        chooseUnitGraveyard.Add("Knight");
        List<string> choosePowerGraveyard = new List<string>();
        List<string> enemyPower = new List<string>();
        List<string> enemyUnit = new List<string>();
        List<string> enemyKing = new List<string>();
        activeDeck.buildDeck(NUM_POWERS, NUM_UNITS, NUM_KINGS, choosePower, chooseUnit, chooseKing, chooseUnitGraveyard, choosePowerGraveyard, enemyPower, enemyUnit, enemyKing);
        hasChosenStart = false;
        roundEndCards = new List<Card>();
        round = RoundType.RoundOne;
        turnsLeft = int.MaxValue;
        player = RowEffected.Player;

    }

    void Start()
    {

        reorganizeGroup();
    }

    public void Update()
    {

        // Updating numberOfCards
        // ---------------------------------------------------------------------------------------------------------------
        if (!hasChosenStart)
        {
            hasChosenStart = true;
            ChooseNController.setChooseN(RowEffected.PlayerHand, activeDeck.sendCardToGraveyard, 3, activeDeck.getRowByType(RowEffected.PlayerHand).Count, new List<CardType>() { CardType.King }, RowEffected.None, State.CHOOSE_N, false);
        }


        RowEffected player = RowEffected.Player;
        // Picking card
        // ---------------------------------------------------------------------------------------------------------------
        Vector3 mouseRelativePosition = new Vector3(0f, 0f, 0f);
        if (Mouse.current != null)
        {
            mouseRelativePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        mouseRelativePosition.z = 0f;
        // Right click to inspect card
        // ---------------------------------------------------------------------------------------------------------------
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (state != State.MULTISTEP && state != State.CHOOSE_N)
            {
                activeDeck.disactiveAllInDeck(false);
            }
            else
            {
                activeDeck.disactiveAllInDeck(true);
            }
            reorganizeGroup();
            List<Card> cards = activeDeck.getVisibleCards(state);
            foreach (Card c in cards)
            {
                if (c.ContainsMouse(mouseRelativePosition))
                {
                    c.scaleBig();
                    c.transform.position = activeDeck.areas.getCenterFrontBig();
                    break;
                }
            }
        }
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && state != State.BLOCKED)
        {

            Debug.Log("--------------------------------------------------------------------");
            if (state == State.REVEAL)
            {
                activeDeck.getRowByType(RowEffected.ChooseN).setVisibile(false);
                state = State.FREE;
                return;
            }
            Debug.Log("Click Registered! State: " + state);
            bool clickOnTarget = false;
            Row playerHand = activeDeck.getRowByType(RowEffected.PlayerHand);

            if (playerHand.Count > 0 && state != State.MULTISTEP && state != State.CHOOSE_N)
            {
                for (int i = 0; i < playerHand.Count; i++)
                {
                    Card c = playerHand[i];
                    if (c.ContainsMouse(mouseRelativePosition))
                    {

                        Debug.Log("Click Registered On Deck");
                        clickOnTarget = true;
                        Debug.Log("clicked on: " + c.ToString() + " isPlayable: " + c.isPlayable(activeDeck, player) + " active: " + c.isTargetActive());
                        if (c.isTargetActive() && (CardModel.isUnit(c.cardType) || c.isPlayable(activeDeck, player)))
                        {
                            PlayController.Play(c, null, null, player);
                            c.setTargetActive(false);
                            if (state == State.MULTISTEP)
                            {
                                activeDeck.disactiveAllInDeck(true);
                                TargetController.ShowTargets(c, player);
                                break;
                            }
                            break;
                        }
                        activeDeck.disactiveAllInDeck(false);
                        activeCard = c;
                        TargetController.ShowTargets(c, player);

                        Debug.Log("Setting Card Active: " + c.cardName);
                        activeCard.setBaseLoc();
                        activeCard.transform.position += new Vector3(0, Card.getBaseHeight() / 3, 0f);
                        state = State.ACTIVE_CARD;
                    }
                }
            }
            if (!clickOnTarget)
            {
                List<Row> activeRowTargets = activeDeck.getActiveRowTargets();
                for (int i = 0; i < activeRowTargets.Count; i++)
                {
                    Row row = activeRowTargets[i];
                    if (row.target.ContainsMouse(mouseRelativePosition))
                    {
                        clickOnTarget = true;
                        PlayController.Play(activeCard, row, null, player);
                        if (state != State.MULTISTEP)
                        {
                            activeDeck.disactiveAllInDeck(false);
                        }
                        else
                        {
                            activeDeck.disactiveAllInDeck(true);
                            TargetController.ShowTargets(activeCard, player);
                        }
                        break;
                    }
                }
            }
            if (!clickOnTarget)
            {
                for (int i = 0; i < activeDeck.rows.Count; i++)
                {
                    Row row = activeDeck.rows[i];
                    if (row.cardTargetsActivated)
                    {
                        for (int j = 0; j < row.Count; j++)
                        {
                            Card selected = row[j];
                            Debug.Log("Card: " + selected.cardName + " Contains: " + selected.ContainsMouse(mouseRelativePosition) + "Card Location: " + selected.transform.position + " Mouse: " + mouseRelativePosition);
                            if (selected.ContainsMouse(mouseRelativePosition))
                            {
                                Debug.Log("Clicked on Card:" + selected.cardName);
                                if (state == State.CHOOSE_N)
                                {
                                    Debug.Log("selected!" + selected.cardName);
                                    ChooseNController.chooseCard(selected, player);
                                }
                                else
                                {
                                    PlayController.Play(activeCard, row, selected, player);
                                    if (state != State.MULTISTEP)
                                    {
                                        activeDeck.disactiveAllInDeck(false);
                                    }
                                    else
                                    {
                                        TargetController.ShowTargets(activeCard, player);
                                    }
                                }
                                clickOnTarget = true;
                            }
                        }
                        if (clickOnTarget)
                        {
                            break;
                        }
                    }
                }
            }
            if (!clickOnTarget)
            {
                List<Row> buttons = activeDeck.getRowsByType(RowEffected.Button);
                foreach (Row row in buttons)
                {
                    if (row.target.ContainsMouse(mouseRelativePosition) && row.isVisible())
                    {
                        row.buttonAction.Invoke();
                        clickOnTarget = true;
                        break;
                    }
                }
            }
            if (!clickOnTarget)
            {
                if (state != State.MULTISTEP && state != State.CHOOSE_N)
                {
                    Debug.Log("No valid click resetting");
                    activeDeck.disactiveAllInDeck(false);
                }
                else
                {
                    activeDeck.disactiveAllInDeck(true);
                    activeCard.LogSelectionsRemaining();
                }
                reorganizeGroup();
            }
            else
            {
                activeDeck.scoreRows(RowEffected.All);
                if (state == State.FREE || activeDeck.getRowByType(CardModel.getHandRow(player)).Count == 0)
                {
                    turnOver();
                }
            }
        }
    }

    private void turnOver()
    {
        activeDeck.getRowByType(RowEffected.Skip).setVisibile(false);
        if (turnsLeft != int.MaxValue && turnsLeft > 0)
        {
            turnsLeft--;
        }
        if ((enemyPassed && playerPassed) || turnsLeft == 0)
        {
            Debug.Log("Round Over");
            round = nextRound(round);
            enemyPassed = false;
            playerPassed = false;
            turnsLeft = int.MaxValue;
            List<Row> setAsideRows = activeDeck.getRowsByType(RowEffected.SetAside);
            foreach (Row row in setAsideRows)
            {
                foreach (Card c in row)
                {
                    Debug.Log(c.setAsideReturnRow);
                    Row returnRow = activeDeck.getRowByType(c.setAsideReturnRow);
                    returnRow.Add(c);
                }
                row.RemoveAll(delegate (Card a) { return true; });
            }

            state = State.ROUND_END;
            foreach (Card c in roundEndCards)
            {
                PlayController.Play(c, activeDeck.getCardRow(c), null, player);
            }
            state = State.FREE;
            activeDeck.sendAllToGraveYard(RowEffected.CleanUp, (Row r) =>
            {
                List<Card> remove = new List<Card>();
                foreach (Card c in r)
                {
                    if (c.roundEndRemoveType == RoundEndRemoveType.Protect)
                    {
                        c.roundEndRemoveType = RoundEndRemoveType.Remove;

                    }
                    else
                    {
                        remove.Add(c);
                    }
                }
                return remove;
            });

            roundEndCards = new List<Card>();
            return;
        }
        if (!enemyPassed && !playerPassed)
        {
            player = CardModel.getEnemy(player);
        }
        else if (enemyPassed && !playerPassed)
        {
            player = RowEffected.Player;
        }
        else if (enemyPassed && !playerPassed)
        {
            player = RowEffected.Enemy;
        }
    }
    public static void playerPass()
    {
        playerPassed = true;
    }

    public static void skipActiveCardEffects()
    {
        activeCard.zeroSkipSelectionCounts();
        ChooseNController.endChooseN(activeDeck.getRowByType(RowEffected.ChooseN), player);
        if (CardModel.isUnit(activeCard.cardType) && activeDeck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerHand)).Contains(activeCard))
        {
            PlayController.Play(activeCard, activeDeck.getRowByType(CardModel.getPlayableRow(player, activeCard.cardType)), null, player);
        }
    }
    private void gameOver()
    {
        giveUpButtonObject.SetActive(false);
        StartCoroutine(GameOverScreen(2f));
    }

    IEnumerator GameOverScreen(float duration)
    {
        yield return new WaitForSeconds(duration);
    }




    public static void reorganizeGroup()
    {
        Debug.Log("Reorganizing all rows");
        float cardHorizontalSpacing = Card.getBaseWidth() * 1.025f;
        float cardThickness = Card.getBaseThickness();
        float attachmentVerticalSpacing = Card.getBaseHeight() * 0.2f;
        foreach (Row row in activeDeck.rows)
        {
            reorganizeRow(cardHorizontalSpacing, cardThickness, attachmentVerticalSpacing, row, row.center);
        }
    }

    public static void reorganizeRow(float cardHorizontalSpacing, float cardThickness, float attachmentVerticalSpacing, Row row, Vector3 centerVector)
    {

        if (row.Count > 0)
        {
            if (!row.wide)
            {
                row[row.Count - 1].transform.position = centerVector;

                for (int i = row.Count - 1; i >= 0; i--)
                {
                    row[i].transform.position = new Vector3(centerVector.x, centerVector.y, centerVector.z + (row.Count - 1 - i) * cardThickness);
                    if (row.flipped != row[i].flipped)
                    {
                        row[i].transform.RotateAround(row[i].transform.position, Vector3.up, 180f);
                        row[i].flipped = !row[i].flipped;
                    }
                }
            }
            else
            {
                Vector3 rowStart = new Vector3(centerVector.x - ((row.Count / 2) * cardHorizontalSpacing), centerVector.y, centerVector.z);
                if (row.Count % 2 == 0)
                {
                    rowStart = new Vector3(centerVector.x - (((row.Count) / 2) * cardHorizontalSpacing - (cardHorizontalSpacing / 2)), centerVector.y, centerVector.z);
                }
                for (int i = 0; i < row.Count; i++)
                {
                    row[i].transform.position = new Vector3(rowStart.x + i * cardHorizontalSpacing, rowStart.y, rowStart.z);
                    if (row.flipped != row[i].flipped)
                    {
                        row[i].transform.RotateAround(row[i].transform.position, Vector3.up, 180f);
                        row[i].flipped = !row[i].flipped;
                    }
                }

                for (int i = 0; i < row.Count; i++)
                {
                    Card c = row[i];
                    c.setBaseLoc();
                    for (int j = 0; j < c.attachments.Count; j++)
                    {
                        Vector3 cardCenter = c.transform.position;
                        c.attachments[j].transform.position = new Vector3(cardCenter.x, cardCenter.y - (j + 1) * attachmentVerticalSpacing, cardCenter.z - j * cardThickness);
                        c.attachments[j].setBaseLoc();
                    }
                }
            }
        }
    }

    public RoundType nextRound(RoundType round)
    {
        switch (round)
        {
            case RoundType.RoundOne: return RoundType.RoundTwo;
            case RoundType.RoundTwo: return RoundType.FinalRound;
            default: return RoundType.GameFinished;
        }
    }
}