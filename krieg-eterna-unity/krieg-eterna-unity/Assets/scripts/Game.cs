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
    private static int state = (int)Status.FREE;
    private static int gameStatus = (int)GameStatus.TOUR1;

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
        endPanelObject.transform.position = new Vector3(0,0,-1.8f);
        endTextObject = GameObject.FindGameObjectWithTag("EndText");
        endText = endTextObject.GetComponent<Text>();

        giveUpButtonObject = GameObject.FindGameObjectWithTag("giveUpButton");
        giveUpButtonObject.SetActive(true);

        endPanelObject.SetActive(false);

        activePlayerNumber = (int)PlayerNumber.PLAYER1;
        gameStatus = (int)GameStatus.TOUR1;
    }    

    void Start()
    {
        
        player1.getDeck().buildDeck(4, 9, 1);
        player1.getDeck().buildTargets();

        activePlayerNumber = (int)PlayerNumber.PLAYER1;

        initializePlayersDecks();
    }

    void initializePlayersDecks()
    {
        player1.getDeck().sendCardsToDeathList();
        player2.getDeck().sendCardsToDeathList();

        player1.moveCardsFromDeskToDeathArea(activePlayerNumber);
        player2.moveCardsFromDeskToDeathArea(activePlayerNumber);

        //Debug.Log("Deleted " + deleteAllCardClones() + " cards");

        // player1.reloadDeck();
        //player2.reloadDeck();

        Debug.Log("P1 amount of cards: " + player1.getDeck().playerHand.Count);
        Debug.Log("P2 amount of cards: " + player2.getDeck().playerHand.Count);

        player2.setDeckVisibility(false);
        activeDeck = player1.getDeck();

        if (player1.getDeck().playerHand.Count > 0)
            activeCard = player1.getDeck().playerHand[0];

        activeShowingCard = Instantiate(activeCard) as Card;
        activeShowingCard.transform.position = new Vector3(8.96f, 0, -0.1f);
        showActiveCard(false);

        reorganizeGroup();
    }

    private enum Status{
        FREE,
        ACTIVE_CARD,
        BLOCKED
    };

    /// <summary>
    /// Delete all clone cards
    /// </summary>
    /// <returns>Number of deleted cards</returns>
    /*public int deleteAllCardClones()
    {
        int cloneNumber = 0;
        GameObject[] cloneCards = GameObject.FindGameObjectsWithTag("CloneCard");
        cloneNumber = cloneCards.Length;

        foreach (GameObject go in cloneCards)
            GameObject.DestroyObject(go);

        return cloneNumber;
    }*/

    void Update()
    {
        // Updating numberOfCards
        // ---------------------------------------------------------------------------------------------------------------
        cardNumber1.text = player1.getDeck().playerHand.Count.ToString();
        cardNumber2.text = player2.getDeck().playerHand.Count.ToString();

        // Picking card
        // ---------------------------------------------------------------------------------------------------------------
        // vector of actual mouse position
        Vector3 mouseRelativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseRelativePosition.z = 0f;
        if (Input.GetMouseButtonDown(0) && state != ((int)Status.BLOCKED))
        {
            Debug.Log("Click Registered");
            // if we click on deck collision
            if (areas.getDeckColliderBounds().Contains(mouseRelativePosition) && activeDeck.playerHand.Count > 0)
            {
                Debug.Log("Click Registered On Deck");
                for (int i = 1; i < activeDeck.playerHand.Count; i++)
                {
                    Card c = activeDeck.playerHand[i];
                    mouseRelativePosition.z = c.transform.position.z;
                    if (c.getBounds().Contains(mouseRelativePosition))
                    {
                        Debug.Log("clicked on: " + c.ToString());
                        if(c.isActive()){
                            Play(c);
                            c.setActive(false);
                            break;
                        }
                        activeDeck.disactiveAllInDeck();
                        activeCard = c;
                        c.setActive(true);

                        showActiveCard(true);
                        activeCard.setBaseLoc();
                        activeCard.transform.position += new Vector3(0,Card.getBaseHeight()/3, 0f);
                        state = (int)Status.ACTIVE_CARD;
                    }
                }
            }
            // Ending player time, tour
            // TODO - Change gameStatus
            // ---------------------------------------------------------------------------------------------------------------
            if (player1.getDeck().playerHand.Count == 0 && player1.isPlaying)
            {
                Debug.Log("Player1 has no cards");
                player1.isPlaying = false;
            }
            if (player2.getDeck().playerHand.Count == 0 && player2.isPlaying)
            {
                Debug.Log("Player2 has no cards");
                player2.isPlaying = false;
            }
            if (player1.isPlaying == false && player2.isPlaying == false && gameStatus != (int)GameStatus.END)
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
                    gameStatus = (int)GameStatus.END;
                }
                else if (player1.health == -1)
                {
                    Debug.Log("Victory!");
                    gameStatus = (int)GameStatus.END;
                }
                else if (player2.health == -1)
                {
                    Debug.Log("Defeat");
                    gameStatus = (int)GameStatus.END;
                }

                if (gameStatus != (int)GameStatus.END)
                {
                    Debug.Log("End tour()");
                    endTour();
                }
                else
                {
                    gameOver();
                }
            }
        }
    }

    public void Play(Card c){
        Debug.Log("Playing: " + c.cardName + " " + c.cardType);
        activeDeck.playerHand.Remove(c);
        switch (c.cardType){
            case CardType.Melee: activeDeck.meleeRow.Add(c); break;
            case CardType.Ranged: activeDeck.rangedRow.Add(c); break;
            case CardType.Siege: activeDeck.siegeRow.Add(c); break;
            case CardType.Switch: activeDeck.meleeRow.Add(c); break;
            case CardType.King: PlayKing(c); break;
            case CardType.Spy: PlaySpy(c); break;
            case CardType.Decoy: PlayDecoy(c); break;
            case CardType.Weather: PlayWeather(c); break;
            case CardType.Power: PlayPower(c); break;
            default:break;
        }

        reorganizeGroup();
    }

    public void PlayKing(Card c){
    }
    
    public void PlaySpy(Card c){
        Debug.Log("Playing Spy in: " + c.rowEffected);
        switch (c.rowEffected){
            case RowEffected.Melee: activeDeck.enemyMeleeRow.Add(c); Debug.Log("added to enemy melee"); break;
            case RowEffected.Ranged: activeDeck.enemyRangedRow.Add(c); Debug.Log("added to enemy ranged"); break;
            case RowEffected.Siege: activeDeck.enemySiegeRow.Add(c); Debug.Log("added to enemy siege");break;
            default:break;
        }
    }
    
    public void PlayDecoy(Card c){
    }
    
    public void PlayWeather(Card c){
        Debug.Log("Playing Weather in: " + c.rowEffected);
        switch (c.rowEffected){
            case RowEffected.Melee: activeDeck.meleeRow.Add(c);activeDeck.enemyMeleeRow.Add(c); break;
            case RowEffected.Ranged: activeDeck.rangedRow.Add(c);activeDeck.enemyRangedRow.Add(c); break;
            case RowEffected.Siege: activeDeck.siegeRow.Add(c);activeDeck.enemySiegeRow.Add(c); break;
            case RowEffected.All: activeDeck.powerGraveyard.Add(c);activeDeck.clearAllWeatherEffects();break;
            default: break;
        }
    }
    
    public void PlayPower(Card c){
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
        if(player1.health == -1 && player2.health == -1)
            endText.text += "\nDraw";
        else if(player2.health == -1)
            endText.text += "\nDefeat";
        else if(player1.health == -1)
            endText.text += "\nVictory";
    }


    public void reorganizeGroup()
    {

        float cardHorizontalSpacing = Card.getBaseWidth() * 1.025f;
        float cardThickness = Card.getBaseThickness();
        reorganizeRow(cardHorizontalSpacing, activeDeck.playerHand);
        reorganizeRow(cardHorizontalSpacing, activeDeck.meleeRow);
        reorganizeRow(cardHorizontalSpacing, activeDeck.rangedRow);
        reorganizeRow(cardHorizontalSpacing, activeDeck.siegeRow);
        reorganizeRow(cardHorizontalSpacing, activeDeck.enemyMeleeRow);
        reorganizeRow(cardHorizontalSpacing, activeDeck.enemyRangedRow);
        reorganizeRow(cardHorizontalSpacing, activeDeck.enemySiegeRow);
        reorganizeVertRow(cardThickness, activeDeck.unitGraveyard, areas.getUnitGraveyardCenterVector());
        reorganizeVertRow(cardThickness, activeDeck.powerGraveyard, areas.getPowerGraveyardCenterVector());
    }

    private void reorganizeVertRow(float cardThickness, List<Card> deck, Vector3 centerVector)
    {
        if (deck.Count > 0)
        {
            deck[0].transform.position = centerVector;

            for (int i = 1; i < deck.Count; i++)
            {
                deck[i].transform.position = new Vector3(centerVector.x, centerVector.y, centerVector.z + i * cardThickness);
            }
        }
    }

    private void reorganizeRow(float cardHorizontalSpacing, Row row)
    {
        Vector3 centerVector = row.center;
        Debug.Log(centerVector);
        if (row.Count > 0)
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
    private enum GameStatus { END, TOUR1, TOUR2, TOUR3 };
    
    /// <summary>
    /// Defined type of card
    /// </summary>
    private enum TypeOfCard { NORMAL, GOLD, SPY, MANEKIN, DESTROY, WEATHER , GOLD_SPY};

    /// <summary>
    /// Switch player - update active deck
    /// </summary>
    private void switchPlayer()
    {
        reorganizeGroup();
        state = (int)Status.BLOCKED;
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

        state = (int)Status.FREE;
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