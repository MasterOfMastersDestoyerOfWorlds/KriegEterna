using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

// TODO - check if SetActive method of GameObject objects works - yes? Correct system of transparent object
// TODO - areas size matched to deck size so player can disactivate card clicking into deck area at the edges

// TODO - Drag and drop system
// TODO - Allign cards with value - increasing. Sorting method to replace cards.
// TODO - Switching players canvas - image like in real gwent
// TODO - Hide active card after second click on it
// TODO - BUG when players put all cards in first tour

public class Game : MonoBehaviour
{
    private Card activeCard;
    private Card activeShowingCard;
    private static int activePlayerNumber;
    public static State state = State.FREE;
    private static int gameState = (int)GameState.TOUR1;

    private GameObject deckObject;
    private GameObject deskObject;
    private GameObject areasObject;

    private Deck activeDeck;
    private Desk desk;
    private Areas areas;

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

    public GameObject endPanelObject;
    public GameObject endTextObject;
    public Text endText;

    private GameObject giveUpButtonObject;

    void Awake()
    {
        player1Object = GameObject.Find("Player1");
        player2Object = GameObject.Find("Player2");
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();

        deskObject = GameObject.Find("Desk");
        desk = deskObject.GetComponent<Desk>();

        areasObject = GameObject.Find("Areas");
        areas = areasObject.GetComponent<Areas>();

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

        endPanelObject = GameObject.FindGameObjectWithTag("EndPanel");
        endPanelObject.transform.position = new Vector3(0, 0, -1.8f);
        endTextObject = GameObject.FindGameObjectWithTag("EndText");
        endText = endTextObject.GetComponent<Text>();

        giveUpButtonObject = GameObject.FindGameObjectWithTag("giveUpButton");
        giveUpButtonObject.SetActive(true);

        endPanelObject.SetActive(false);

        activePlayerNumber = (int)PlayerNumber.PLAYER1;
        gameState = (int)GameState.TOUR1;
        Deck deck = player1.getDeck();
        deck.buildDeck(4, 9, 1);
        deck.buildTargets();

        activePlayerNumber = (int)PlayerNumber.PLAYER1;

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

        if (player1.getDeck().getRowByType(RowEffected.PlayerHand).Count > 0)
            activeCard = player1.getDeck().getRowByType(RowEffected.PlayerHand)[0];

        activeShowingCard = Instantiate(activeCard) as Card;
        activeShowingCard.transform.position = new Vector3(8.96f, 0, -0.1f);
        showActiveCard(false);

        reorganizeGroup();
    }

    void Update()
    {
        // Updating numberOfCards
        // ---------------------------------------------------------------------------------------------------------------
        cardNumber1.text = player1.getDeck().getRowByType(RowEffected.PlayerHand).Count.ToString();
        cardNumber2.text = player2.getDeck().getRowByType(RowEffected.PlayerHand).Count.ToString();

        // Picking card
        // ---------------------------------------------------------------------------------------------------------------
        // vector of actual mouse position
        Vector3 mouseRelativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseRelativePosition.z = 0f;
        if (Input.GetMouseButtonDown(1))
        {
            activeDeck.disactiveAllInDeck();
            reorganizeGroup();
            List<Card> cards = activeDeck.getVisibleCards();
            foreach (Card c in cards)
            {
                if (c.ContainsMouse(mouseRelativePosition))
                {
                    c.scaleBig();
                    c.transform.position = areas.getCenterFront();
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && state != State.BLOCKED)
        {
            Debug.Log("Click Registered");
            bool clickOnTarget = false;

            // if we click on deck collision
            Row playerHand = activeDeck.getRowByType(RowEffected.PlayerHand);
            if (playerHand.target.ContainsMouse(mouseRelativePosition) && playerHand.Count > 0 && state != State.MULTISTEP)
            {
                clickOnTarget = true;
                Debug.Log("Click Registered On Deck");
                for (int i = 0; i < playerHand.Count; i++)
                {
                    Card c = playerHand[i];
                    if (c.ContainsMouse(mouseRelativePosition))
                    {
                        Debug.Log("clicked on: " + c.ToString());
                        if (c.isTargetActive() && c.isPlayable())
                        {
                            Play(c, null, null);
                            c.setTargetActive(false);
                            break;
                        }
                        activeDeck.disactiveAllInDeck();
                        activeCard = c;
                        ShowTargets(c);
                        showActiveCard(true);
                        activeCard.setBaseLoc();
                        activeCard.transform.position += new Vector3(0, Card.getBaseHeight() / 3, 0f);
                        state = State.ACTIVE_CARD;
                    }
                }
            }
            List<Row> activeRowTargets = activeDeck.getActiveRowTargets();
            for (int i = 0; i < activeRowTargets.Count; i++)
            {
                Row row = activeRowTargets[i];
                if (row.target.ContainsMouse(mouseRelativePosition))
                {
                    clickOnTarget = true;
                    Play(activeCard, row, null);
                    activeDeck.disactiveAllInDeck();
                }
            }
            for (int i = 0; i < activeDeck.rows.Count; i++)
            {
                Row row = activeDeck.rows[i];
                if (row.cardTargetsActivated && row.target.ContainsMouse(mouseRelativePosition))
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        Card selected = row[j];
                        if (selected.ContainsMouse(mouseRelativePosition))
                        {
                            List<Card> selectedCards = new List<Card>();
                            selectedCards.Add(selected);
                            clickOnTarget = true;
                            activeDeck.disactiveAllInDeck();
                            Play(activeCard, row, selectedCards);
                        }
                    }
                }
            }
            // Ending player time, tour
            // TODO - Change gameState
            // ---------------------------------------------------------------------------------------------------------------
            if (player1.getDeck().getRowByType(RowEffected.PlayerHand).Count == 0 && player1.isPlaying)
            {
                Debug.Log("Player1 has no cards");
                player1.isPlaying = false;
            }
            if (player2.getDeck().getRowByType(RowEffected.PlayerHand).Count == 0 && player2.isPlaying)
            {
                Debug.Log("Player2 has no cards");
                player2.isPlaying = false;
            }
            if (player1.isPlaying == false && player2.isPlaying == false && gameState != (int)GameState.END)
            {
                player1.updateScore();
                player2.updateScore();
                Debug.Log("Both players have no cards");
                Debug.Log("P1: " + player1.score + ", P2: " + player2.score);
                // End of tour - check who won, subtract health, set new tour
                if (player1.score > player2.score)
                {
                    Debug.Log("player1.score > player2.score");
                    if (player2.health > 0)
                    {
                        Debug.Log("player2.health > 0");
                        // Player 1 won the tour
                        endText.text = "Gracz 1 wygrał!";
                        player2.health--;
                        player2.updateHealthDiamonds();
                    }
                    if (player2.health == 0)
                    {
                        // Player 1 won the game
                        player2.health = -1;
                    }
                }
                else if (player1.score < player2.score)
                {
                    Debug.Log("player1.score <= player2.score");
                    if (player1.health > 0)
                    {
                        Debug.Log("player1.health > 0");
                        // Player 2 won the tour
                        endText.text = "Gracz 2 wygrał!";
                        player1.health--;
                        player1.updateHealthDiamonds();
                    }
                    if (player1.health == 0)
                    {
                        // Player 2 won the game
                        player1.health = -1;
                    }
                }
                else
                {
                    if (player1.health > 0)
                        player1.health--;
                    if (player2.health > 0)
                        player2.health--;
                    endText.text = "Draw!";
                    if (player1.health == 0)
                        player1.health = -1;
                    if (player2.health == 0)
                        player2.health = -1;

                    player1.updateHealthDiamonds();
                    player2.updateHealthDiamonds();
                }
                if (player1.health == -1 && player2.health == -1)
                {
                    Debug.Log("Draw!");
                    gameState = (int)GameState.END;
                }
                else if (player1.health == -1)
                {
                    Debug.Log("Victory!");
                    gameState = (int)GameState.END;
                }
                else if (player2.health == -1)
                {
                    Debug.Log("Defeat");
                    gameState = (int)GameState.END;
                }

                if (gameState != (int)GameState.END)
                {
                    Debug.Log("End tour()");
                    endTour();
                }
                else
                {
                    gameOver();
                }
            }

            Debug.Log("Click on target: " + clickOnTarget);
            if (!clickOnTarget)
            {
                Debug.Log("No valid click resetting");
                activeDeck.disactiveAllInDeck();
                reorganizeGroup();
            }
        }
    }

    public void Play(Card c, Row targetRow, List<Card> targetCards)
    {
        Debug.Log("Playing: " + c.cardName + " Type: " + c.cardType);
        if (targetRow != null)
        {
            Debug.Log(" TargetRow: " + targetRow.name);
        }
        activeDeck.getRowByType(RowEffected.PlayerHand).Remove(c);
        switch (c.cardType)
        {
            case CardType.Melee: activeDeck.getRowByType(RowEffected.PlayerMelee).Add(c); break;
            case CardType.Ranged: activeDeck.getRowByType(RowEffected.PlayerRanged).Add(c); break;
            case CardType.Siege: activeDeck.getRowByType(RowEffected.PlayerSiege).Add(c); break;
            case CardType.Switch: targetRow.Add(c); break;
            case CardType.King: PlayKing(c, targetRow); break;
            case CardType.Spy: PlaySpy(c, targetRow); break;
            case CardType.Decoy: PlayDecoy(c, targetRow, targetCards); break;
            case CardType.Weather: PlayWeather(c); break;
            case CardType.Power: PlayPower(c, targetRow, targetCards); break;
            default: break;
        }
        fulfillSideEffects(c);
        reorganizeGroup();
    }

    public void fulfillSideEffects(Card c)
    {
        if (c.playerCardDraw > 0)
        {

        }
    }
    public void PlayKing(Card c, Row targetRow)
    {
        targetRow.Add(c);
    }

    public void PlaySpy(Card c, Row targetRow)
    {
        if (targetRow == null)
        {
            Debug.Log("Playing Spy in: " + c.rowEffected);
            switch (c.rowEffected)
            {
                case RowEffected.Melee: activeDeck.getRowByType(RowEffected.EnemyMelee).Add(c); Debug.Log("added to enemy melee"); break;
                case RowEffected.Ranged: activeDeck.getRowByType(RowEffected.EnemyRanged).Add(c); Debug.Log("added to enemy ranged"); break;
                case RowEffected.Siege: activeDeck.getRowByType(RowEffected.EnemySiege).Add(c); Debug.Log("added to enemy siege"); break;
                default: activeDeck.getRowByType(RowEffected.PowerGraveyard).Add(c); break;
            }
        }
        else
        {
            targetRow.Add(c);
            Debug.Log("Added Spy to Row: " + targetRow);
        }
    }

    public void PlayDecoy(Card c, Row targetRow, List<Card> targetCards)
    {

        if (c.playerCardReturn > 0)
        {
            int index = targetRow.IndexOf(targetCards[0]);
            targetRow.Insert(index, c);
            for (int i = 0; i < targetCards.Count; i++)
            {
                targetRow.Remove(targetCards[i]);
                activeDeck.getRowByType(RowEffected.PlayerHand).Add(targetCards[i]);
            }
        }
        else if (c.attach)
        {
            targetCards[0].attachCard(c);
        }
    }

    public void PlayWeather(Card c)
    {
        Debug.Log("Playing Weather in: " + c.rowEffected);
        switch (c.rowEffected)
        {
            case RowEffected.Melee: activeDeck.getRowByType(RowEffected.PlayerMelee).Add(c); activeDeck.getRowByType(RowEffected.EnemyMelee).Add(c); break;
            case RowEffected.Ranged: activeDeck.getRowByType(RowEffected.PlayerRanged).Add(c); activeDeck.getRowByType(RowEffected.EnemyRanged).Add(c); break;
            case RowEffected.Siege: activeDeck.getRowByType(RowEffected.PlayerSiege).Add(c); activeDeck.getRowByType(RowEffected.EnemySiege).Add(c); break;
            case RowEffected.All: activeDeck.getRowByType(RowEffected.PowerGraveyard).Add(c); activeDeck.clearAllWeatherEffects(); break;
            default: break;
        }
    }

    public void PlayPower(Card c, Row targetRow, List<Card> targetCards)
    {
        if(c.attach){
            targetCards[0].attachCard(c);
        }
    }

    public void ShowTargets(Card c)
    {
        Debug.Log("Showing Targets for: " + c.cardName + " " + c.cardType);
        switch (c.cardType)
        {
            case CardType.Melee: c.setTargetActive(true); break;
            case CardType.Ranged: c.setTargetActive(true); break;
            case CardType.Siege: c.setTargetActive(true); break;
            case CardType.Switch:
                activeDeck.getRowByType(RowEffected.PlayerMelee).setActivateRowCardTargets(true, false);
                activeDeck.getRowByType(RowEffected.PlayerRanged).setActivateRowCardTargets(true, false);
                break;
            case CardType.King:
                activeDeck.activateRowsByType(true, false, RowEffected.PlayerKing);
                break;
            case CardType.Spy:
                if (c.rowEffected == RowEffected.Enemy)
                {
                    activeDeck.activateRowsByType(true, false, RowEffected.Enemy);
                }
                else
                {
                    c.setTargetActive(true);
                }
            ; break;
            case CardType.Decoy:
                activeDeck.activateRowsByType(true, true, c.rowEffected);
                break;
            case CardType.Weather: c.setTargetActive(true); break;
            case CardType.Power:
                if(c.playerCardDestroy > 0){
                    activeDeck.activateRowsByType(true, true, RowEffected.Player);
                }else if(c.enemyCardDestroy > 0){
                    activeDeck.activateRowsByType(true, true, RowEffected.Enemy);
                }else if(c.attach){
                    activeDeck.activateRowsByType(true, true, RowEffected.All);
                }else if(c.rowEffected != RowEffected.None){
                    activeDeck.activateRowsByType(true, true, c.rowEffected);
                }else{
                    c.setTargetActive(true);
                }
                break;
            default: break;
        }
        c.setPlayable(true);
        reorganizeGroup();
    }


    private void gameOver()
    {
        endPanelObject.SetActive(true);
        giveUpButtonObject.SetActive(false);
        StartCoroutine(GameOverScreen(2f));
    }

    IEnumerator GameOverScreen(float duration)
    {
        yield return new WaitForSeconds(duration);

        // after
        endText.text = "Game Over\n";
        if (player1.health == -1 && player2.health == -1)
            endText.text += "\nDraw";
        else if (player2.health == -1)
            endText.text += "\nDefeat";
        else if (player1.health == -1)
            endText.text += "\nVictory";
    }


    public void reorganizeGroup()
    {
        float cardHorizontalSpacing = Card.getBaseWidth() * 1.025f;
        float cardThickness = Card.getBaseThickness();
        float attachmentVerticalSpacing = Card.getBaseHeight() * 0.2f;
        foreach (Row row in activeDeck.rows)
        {
            reorganizeRow(cardHorizontalSpacing, cardThickness, attachmentVerticalSpacing, row);
        }
    }

    private void reorganizeRow(float cardHorizontalSpacing, float cardThickness, float attachmentVerticalSpacing, Row row)
    {
        Vector3 centerVector = row.center;

        if (row.Count > 0)
        {
            if (!row.wide)
            {
                row[0].transform.position = centerVector;

                for (int i = 1; i < row.Count; i++)
                {
                    row[i].transform.position = new Vector3(centerVector.x, centerVector.y, centerVector.z + i * cardThickness);
                }
            }
            else
            {
                if (row.Count % 2 == 1)
                {
                    int j = 1;
                    row[0].transform.position = centerVector;

                    for (int i = 1; i < row.Count; i++)
                    {
                        row[i].transform.position = new Vector3(centerVector.x + j * cardHorizontalSpacing, centerVector.y, centerVector.z);
                        j *= -1;
                        if (i % 2 == 0)
                            j++;
                    }
                }
                else
                {
                    int j = 1;
                    row[0].transform.position = centerVector + new Vector3(cardHorizontalSpacing / 2, 0, 0);
                    row[1].transform.position = centerVector + new Vector3(-cardHorizontalSpacing / 2, 0, 0);

                    for (int i = 2; i < row.Count; i++)
                    {
                        row[i].transform.position = new Vector3(centerVector.x + j * cardHorizontalSpacing + Math.Sign(j) * (cardHorizontalSpacing / 2), centerVector.y, centerVector.z);

                        j *= -1;
                        if (i % 2 == 1)
                            j++;
                    }
                }
                for (int i = 0; i < row.Count; i++)
                {
                    Card c = row[i];
                    for (int j = 0; j < c.attachments.Count; j++)
                    {
                        Vector3 cardCenter = c.transform.position;
                        c.attachments[j].transform.position = new Vector3(cardCenter.x, cardCenter.y - (j+1) * attachmentVerticalSpacing, cardCenter.z + j * cardThickness);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Defined type of card groups
    /// </summary>
    private enum CardGroup { DECK, SWORD, BOW, TREBUCHET };

    /// <summary>
    /// Defined name of players
    /// </summary>
    private enum PlayerNumber { PLAYER1 = 1, PLAYER2 };

    /// <summary>
    /// Defined game status
    /// </summary>
    private enum GameState { END, TOUR1, TOUR2, TOUR3 };

    /// <summary>
    /// Defined type of card
    /// </summary>
    private enum TypeOfCard { NORMAL, GOLD, SPY, MANEKIN, DESTROY, WEATHER, GOLD_SPY };

    /// <summary>
    /// Switch player - update active deck
    /// </summary>
    private void switchPlayer()
    {
        reorganizeGroup();
        state = State.BLOCKED;
        showActiveCard(false);

        StartCoroutine(Wait(0.75f));
    }

    /// <summary>
    /// End tour - show who won and start new game
    /// </summary>
    private void endTour()
    {
        // before
        endPanelObject.SetActive(true);
        giveUpButtonObject.SetActive(false);

        player1.isPlaying = true;
        player2.isPlaying = true;

        initializePlayersDecks();
        // TODO - Player who has won - started
        Debug.Log("WaitEndTour() has started");
        StartCoroutine(WaitEndTour(3f));
    }

    IEnumerator WaitEndTour(float duration)
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("WaitEndTour() has ended");
        // after
        endPanelObject.SetActive(false);
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
            this.activeDeck = player2.getDeck();
            player1.setDeckVisibility(false);
            player2.setDeckVisibility(true);
            activePlayerNumber = (int)PlayerNumber.PLAYER2;
        }
        else if (activePlayerNumber == (int)PlayerNumber.PLAYER2)
        {
            this.activeDeck = player1.getDeck();
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

    private void showActiveCard(bool ifShow)
    {

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