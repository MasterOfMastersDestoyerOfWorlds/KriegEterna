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

    private bool hasChosenStart;
    private RowEffected chooseNRow;

    private RowEffected chooseNPlayerHand;
    private System.Action<Row, RowEffected, Card> chooseNAction;
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

    Card LastCardPlayed;

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
        List<string> choosePower = new List<string>();
        choosePower.Add("Redemption");
        choosePower.Add("Retreat");
        choosePower.Add("Spy");
        choosePower.Add("Jester");
        List<string> chooseUnit = new List<string>();
        chooseUnit.Add("Scout");
        List<string> chooseKing = new List<string>();
        chooseKing.Add("TerrorKing");
        List<string> chooseUnitGraveyard = new List<string>();
        chooseUnitGraveyard.Add("Crusader");
        chooseUnitGraveyard.Add("Knight");
        List<string> choosePowerGraveyard = new List<string>();
        List<string> enemyPower = new List<string>();
        List<string> enemyUnit = new List<string>();
        List<string> enemyKing = new List<string>();
        deck.buildDeck(4, 9, 1, choosePower, chooseUnit, chooseKing, chooseUnitGraveyard, choosePowerGraveyard, enemyPower, enemyUnit, enemyKing);
        deck.buildTargets();
        hasChosenStart = false;

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

        if (activeDeck.getRowByType(RowEffected.PlayerHand).Count > 0)
            activeCard = player1.getDeck().getRowByType(RowEffected.PlayerHand)[0];

        activeShowingCard = Instantiate(activeCard) as Card;
        activeShowingCard.transform.position = new Vector3(8.96f, 0, -0.1f);

        reorganizeGroup();
    }

    void Update()
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
        Vector3 mouseRelativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseRelativePosition.z = 0f;
        if (Input.GetMouseButtonDown(1))
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
                    c.transform.position = areas.getCenterFrontBig();
                    break;
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && state != State.BLOCKED)
        {
            if (state == State.REVEAL)
            {
                activeDeck.getRowByType(RowEffected.ChooseN).setVisibile(false);
                state = State.FREE;
                return;
            }
            Debug.Log("Click Registered");
            bool clickOnTarget = false;
            Row playerHand = activeDeck.getRowByType(RowEffected.PlayerHand);

            if (playerHand.target.ContainsMouse(mouseRelativePosition) && playerHand.Count > 0 && state != State.MULTISTEP && state != State.CHOOSE_N)
            {
                clickOnTarget = true;
                Debug.Log("Click Registered On Deck");
                for (int i = 0; i < playerHand.Count; i++)
                {
                    Card c = playerHand[i];
                    if (c.ContainsMouse(mouseRelativePosition))
                    {
                        Debug.Log("clicked on: " + c.ToString() + " isPlayable: " + c.isPlayable(activeDeck, player));
                        if (c.isTargetActive() && (CardModel.isUnit(c.cardType) || c.isPlayable(activeDeck, player)))
                        {
                            Play(c, null, null, player);
                            c.setTargetActive(false);
                            if (state == State.MULTISTEP)
                            {
                                activeDeck.disactiveAllInDeck(true);
                                ShowTargets(c, player);
                                break;
                            }
                            break;
                        }
                        activeDeck.disactiveAllInDeck(false);
                        activeCard = c;
                        ShowTargets(c, player);
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
                    Play(activeCard, row, null, player);
                    if (state != State.MULTISTEP)
                    {
                        activeDeck.disactiveAllInDeck(false);
                    }
                    else
                    {
                        activeDeck.disactiveAllInDeck(true);
                        ShowTargets(activeCard, player);
                    }
                }
            }
            for (int i = 0; i < activeDeck.rows.Count; i++)
            {
                Row row = activeDeck.rows[i];
                if (row.cardTargetsActivated)
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        Card selected = row[j];
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
                                Play(activeCard, row, selected, player);
                                if (state != State.MULTISTEP)
                                {
                                    activeDeck.disactiveAllInDeck(false);
                                }
                                else
                                {
                                    ShowTargets(activeCard, player);
                                    activeDeck.disactiveAllInDeck(true);
                                }
                            }
                            clickOnTarget = true;
                        }
                    }
                }
            }

            Debug.Log("Click on target: " + clickOnTarget);
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
        }
    }

    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Debug.Log("Playing: " + c.cardName + " Type: " + c.cardType);
        if (targetRow != null)
        {
            Debug.Log(" TargetRow: " + targetRow.name);
        }
        switch (c.cardType)
        {
            case CardType.Melee: activeDeck.getRowByType(CardModel.getPlayerRow(player, RowEffected.PlayerMelee)).Add(c); break;
            case CardType.Ranged: activeDeck.getRowByType(CardModel.getPlayerRow(player, RowEffected.PlayerRanged)).Add(c); break;
            case CardType.Siege: activeDeck.getRowByType(CardModel.getPlayerRow(player, RowEffected.PlayerSiege)).Add(c); break;
            case CardType.Switch: targetRow.Add(c); break;
            case CardType.Weather: PlayWeather(c); break;
            case CardType.King: PlayKing(c, targetRow, targetCard, player); break;
            default: PlayPower(c, targetRow, targetCard, RowEffected.Player); break;
        }
        updateStateBasedOnCardState(c);
        if (state != State.MULTISTEP)
        {
            activeDeck.getRowByType(RowEffected.PlayerHand).Remove(c);
            if (c.enemyReveal > 0)
            {
                //may want to turn off flashing since it is only otherwise used when some input is happening
                setChooseN(RowEffected.EnemyHand, null, 0, c.enemyReveal, new List<CardType>() { CardType.King, CardType.Melee, CardType.Ranged, CardType.Siege }, RowEffected.None, State.REVEAL);
            }
        }
        reorganizeGroup();
    }
    public void PlayKing(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        if (state == State.MULTISTEP)
        {
            PlayPower(c, targetRow, targetCard, player);
        }
        else
        {
            targetRow.Add(c);
            RowEffected enemySide = CardModel.getPlayerRow(player, RowEffected.EnemyPlayable);
            int cardsRemaining = activeDeck.countCardsInRows(enemySide);
            if (c.setAsideRemain > cardsRemaining)
            {
                c.setAsideRemain = cardsRemaining;
            }
            if (c.enemyCardDestroyRemain > cardsRemaining)
            {
                c.enemyCardDestroyRemain = cardsRemaining;
            }
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
    public void PlayPower(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        RowEffected playerHand = CardModel.getHandRow(player);
        RowEffected enemy = CardModel.getEnemy(player);
        if (targetRow == null && c.rowEffected != RowEffected.None)
        {
            RowEffected rowEffected = CardModel.getPlayerRow(player, c.rowEffected);
            Debug.Log("Playing Card in: " + rowEffected);
            activeDeck.getRowByType(rowEffected).Add(c);
        }
        else if (targetRow != null && targetRow.hasType(RowEffected.Enemy) && c.cardType == CardType.Spy)
        {
            targetRow.Add(c);
            Debug.Log("Added Spy to Row: " + targetRow);
        }
        else if (c.playerCardDestroyRemain > 0)
        {
            c.playerCardDestroyRemain--;
            Row playerHandRow = activeDeck.getRowByType(playerHand);
            if (c.destroyType == DestroyType.Unit)
            {
                activeDeck.sendCardToGraveyard(targetRow, RowEffected.None, targetCard);
            }
            else if (c.destroyType == DestroyType.King)
            {
                Card king = playerHandRow.getKing();
                if (king != null)
                {
                    c.playerCardDrawRemain++;
                    activeDeck.sendCardToGraveyard(playerHandRow, playerHand, king);
                }
                else
                {
                    Row kingRow = activeDeck.getRowByType(activeDeck.getKingRow(player));
                    activeDeck.sendCardToGraveyard(kingRow, RowEffected.None, kingRow[0]);
                }

            }
            else if (c.destroyType == DestroyType.MaxAll)
            {
                List<Row> destroyRows = activeDeck.getRowsByType(c.rowEffected);
                int max = activeDeck.maxStrength(c.rowEffected);
                List<Card> graveyardList = new List<Card>();
                List<Row> graveyardRowList = new List<Row>();
                foreach (Row r in destroyRows)
                {
                    foreach (Card destroyCard in r)
                    {
                        if (destroyCard.strength == max)
                        {
                            graveyardList.Add(destroyCard);
                            graveyardRowList.Add(r);
                        }
                    }
                }
                for (int i = 0; i < graveyardList.Count; i++)
                {
                    activeDeck.sendCardToGraveyard(graveyardRowList[i], RowEffected.None, graveyardList[i]);
                }
                c.playerCardDestroyRemain = 0;
                c.enemyCardDestroyRemain = 0;
            }
            else if (c.destroyType == DestroyType.Max)
            {
                List<Row> destroyRows = activeDeck.getRowsByType(c.rowEffected);
                List<Card> graveyardList = new List<Card>();
                List<Row> graveyardRowList = new List<Row>();
                foreach (Row r in destroyRows)
                {
                    if (r.Count > 0)
                    {
                        graveyardList.Add(r.maxStrengthCard());
                        graveyardRowList.Add(r);
                    }
                }
                for (int i = 0; i < graveyardList.Count; i++)
                {
                    activeDeck.sendCardToGraveyard(graveyardRowList[i], RowEffected.None, graveyardList[i]);
                }
                c.playerCardDestroyRemain = 0;
                c.enemyCardDestroyRemain = 0;
            }
        }
        else if (c.enemyCardDestroyRemain > 0)
        {
            if (c.destroyType == DestroyType.Unit)
            {
                int cardsRemaining = activeDeck.countCardsInRows(c.rowEffected);
                if (c.enemyCardDestroyRemain > cardsRemaining)
                {
                    c.enemyCardDestroyRemain = cardsRemaining - 1;
                }
                else
                {
                    c.enemyCardDestroyRemain--;
                }
                activeDeck.sendCardToGraveyard(targetRow, RowEffected.None, targetCard);
            }
            else
            {
                Debug.Log("REEEE" + c.destroyType);
            }
        }
        else if (c.moveRemain > 0)
        {
            c.moveRemain--;
            activeDeck.addCardToHand(activeDeck.getRowByType(c.moveRow), targetRow.uniqueType, c.moveCard);
        }
        else if (c.playerCardReturnRemain > 0)
        {

            if (c.cardReturnType == CardReturnType.Unit)
            {

                int cardsRemaining = activeDeck.countCardsInRows(c.rowEffected);
                if (c.playerCardReturnRemain > cardsRemaining)
                {
                    c.playerCardReturnRemain = cardsRemaining - 1;
                }
                else
                {
                    c.playerCardReturnRemain--;
                }
                if (c.playerCardReturnRemain <= 0 && c.cardType == CardType.Decoy)
                {
                    int index = targetRow.IndexOf(targetCard);
                    targetRow.Insert(index, c);
                }
                activeDeck.addCardToHand(targetRow, playerHand, targetCard);
            }
            else if (c.cardReturnType == CardReturnType.Move)
            {
                c.playerCardReturnRemain--;
                c.moveRemain++;
                c.moveCard = targetCard;
                targetCard.setTargetActive(false);
                c.moveRow = targetRow.uniqueType;
            }
            else if (c.cardReturnType == CardReturnType.King)
            {
                RowEffected kingLoc = activeDeck.getKingRow(player);
                if (kingLoc != RowEffected.None)
                {
                    c.playerCardReturnRemain--;
                    Row kingRow = activeDeck.getRowByType(kingLoc);
                    activeDeck.addCardToHand(kingRow, playerHand, kingRow[0]);
                }
            }
        }
        else if (c.setAsideRemain > 0)
        {
            int cardsRemaining = activeDeck.countCardsInRows(c.rowEffected);
            if (c.setAsideRemain > cardsRemaining)
            {
                c.setAsideRemain = cardsRemaining - 1;
            }
            else
            {
                c.setAsideRemain--;
            }
            activeDeck.setCardAside(targetRow, targetCard, player);
        }
        else if (c.playerCardDrawRemain > 0)
        {
            c.playerCardDrawRemain--;
            activeDeck.drawCard(targetRow, true);
        }
        else if (c.chooseN > 0)
        {
            Row row = activeDeck.getRowByType(c.chooseRow);
            if (row.Count > 0)
            {
                setChooseN(c.chooseRow, activeDeck.addCardToHand, c.chooseN, c.chooseShowN > 0 ? c.chooseShowN : row.Count, new List<CardType>() { CardType.None }, playerHand, State.CHOOSE_N);
            }
        }
        else if (c.attach)
        {
            targetCard.attachCard(c);
        }
        if (c.doneMultiSelection())
        {
            if (c.rowMultiple > 0)
            {
                if (c.cardType != CardType.King)
                {
                    targetRow.Add(c);
                }
                if (c.rowMultiple == 1 && c.rowEffected == RowEffected.All)
                {
                    activeDeck.clearAllWeatherEffects();
                }
            }
            if (c.enemyCardDrawRemain > 0)
            {
                int cardsDrawn = 0;
                for (int i = 0; i < c.enemyCardDrawRemain; i++)
                {
                    activeDeck.drawCard(activeDeck.getRowByType(RowEffected.UnitDeck), false);
                    cardsDrawn++;
                }
                c.enemyCardDrawRemain -= cardsDrawn;
            }
            if (c.graveyardCardDrawRemain > 0)
            {
                activeDeck.drawCardGraveyard(c, targetCard);
            }
            if (!c.attach && c.cardType == CardType.Power)
            {
                activeDeck.sendCardToGraveyard(activeDeck.getRowByType(playerHand), RowEffected.None, c);
            }
        }
    }

    public void updateStateBasedOnCardState(Card c)
    {
        c.LogSelectionsRemaining();
        if (state == State.CHOOSE_N)
        {
            return;
        }
        if (c.doneMultiSelection())
        {
            Debug.Log("Done with card: " + c.cardName);
            state = State.FREE;
        }
        else
        {
            state = State.MULTISTEP;
        }
        Debug.Log(state);
    }

    public void ShowTargets(Card c, RowEffected player)
    {
        Debug.Log("Showing Targets for: " + c.cardName + " " + c.cardType);
        RowEffected playerPlayable = CardModel.getPlayerRow(player, RowEffected.PlayerPlayable);
        RowEffected playerKing = CardModel.getPlayerRow(player, RowEffected.PlayerKing);
        RowEffected enemyKing = CardModel.getPlayerRow(player, RowEffected.EnemyKing);
        RowEffected enemyPlayable = CardModel.getPlayerRow(player, RowEffected.EnemyPlayable);
        RowEffected enemy = CardModel.getEnemy(player);
        switch (c.cardType)
        {
            case CardType.Melee: c.setTargetActive(true); break;
            case CardType.Ranged: c.setTargetActive(true); break;
            case CardType.Siege: c.setTargetActive(true); break;
            case CardType.Switch:
                activeDeck.getRowByType(CardModel.getPlayerRow(player, RowEffected.PlayerMelee))
                    .setActivateRowCardTargets(true, false);
                activeDeck.getRowByType(CardModel.getPlayerRow(player, RowEffected.PlayerRanged))
                    .setActivateRowCardTargets(true, false);
                break;
            case CardType.King:
                if (state == State.MULTISTEP)
                {
                    if (c.enemyCardDestroy > 0)
                    {
                        activeDeck.activateRowsByType(true, true, enemyPlayable);
                    }
                    else if (c.setAsideRemain > 0)
                    {
                        switch (c.setAsideType)
                        {
                            case SetAsideType.King:
                                activeDeck.activateRowsByType(true, true, playerKing); break;
                            case SetAsideType.EnemyKing:
                                activeDeck.activateRowsByType(true, true, enemyKing); break;
                            case SetAsideType.Enemy:
                                activeDeck.activateRowsByType(true, true, enemy); break;
                            case SetAsideType.Player:
                                activeDeck.activateRowsByType(true, true, player); break;

                        }
                    }
                    else if (c.playerCardDrawRemain > 0)
                    {
                        activeDeck.activateRowsByType(true, false, RowEffected.DrawableDeck);
                    }
                }
                else
                {
                    activeDeck.activateRowsByType(true, false, playerKing);
                }
                break;
            case CardType.Spy:
                if (state == State.MULTISTEP)
                {
                    activeDeck.activateRowsByType(true, false, RowEffected.DrawableDeck);
                }
                else
                {
                    if (c.rowEffected == RowEffected.EnemyPlayable)
                    {
                        activeDeck.activateRowsByType(true, false, enemyPlayable);
                    }
                    else
                    {
                        c.setTargetActive(true);
                    }
                }
            ; break;
            case CardType.Decoy:
                activeDeck.activateRowsByType(true, true, c.rowEffected); break;
            case CardType.Weather: c.setTargetActive(true); break;
            case CardType.Power:
                Debug.Log("Setup Power Targets: " + c.cardName);
                if (c.playerCardDestroyRemain > 0)
                {
                    switch (c.destroyType)
                    {
                        case DestroyType.Unit: activeDeck.activateRowsByType(true, true, playerPlayable); break;
                        default: c.setTargetActive(true); break;
                    }
                    break;
                }
                else if (c.enemyCardDestroyRemain > 0)
                {
                    activeDeck.activateRowsByType(true, true, enemyPlayable);
                }
                else if (c.moveRemain > 0)
                {
                    activeDeck.activateRowsByTypeExclude(true, false, c.rowEffected, c.moveRow);
                }
                else if (c.playerCardReturnRemain > 0)
                {
                    switch (c.cardReturnType)
                    {
                        case CardReturnType.King: activeDeck.activateRowsByType(true, true, activeDeck.getKingRow(player)); break;
                        default: activeDeck.activateRowsByType(true, true, playerPlayable); break;
                    }

                }
                else if (c.setAsideRemain > 0)
                {
                    switch (c.setAsideType)
                    {
                        case SetAsideType.King: activeDeck.activateRowsByType(true, true, playerKing); break;
                        case SetAsideType.EnemyKing: activeDeck.activateRowsByType(true, true, enemyKing); break;
                        case SetAsideType.Enemy: activeDeck.activateRowsByType(true, true, enemy); break;
                        case SetAsideType.Player: activeDeck.activateRowsByType(true, true, player); break;

                    }
                }
                else if (c.playerCardDrawRemain > 0)
                {
                    switch (c.cardDrawType)
                    {
                        case CardDrawType.Either: activeDeck.activateRowsByType(true, false, RowEffected.DrawableDeck); break;
                        case CardDrawType.Unit: activeDeck.activateRowsByType(true, false, RowEffected.UnitDeck); break;
                        case CardDrawType.Power: activeDeck.activateRowsByType(true, false, RowEffected.PowerDeck); break;
                    }
                }
                else if (c.attach)
                {
                    activeDeck.activateRowsByType(true, true, RowEffected.All);
                }
                else if (c.rowEffected != RowEffected.None)
                {
                    if (c.rowEffected == RowEffected.EnemyMax)
                    {
                        activeDeck.activateAllRowsByType(true, false, activeDeck.getMaxScoreRows(enemyPlayable));
                    }
                    activeDeck.activateRowsByType(true, true, c.rowEffected);
                }
                else
                {
                    c.setTargetActive(true);
                }
                break;
            default: break;
        }
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


    public void chooseCard(Card cardClone)
    {

        Debug.Log("Choosing Card: " + cardClone.cardName);
        Row row = activeDeck.getRowByType(chooseNRow);
        Row displayRow = activeDeck.getRowByType(RowEffected.ChooseN);

        displayRow.Remove(cardClone);
        Card realCard = row[row.IndexOf(cardClone)];
        row.chooseNRemain--;
        chooseNAction.Invoke(row, chooseNPlayerHand, realCard);

        cardClone.Destroy();
        reorganizeGroup();

        if (row.chooseNRemain <= 0)
        {
            Debug.Log("Setting Invisible");
            displayRow.setVisibile(false);
            activeDeck.disactiveAllInDeck(false);
            state = State.FREE;
        }
    }

    public void setChooseN(RowEffected chooseRow, System.Action<Row, RowEffected, Card> action, int numChoose, int numShow, List<CardType> exclude, RowEffected playerHand, State newState)
    {

        Row row = activeDeck.getRowByType(chooseRow);
        row.chooseNRemain = numChoose;
        state = newState;

        Debug.Log(state);
        Debug.Log("setting up choice");
        float cardHorizontalSpacing = Card.getBaseWidth() * 1.025f;
        float cardThickness = Card.getBaseThickness();
        float attachmentVerticalSpacing = Card.getBaseHeight() * 0.2f;
        Row displayRow = activeDeck.getRowByType(RowEffected.ChooseN);
        chooseNRow = chooseRow;
        chooseNPlayerHand = playerHand;
        chooseNAction = action;
        while (displayRow.Count > 0)
        {
            Card clone = displayRow[0];
            displayRow.Remove(clone);
            clone.Destroy();

        }
        for (int i = 0; i < numShow && i < row.Count; i++)
        {
            if (!exclude.Contains(row[i].cardType) && (state != State.REVEAL || !row[i].beenRevealed))
            {
                Card clone = Instantiate(row[i]) as Card;
                clone.setVisible(true);
                displayRow.Add(clone);
                if (state == State.REVEAL)
                {
                    row[i].beenRevealed = true;
                }
            }
        }
        reorganizeRow(cardHorizontalSpacing, cardThickness, attachmentVerticalSpacing, displayRow, displayRow.center);
        displayRow.setActivateRowCardTargets(true, true);
    }


    public void reorganizeGroup()
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

    private void reorganizeRow(float cardHorizontalSpacing, float cardThickness, float attachmentVerticalSpacing, Row row, Vector3 centerVector)
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

    /// <summary>
    /// Switch player - update active deck
    /// </summary>
    private void switchPlayer()
    {
        reorganizeGroup();
        state = State.BLOCKED;

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