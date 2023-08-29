using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Card activeCard;
    public static Card lastPlayedCard;
    public static Card enemyLastPlayedCard;
    private static int activePlayerNumber;
    public static State state = State.FREE;

    public static bool testing = false;

    public static Deck activeDeck;

    public static List<Card> roundEndCards;

    public static RoundType round;

    private static bool hasChosenStart;
    private static bool loadingDone;
    private static bool setupComplete;

    public static int turnsLeft;
    public static bool enemyPassed;
    public static bool playerPassed;
    public static int playerRoundsWon;
    public static int enemyRoundsWon;
    public static RowEffected player;
    public static EnemyControllerInterface enemyController;
    private TMP_Text playerNameText;
    private TMP_Text playerPassedText;

    public static int playerTotalScore;
    private TMP_Text playerCardCountText;
    private TMP_Text playerTotalScoreText;
    private TMP_Text playerRoundsWonText;

    private TMP_Text enemyNameText;
    private TMP_Text enemyPassedText;
    private TMP_Text enemyCardCountText;
    private TMP_Text enemyTotalScoreText;
    public static int enemyTotalScore;
    private TMP_Text enemyRoundsWonText;

    private GameObject player1Object;
    private GameObject player2Object;
    private Player player1;
    private Player player2;

    List<Move> moveList;



    private GameObject giveUpButtonObject;
    public static LoadingScreen loadingScreen;

    public static readonly int NUM_POWERS = 4;
    public static readonly int NUM_UNITS = 9;
    public static readonly int NUM_KINGS = 1;
    public static readonly int NUM_DISCARD_START = 3;

    public static int enemyDiscarded = 0;

    public static SteamManager steamManager;
    public static SteamNetworkingTest net;

    List<string> choosePower = new List<string>();
    List<string> chooseUnit = new List<string>();
    List<string> chooseKing = new List<string>();
    List<string> chooseUnitGraveyard = new List<string>();
    List<string> choosePowerGraveyard = new List<string>();
    List<string> enemyPower = new List<string>();
    List<string> enemyUnit = new List<string>();
    List<string> enemyKing = new List<string>();

    List<Card> cardsToLoad;

    GameObject playerInfo;
    GameObject enemyInfo;

    public static Camera shadowCamera;

    public static System.Random random;

    void Awake()
    {
        var totaltime = System.Diagnostics.Stopwatch.StartNew();
        GameObject camera = GameObject.Instantiate(Resources.Load("Prefabs/Main Camera") as GameObject, new Vector3(0f, 0f, -100f), transform.rotation);
        camera.tag = "MainCamera";
        Transform shadowCameraObj = camera.transform.Find("Shadow Camera");
        if (shadowCameraObj != null)
        {
            shadowCamera = shadowCameraObj.GetComponent<Camera>();
            shadowCamera.enabled = false;
        }
        GameObject deckObject = GameObject.Instantiate(Resources.Load("Prefabs/Deck") as GameObject, transform.position, transform.rotation);
        activeDeck = deckObject.GetComponent<Deck>();
        GameObject steamManagerObj = GameObject.Find("SteamManager(Clone)");
        bool makeRand = false;
        if (steamManagerObj == null)
        {
            steamManagerObj = GameObject.Instantiate(Resources.Load("Prefabs/SteamManager") as GameObject, transform.position, transform.rotation);
            makeRand = true;
        }
        steamManager = steamManagerObj.GetComponent<SteamManager>();
        net = steamManager.NetworkingTest;
        if (makeRand)
        {
            net.random = new System.Random();
        }
        random = net.random;

        player2Object = GameObject.Instantiate(Resources.Load("Prefabs/Player") as GameObject, transform.position, transform.rotation);
        player1Object = GameObject.Instantiate(Resources.Load("Prefabs/Player") as GameObject, transform.position, transform.rotation);
        player1 = player1Object.GetComponent<Player>();
        player2 = player2Object.GetComponent<Player>();

        GameObject loadingScreenObj = GameObject.Find("LoadingScreen(Clone)");
        if (loadingScreenObj == null)
        {
            loadingScreenObj = GameObject.Instantiate(Resources.Load("Prefabs/LoadingScreen") as GameObject, new Vector3(0f, 0f, -10f), transform.rotation);
        }
        loadingScreen = loadingScreenObj.GetComponent<LoadingScreen>();


        //Player Info Setup
        playerInfo = GameObject.Instantiate(Resources.Load("Prefabs/PlayerInfo") as GameObject, activeDeck.areas.getPlayerInfoCenterVector(), transform.rotation);

        GameObject playerNameTextObject = playerInfo.transform.Find("PlayerName").gameObject;
        playerNameText = playerNameTextObject.GetComponent<TMP_Text>();
        playerNameText.text = "Player";

        GameObject playerPassedTextObject = playerInfo.transform.Find("Passed").gameObject;
        playerPassedText = playerPassedTextObject.GetComponent<TMP_Text>();

        GameObject playerCardCountTextObject = playerInfo.transform.Find("HandCount").Find("Score").gameObject;
        playerCardCountText = playerCardCountTextObject.GetComponent<TMP_Text>();

        GameObject playerTotalScoreTextObject = playerInfo.transform.Find("TotalScore").Find("Score").gameObject;
        playerTotalScoreText = playerTotalScoreTextObject.GetComponent<TMP_Text>();

        GameObject playerRoundsWonTextObject = playerInfo.transform.Find("RoundsWon").Find("Score").gameObject;
        playerRoundsWonText = playerRoundsWonTextObject.GetComponent<TMP_Text>();

        Areas.scaleToScreenSize(playerInfo.transform);

        //Enemy Info Setup
        enemyInfo = GameObject.Instantiate(Resources.Load("Prefabs/PlayerInfo") as GameObject, activeDeck.areas.getEnemyInfoCenterVector(), transform.rotation);

        GameObject enemyNameTextObject = enemyInfo.transform.Find("PlayerName").gameObject;
        enemyNameText = enemyNameTextObject.GetComponent<TMP_Text>();
        enemyNameText.text = "Enemy";

        GameObject enemyPassedTextObject = enemyInfo.transform.Find("Passed").gameObject;
        enemyPassedText = enemyPassedTextObject.GetComponent<TMP_Text>();

        GameObject enemyCardCountTextObject = enemyInfo.transform.Find("HandCount").Find("Score").gameObject;
        enemyCardCountText = enemyCardCountTextObject.GetComponent<TMP_Text>();

        GameObject enemyTotalScoreTextObject = enemyInfo.transform.Find("TotalScore").Find("Score").gameObject;
        enemyTotalScoreText = enemyTotalScoreTextObject.GetComponent<TMP_Text>();

        GameObject enemyRoundsWonTextObject = enemyInfo.transform.Find("RoundsWon").Find("Score").gameObject;
        enemyRoundsWonText = enemyRoundsWonTextObject.GetComponent<TMP_Text>();
        Areas.scaleToScreenSize(enemyInfo.transform);

        if (!net.isNetworkGame)
        {
            enemyController = new RandomBot();
        }
        else
        {
            enemyController = new NetworkBot();
        }
        hasChosenStart = false;
        setupComplete = false;
        loadingDone = false;
        roundEndCards = new List<Card>();
        round = RoundType.RoundOne;
        turnsLeft = int.MaxValue;
        player = RowEffected.Player;
        MoveLogger.newLogFile();
        MoveLogger.logSeed(net.seed);
        totaltime.Stop();
        Debug.Log("total build Time elapsed: " + totaltime.Elapsed);
    }

    void Start()
    {

        reorganizeGroup();
        /*if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            EFriendFlags fflag = EFriendFlags.k_EFriendFlagAll;
            int numFriends = SteamFriends.GetFriendCount(fflag);
            Debug.Log("****STEAM INFO****");
            for (int i = 0; i < numFriends; i++)
            {
                CSteamID f = SteamFriends.GetFriendByIndex(i, fflag);
                string friendName = SteamFriends.GetFriendPersonaName(f);
                Debug.Log("Friend: " + friendName + " id: " + f);
                FriendGameInfo_t fGame;
                bool playingGame = SteamFriends.GetFriendGamePlayed(f, out fGame);
                if(playingGame){
                    Debug.Log("playing game: " + fGame.m_gameID.AppID());
                }
            }
            Debug.Log(name);
        }*/
    }

    void OnDestroy()
    {
        MoveLogger.flushLogs();
        MoveLogger.closeLogs();
    }

    public void Update()
    {
        // Setting up initial card choice
        // ---------------------------------------------------------------------------------------------------------------
        if (!hasChosenStart)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            activeDeck.buildDeck(NUM_POWERS, NUM_UNITS, NUM_KINGS, choosePower, chooseUnit, chooseKing, chooseUnitGraveyard, choosePowerGraveyard, enemyPower, enemyUnit, enemyKing);
            sw.Stop();
            Debug.Log("buildDeck Time elapsed: " + sw.Elapsed);
            Debug.Log("Startup Time: " + Time.deltaTime);
            hasChosenStart = true;
            state = State.LOADING;
        }

        if (state == State.LOADING)
        {
            if (loadingDone)
            {
                if (loadingScreen.FadeOut())
                {
                    state = State.CHOOSE_N;
                }
            }
            if (cardsToLoad == null)
            {
                cardsToLoad = activeDeck.getVisibleCards(state);
                foreach (Card c in cardsToLoad)
                {
                    Debug.Log("Loading:  " + c.cardName);
                    var co = StartCoroutine(c.loadCardFrontAsync());
                }
            }
            else if (!loadingDone)
            {
                bool allLoaded = true;
                foreach (Card c in cardsToLoad)
                {
                    if (!c.textureLoaded)
                    {
                        allLoaded = false;
                    }
                }
                if (allLoaded)
                {

                    ChooseNController.setChooseN(RowEffected.PlayerHand, CardModel.getRowName(RowEffected.PlayerHand), activeDeck.sendCardToGraveyard, "discard.", NUM_DISCARD_START, activeDeck.getRowByType(RowEffected.PlayerHand).Count, new List<CardType>() { CardType.King }, RowEffected.None, State.CHOOSE_N, false);
                    activeDeck.getRowByType(RowEffected.Pass).setVisibile(false);
                    state = State.LOADING;
                    reorganizeGroup();
                    loadingDone = true;
                }
            }
        }
        else
        {
            if (enemyDiscarded < NUM_DISCARD_START)
            {
                List<Card> discardList = enemyController.ChooseDiscard(NUM_DISCARD_START - enemyDiscarded);
                enemyDiscarded += discardList.Count;
                activeDeck.sendListToGraveyard(discardList, RowEffected.EnemyHand);
                reorganizeGroup();
            }

            enemyCardCountText.text = activeDeck.getRowByType(RowEffected.EnemyHand).Count.ToString();
            playerCardCountText.text = activeDeck.getRowByType(RowEffected.PlayerHand).Count.ToString();

            // Picking card
            // -------------------------------------------------------------- -------------------------------------------------
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
            if (enemyPassed && player == RowEffected.Enemy)
            {
                player = RowEffected.Player;
                Debug.LogError("+++++++++++++++++++++++++++++++++++++++" + player.ToString() + " Turn Round:" + round.ToString() + "+++++++++++++++++++++++++++++++++++++");
            }
            // Doing Enemy Turn
            // ---------------------------------------------------------------------------------------------------------------
            if (!enemyPassed && state != State.BLOCKED && player == RowEffected.Enemy)
            {
                Debug.Log("+++++++++++++++++++++++++++++++++++++++ Enemy Turn " + state + "+++++++++++++++++++++++++++++++++++++");

                moveList = Move.getPossibleMoves(player);

                if (moveList != null)
                {
                    if (moveList.Count == 0)
                    {
                        Debug.LogError("reee I have no moves");
                    }
                    Debug.Log("Enemy Hand: " + activeDeck.getRowByType(RowEffected.EnemyHand) + " size: " + moveList.Count);
                    foreach (Move m in moveList)
                    {
                        Debug.Log("\t" + m);
                    }
                }

                Move nextMove = enemyController.NextMove(moveList);
                Debug.Log(nextMove);
                if (nextMove != null)
                {
                    if (nextMove.isButton)
                    {
                        activeDeck.getRowByType(nextMove.targetRow).buttonAction.Invoke();
                        MoveLogger.logButtonPress(player, nextMove.targetRow);
                    }
                    else if (nextMove.activate)
                    {
                        activeCard = nextMove.c;
                        TargetController.ShowTargets(nextMove);
                        state = State.ACTIVE_CARD;
                    }
                    else
                    {
                        if (state == State.CHOOSE_N)
                        {
                            ChooseNController.chooseCard(nextMove.targetCard, player);
                        }
                        else
                        {
                            PlayController.Play(nextMove);
                            TargetController.ShowTargets(nextMove);
                        }

                        scoreAndUpdate();
                        if (state == State.REVEAL)
                        {
                            state = State.FREE;
                        }
                    }
                    if (state == State.FREE)
                    {
                        turnOver();
                    }

                    activeCard.LogSelectionsRemaining();
                    moveList = null;
                }
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && state != State.BLOCKED && !playerPassed)
            {

                Debug.Log("--------------------------------------------------------------------");
                if (state == State.REVEAL)
                {
                    activeDeck.getRowByType(RowEffected.PlayerChooseN).setVisibile(false);
                    state = State.FREE;
                    return;
                }
                Debug.Log("Click Registered! State: " + state + " player: " + player);
                bool clickOnTarget = false;
                Row playerHand = activeDeck.getRowByType(RowEffected.PlayerHand);

                if (playerHand.Count > 0 && state != State.MULTISTEP && state != State.CHOOSE_N && player == RowEffected.Player)
                {
                    for (int i = 0; i < playerHand.Count; i++)
                    {
                        Card c = playerHand[i];
                        if (c.ContainsMouse(mouseRelativePosition))
                        {

                            Debug.Log("Click Registered On Deck");
                            clickOnTarget = true;
                            bool canPlay = (CardModel.isUnit(c.cardType) || c.isPlayable(player));
                            Debug.Log("clicked on: " + c.ToString() + " isPlayable: " + c.isPlayable(player) + " active: " + c.isTargetActive());
                            if (c.isTargetActive() && canPlay)
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
                            else if (canPlay)
                            {
                                activeDeck.disactiveAllInDeck(false);
                                activeCard = c;
                                TargetController.ShowTargets(c, player);
                                if (net.isNetworkGame)
                                {
                                    Move move = new Move(c, null, RowEffected.None, player, false, true);
                                    net.sendNextMessage(PacketType.MOVE, move.id);
                                }
                                Debug.Log("Setting Card Active: " + c.cardName);
                                activeCard.setBaseLoc();
                                activeCard.transform.position += new Vector3(0, Card.getBaseHeight() / 3, 0f);
                                state = State.ACTIVE_CARD;
                            }
                            else
                            {
                                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!Cannot Play!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                clickOnTarget = false;
                            }
                        }
                    }
                }
                if (!clickOnTarget && player == RowEffected.Player)
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
                if (!clickOnTarget && player == RowEffected.Player)
                {
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
                            Debug.Log("Button: " + row.uniqueType + " Clicked!");
                            row.buttonAction.Invoke();
                            clickOnTarget = true;
                            MoveLogger.logButtonPress(row.uniqueType, player);
                            if (net.isNetworkGame)
                            {
                                Move move = new Move(null, null, row.uniqueType, player, true, false);
                                net.sendNextMessage(PacketType.MOVE, move.id);
                            }
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
                        if (activeCard != null)
                        {
                            activeCard.LogSelectionsRemaining();
                            if (activeCard.doneMultiSelection(player))
                            {
                                state = State.FREE;
                            }
                        }
                    }
                    reorganizeGroup();
                }
                else
                {

                    scoreAndUpdate();
                    if (!setupComplete && state == State.FREE)
                    {
                        setupComplete = true;
                    }
                    else if (state == State.FREE || activeDeck.getRowByType(CardModel.getHandRow(player)).Count == 0)
                    {
                        turnOver();
                    }
                }
            }
        }
    }

    private void turnOver()
    {
        Debug.Log("TURN OVER: " + playerPassed + " enemypassed: " + enemyPassed);
        MoveLogger.logTurnOver(player);
        activeDeck.getRowByType(RowEffected.Skip).setVisibile(false);
        if (player == RowEffected.Player)
        {
            activeDeck.getRowByType(RowEffected.Pass).setVisibile(true);
        }
        if (turnsLeft != int.MaxValue && turnsLeft > 0)
        {
            turnsLeft--;
            Debug.Log("Turns Left: " + turnsLeft);
        }
        if ((enemyPassed && playerPassed) || turnsLeft == 0)
        {
            Debug.Log("Round Over");
            enemyPassed = false;
            playerPassed = false;
            turnsLeft = int.MaxValue;
            List<Row> setAsideRows = activeDeck.getRowsByType(RowEffected.SetAside);
            foreach (Row row in setAsideRows)
            {
                foreach (Card c in row)
                {
                    Debug.Log("Returning: " + c.cardName + " to Hand: " + c.setAsideReturnRow);
                    Row returnRow = activeDeck.getRowByType(c.setAsideReturnRow);
                    returnRow.Add(c);
                }
                row.RemoveAll(delegate (Card a) { return true; });
            }

            state = State.ROUND_END;
            foreach (Card c in roundEndCards)
            {
                PlayController.Play(c, activeDeck.getCardRow(c), null, c.playerPlayed);
            }
            //Score
            updateScores();
            bool playerWon = false;
            bool draw = false;
            if (enemyTotalScore < playerTotalScore)
            {
                playerRoundsWon++;
                playerWon = true;
            }
            else if (playerTotalScore < enemyTotalScore)
            {
                enemyRoundsWon++;
                playerWon = false;
            }
            else
            {
                draw = true;
                //TODO: need to make tie breaker or go back to prev round
            }
            round = nextRound(round);
            StartCoroutine(loadingScreen.roundTextFlash(round, playerWon, draw));
            if (round != RoundType.GameFinished)
            {
                state = State.FREE;
            }
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
            reorganizeGroup();
            scoreAndUpdate();
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
        else if (!enemyPassed && playerPassed)
        {
            player = RowEffected.Enemy;
        }
        playerInfoTextUpdate();
    }
    public static void playerPass()
    {
        if (player == RowEffected.Player)
        {
            playerPassed = true;
        }
        else
        {
            enemyPassed = true;
        }
    }

    public static void skipActiveCardEffects()
    {
        activeCard.zeroSkipSelectionCounts();
        if (state == State.CHOOSE_N)
        {
            ChooseNController.endChooseN(activeDeck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerChooseN)), player);
        }
        else if (CardModel.isUnit(activeCard.cardType) && activeDeck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerHand)).Contains(activeCard))
        {
            PlayController.Play(activeCard, activeDeck.getRowByType(CardModel.getPlayableRow(player, activeCard.cardType)), null, player);
        }
        if (activeCard.doneMultiSelection(player))
        {
            state = State.FREE;
            activeDeck.disactiveAllInDeck(false);
        }
        else
        {
            activeDeck.disactiveAllInDeck(true);
            TargetController.ShowTargets(activeCard, player);
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
            case RoundType.RoundTwo: return Math.Abs(playerRoundsWon - enemyRoundsWon) > 1 ? RoundType.GameFinished : RoundType.FinalRound;
            default: return RoundType.GameFinished;
        }
    }

    internal static bool canPass()
    {
        return state == State.FREE;
    }
    internal static bool canSkip()
    {
        return (state == State.MULTISTEP || state == State.CHOOSE_N) && activeCard.canSkip();
    }

    private void updateScores()
    {
        activeDeck.scoreRows(RowEffected.All);
        enemyTotalScore = activeDeck.totalScore(RowEffected.Enemy);
        playerTotalScore = activeDeck.totalScore(RowEffected.Player);
    }
    private void playerInfoTextUpdate()
    {
        enemyTotalScoreText.text = enemyTotalScore.ToString();
        enemyRoundsWonText.text = enemyRoundsWon.ToString();
        enemyPassedText.text = "Passed: " + enemyPassed;
        playerTotalScoreText.text = playerTotalScore.ToString();
        playerRoundsWonText.text = playerRoundsWon.ToString();
        playerPassedText.text = "Passed: " + playerPassed;
    }
    private void scoreAndUpdate()
    {
        updateScores();
        playerInfoTextUpdate();
    }
    public static void battleOver()
    {
        if (net.isNetworkGame)
        {
            SceneManager.LoadSceneAsync("menuScene");
        }
        else
        {
            //TODO Score screen, damage loot etc.
            //probably want to keep the cards in memory
            //Should we start loading them on the front menu?
            SceneManager.LoadSceneAsync("menuScene");
        }
    }

    internal static Card getLastCardPlayed(RowEffected player)
    {
        if (player == RowEffected.Enemy)
        {
            return enemyLastPlayedCard;
        }
        else
        {
            return lastPlayedCard;
        }
    }
    internal static void setLastCardPlayed(Card c, RowEffected player)
    {
        if (player == RowEffected.Enemy)
        {
            enemyLastPlayedCard = c;
        }
        else
        {
            lastPlayedCard = c;
        }
    }
}