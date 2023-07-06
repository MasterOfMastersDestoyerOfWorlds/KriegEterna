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
    private Card activeShowingCard;
    private static int activePlayerNumber;
    public static State state = State.FREE;
    private static int gameState = (int)GameState.TOUR1;

    private GameObject deckObject;
    private GameObject deskObject;
    private GameObject areasObject;

    public static Deck activeDeck;

    public static RoundType round;

    private bool hasChosenStart;
    private static RowEffected chooseNRow;

    private static RowEffected chooseNSendRow;
    private static System.Action<Row, RowEffected, Card> chooseNAction;
    private Desk desk;

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

    private GameObject score1Object;
    private GameObject score2Object;
    private GameObject score3Object;
    private GameObject score4Object;
    private GameObject score5Object;
    private GameObject score6Object;
    private GameObject score7Object;
    private GameObject score8Object;
    private GameObject cardNumber1Object;
    private GameObject cardNumber2Object;
    private Text score1Text;
    private Text score2Text;
    private Text score3Text;
    private Text score4Text;
    private Text score5Text;
    private Text score6Text;
    private Text score7Text;
    private Text score8Text;
    private Text cardNumber1;
    private Text cardNumber2;

    private GameObject buttonObject;
    private Button button;


    private GameObject giveUpButtonObject;

    public static readonly int NUM_POWERS = 4;
    public static readonly int NUM_UNITS = 9;
    public static readonly int NUM_KINGS = 1;

    Card LastCardPlayed;

    void Awake()
    {
        GameObject camera = GameObject.Instantiate(Resources.Load("Prefabs/Main Camera") as GameObject, new Vector3(0f, 0f, -100f), transform.rotation);
        camera.tag = "MainCamera";
        Debug.Log("Made Camera");
        player2Object = GameObject.Instantiate(Resources.Load("Prefabs/Player") as GameObject, transform.position, transform.rotation);
        var player1Object = GameObject.Instantiate(Resources.Load("Prefabs/Player") as GameObject, transform.position, transform.rotation);
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();

        deskObject = GameObject.Instantiate(Resources.Load("Prefabs/Desk") as GameObject, transform.position, transform.rotation);
        desk = deskObject.GetComponent<Desk>();


        GameObject.Instantiate(Resources.Load("Prefabs/Canvas") as GameObject, transform.position, transform.rotation);


        playerDownNameTextObject = GameObject.Find("DownPlayerName");
        playerDownNameText = playerDownNameTextObject.GetComponent<Text>();

        playerUpNameTextObject = GameObject.Find("UpPlayerName");
        playerUpNameText = playerUpNameTextObject.GetComponent<Text>();


        score1Object = GameObject.Find("Score1");
        score2Object = GameObject.Find("Score2");
        score3Object = GameObject.Find("Score3");
        score4Object = GameObject.Find("Score4");
        score5Object = GameObject.Find("Score5");
        score6Object = GameObject.Find("Score6");
        score7Object = GameObject.Find("Score7");
        score8Object = GameObject.Find("Score8");
        cardNumber1Object = GameObject.Find("cardNumber1");
        cardNumber2Object = GameObject.Find("cardNumber2");
        score1Text = score1Object.GetComponent<Text>();
        score2Text = score2Object.GetComponent<Text>();
        score3Text = score3Object.GetComponent<Text>();
        score4Text = score4Object.GetComponent<Text>();
        score5Text = score5Object.GetComponent<Text>();
        score6Text = score6Object.GetComponent<Text>();
        score7Text = score7Object.GetComponent<Text>();
        score8Text = score8Object.GetComponent<Text>();
        cardNumber1 = cardNumber1Object.GetComponent<Text>();
        cardNumber2 = cardNumber2Object.GetComponent<Text>();


        buttonObject = GameObject.Find("Button");
        button = buttonObject.GetComponent<Button>();

        giveUpButtonObject = GameObject.FindGameObjectWithTag("giveUpButton");
        giveUpButtonObject.SetActive(true);

        activePlayerNumber = (int)PlayerNumber.PLAYER1;
        gameState = (int)GameState.TOUR1;
        Deck deck = player1.getDeck();
        List<string> choosePower = new List<string>();
        choosePower.Add("Redemption");
        choosePower.Add("Ruin");
        choosePower.Add("Spy");
        choosePower.Add("Jester");
        List<string> chooseUnit = new List<string>();
        chooseUnit.Add("Scout");
        List<string> chooseKing = new List<string>();
        chooseKing.Add("TraitorKing");
        List<string> chooseUnitGraveyard = new List<string>();
        chooseUnitGraveyard.Add("Crusader");
        chooseUnitGraveyard.Add("Knight");
        List<string> choosePowerGraveyard = new List<string>();
        List<string> enemyPower = new List<string>();
        List<string> enemyUnit = new List<string>();
        List<string> enemyKing = new List<string>();
        deck.buildDeck(NUM_POWERS, NUM_UNITS, NUM_KINGS, choosePower, chooseUnit, chooseKing, chooseUnitGraveyard, choosePowerGraveyard, enemyPower, enemyUnit, enemyKing);
        deck.buildTargets();
        hasChosenStart = false;
        round = RoundType.RoundOne;
        activePlayerNumber = (int)PlayerNumber.PLAYER1;
        turnsLeft = int.MaxValue;
        player = RowEffected.Player;

    }

    void Start()
    {
        initializePlayersDecks();

        reorganizeGroup();
    }

    void initializePlayersDecks()
    {
        player1.getDeck().sendCardsToDeathList();
        player2.getDeck().sendCardsToDeathList();

        player1.moveCardsFromDeskToDeathArea(activePlayerNumber);
        player2.moveCardsFromDeskToDeathArea(activePlayerNumber);


        Debug.Log("P1 amount of cards: " + player1.getDeck().getRowByType(RowEffected.PlayerHand).Count);
        Debug.Log("P2 amount of cards: " + player2.getDeck().getRowByType(RowEffected.PlayerHand).Count);

        player2.setDeckVisibility(false);
        activeDeck = player1.getDeck();

        if (activeDeck.getRowByType(RowEffected.PlayerHand).Count > 0)
            activeCard = player1.getDeck().getRowByType(RowEffected.PlayerHand)[0];

        activeShowingCard = Instantiate(activeCard) as Card;
        activeShowingCard.transform.position = new Vector3(8.96f, 0, -0.1f);

        reorganizeGroup();
    }

    public void Update()
    {

        // Updating numberOfCards
        // ---------------------------------------------------------------------------------------------------------------
        cardNumber1.text = activeDeck.getRowByType(RowEffected.PlayerHand).Count.ToString();
        cardNumber2.text = activeDeck.getRowByType(RowEffected.EnemyHand).Count.ToString();
        if (!hasChosenStart)
        {
            hasChosenStart = true;
            setChooseN(RowEffected.PlayerHand, activeDeck.sendCardToGraveyard, 3, activeDeck.getRowByType(RowEffected.PlayerHand).Count, new List<CardType>() { CardType.King }, RowEffected.None, State.CHOOSE_N);
        }


        RowEffected player = RowEffected.Player;
        // Picking card
        // ---------------------------------------------------------------------------------------------------------------
        // vector of actual mouse position
        Vector3 mouseRelativePosition = new Vector3(0f, 0f, 0f);
        if (Mouse.current != null)
        {
            mouseRelativePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        mouseRelativePosition.z = 0f;
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
                                    chooseCard(selected);
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
                Debug.Log("ur a wizard harry: " + turnsLeft);
                activeDeck.scoreRows(RowEffected.All);
                if (state == State.FREE)
                {
                    turnOver();
                }
            }
        }
    }

    private void turnOver()
    {
        if (turnsLeft != int.MaxValue && turnsLeft > 0)
        {
            turnsLeft--;
        }
        if((enemyPassed && playerPassed) || turnsLeft == 0)
        {
            Debug.Log("Round Over");
            round = nextRound(round);
            enemyPassed = false;
            playerPassed = false;
            turnsLeft = int.MaxValue;
            return;
        }
        if (!enemyPassed && !playerPassed )
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
    private void gameOver()
    {
        giveUpButtonObject.SetActive(false);
        StartCoroutine(GameOverScreen(2f));
    }

    IEnumerator GameOverScreen(float duration)
    {
        yield return new WaitForSeconds(duration);
    }


    public void chooseCard(Card cardClone)
    {

        Debug.Log("Choosing Card: " + cardClone.cardName);
        Row row = activeDeck.getRowByType(chooseNRow);
        Row displayRow = activeDeck.getRowByType(RowEffected.ChooseN);

        displayRow.Remove(cardClone);
        Card realCard = row[row.IndexOf(cardClone)];
        activeCard.chooseNRemain--;
        row.chooseNRemain--;
        chooseNAction.Invoke(row, chooseNSendRow, realCard);

        cardClone.Destroy();
        reorganizeGroup();

        if (row.chooseNRemain <= 0)
        {
            Debug.Log("Setting Invisible");
            displayRow.setVisibile(false);
            activeDeck.disactiveAllInDeck(false);
            if(activeCard.doneMultiSelection()){  
                state = State.FREE;
            }else{
                state = State.MULTISTEP;
                TargetController.ShowTargets(activeCard, player);
            }
        }
    }

    public static void setChooseN(RowEffected chooseRow, System.Action<Row, RowEffected, Card> action, int numChoose, int numShow, List<CardType> exclude, RowEffected sendRow, State newState)
    {

        Row row = activeDeck.getRowByType(chooseRow);
        row.chooseNRemain = row.Count > numChoose ? numChoose : row.Count;
        state = newState;

        Debug.Log(state);
        Debug.Log("setting up choice");
        float cardHorizontalSpacing = Card.getBaseWidth() * 1.025f;
        float cardThickness = Card.getBaseThickness();
        float attachmentVerticalSpacing = Card.getBaseHeight() * 0.2f;
        Row displayRow = activeDeck.getRowByType(RowEffected.ChooseN);
        chooseNRow = chooseRow;
        chooseNSendRow = sendRow;
        chooseNAction = action;
        while (displayRow.Count > 0)
        {
            Card clone = displayRow[0];
            displayRow.Remove(clone);
            clone.Destroy();

        }
        int revealed = 0;
        for (int i = 0; revealed < numShow && i < row.Count; i++)
        {
            if (!exclude.Contains(row[i].cardType) && (state != State.REVEAL || !row[i].beenRevealed))
            {
                Card clone = Instantiate(row[i]) as Card;
                clone.setVisible(true);
                displayRow.Add(clone);
                revealed++;
                if (state == State.REVEAL)
                {
                    row[i].beenRevealed = true;
                }
            }
        }
        reorganizeRow(cardHorizontalSpacing, cardThickness, attachmentVerticalSpacing, displayRow, displayRow.center);
        displayRow.setActivateRowCardTargets(true, true);
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

    private static void reorganizeRow(float cardHorizontalSpacing, float cardThickness, float attachmentVerticalSpacing, Row row, Vector3 centerVector)
    {

        if (row.Count > 0)
        {
            if (!row.wide)
            {
                row[row.Count - 1].transform.position = centerVector;

                for (int i = row.Count - 1; i >= 0; i--)
                {
                    row[i].transform.position = new Vector3(centerVector.x, centerVector.y, centerVector.z + (row.Count - 1 - i) * cardThickness);
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

    /// <summary>
    /// Defined name of players
    /// </summary>
    private enum PlayerNumber { PLAYER1 = 1, PLAYER2 };

    /// <summary>
    /// Defined game status
    /// </summary>
    private enum GameState { END, TOUR1, TOUR2, TOUR3 };

    public RoundType nextRound(RoundType round)
    {
        switch (round)
        {
            case RoundType.RoundOne: return RoundType.RoundTwo;
            case RoundType.RoundTwo: return RoundType.FinalRound;
            default: return RoundType.GameFinished;
        }
    }

    /// <summary>
    /// Switch player - update active deck
    /// </summary>
    private void switchPlayer()
    {
        reorganizeGroup();
        state = State.BLOCKED;

        StartCoroutine(Wait(0.75f));
    }

    IEnumerator WaitEndTour(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("WaitEndTour() has ended");
        // after
        giveUpButtonObject.SetActive(true);
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);

        //giveUpButtonObject.SetActive(false);
        //playerDownNameTextObject.SetActive(false);
        // playerUpNameTextObject.SetActive(false);

        button.transform.position = new Vector3(0, 0, -1f);

        if (activePlayerNumber == (int)PlayerNumber.PLAYER1)
        {
            activeDeck = player2.getDeck();
            player1.setDeckVisibility(false);
            player2.setDeckVisibility(true);
            activePlayerNumber = (int)PlayerNumber.PLAYER2;
        }
        else if (activePlayerNumber == (int)PlayerNumber.PLAYER2)
        {
            activeDeck = player1.getDeck();
            player1.setDeckVisibility(true);
            player2.setDeckVisibility(false);
            activePlayerNumber = (int)PlayerNumber.PLAYER1;
        }

        button.GetComponentInChildren<Text>().text = "Gracz " + activePlayerNumber + ",\nDotknij aby kontynuować";
        //playerNameText.text = "Gracz " + activePlayerNumber.ToString();

        Vector3 upPlayerNamePosition = playerUpNameTextObject.transform.position;
        playerUpNameTextObject.transform.position = playerDownNameTextObject.transform.position;
        playerDownNameTextObject.transform.position = upPlayerNamePosition;

        // changing position of players health diamonds
        Vector3 playerOneHealthOneVector = player1.getHealthDiamond(1).getPosition();
        Vector3 playerOneHealthSecondVector = player1.getHealthDiamond(2).getPosition();
        Vector3 playerTwoHealthOneVector = player2.getHealthDiamond(1).getPosition();
        Vector3 playerTwoHealthSecondVector = player2.getHealthDiamond(2).getPosition();
        player1.getHealthDiamond(1).setPosition(playerTwoHealthOneVector);
        player1.getHealthDiamond(2).setPosition(playerTwoHealthSecondVector);
        player2.getHealthDiamond(1).setPosition(playerOneHealthOneVector);
        player2.getHealthDiamond(2).setPosition(playerOneHealthSecondVector);

        // number of cards posiotion replacing
        Vector3 playerOneNumberOfCardsPosition = cardNumber1.transform.position;
        cardNumber1.transform.position = cardNumber2.transform.position;
        cardNumber2.transform.position = playerOneNumberOfCardsPosition;


        // score position replacing
        Vector3 tempVector = score1Text.transform.position;
        score1Text.transform.position = score2Text.transform.position;
        score2Text.transform.position = tempVector;

        tempVector = score3Text.transform.position;
        score3Text.transform.position = score6Text.transform.position;
        score6Text.transform.position = tempVector;

        tempVector = score4Text.transform.position;
        score4Text.transform.position = score7Text.transform.position;
        score7Text.transform.position = tempVector;

        tempVector = score5Text.transform.position;
        score5Text.transform.position = score8Text.transform.position;
        score8Text.transform.position = tempVector;

        reorganizeGroup();

        state = State.FREE;
    }


    public void giveUp()
    {
        Debug.Log("Button position: " + button.transform.position);
        if (button.transform.position.y > 5f)
        {
            Debug.Log("Give up!");
            switchPlayer();

            if (activePlayerNumber == (int)PlayerNumber.PLAYER1)
                player1.isPlaying = false;
            else if (activePlayerNumber == (int)PlayerNumber.PLAYER2)
                player2.isPlaying = false;
        }
        else
            Debug.Log("Blocked!");
    }
}