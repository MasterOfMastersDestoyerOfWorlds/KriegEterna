using System.Collections.Generic;
using UnityEngine;
using System;
public class ChooseNController : EffectControllerInterface
{

    public static RowEffected chooseNRow;
    private static RowEffected chooseNSendRow;
    private static System.Action<Row, RowEffected, Card> chooseNAction;
    private static Coroutine flashCoroutine;
    private static string displayText;

    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Setting up Choose N");

        RowEffected chooseRow = CardModel.getRowFromSide(player, c.chooseRow);
        Row row = deck.getRowByType(chooseRow);
        if (row.Count > 0)
        {
            RowEffected sendRow = CardModel.getRowFromSide(player, c.rowEffected);
            Action<Row, RowEffected, Card> chooseAction = null;
            switch (c.chooseNAction)
            {
                case ChooseNAction.AddHand: chooseAction = deck.addCardToHand; break;
                case ChooseNAction.SendGraveyard: chooseAction = deck.sendCardToGraveyard; break;
                case ChooseNAction.SendGraveyardMultiply: chooseAction = deck.sendCardToGraveyardMultiply; break;
                case ChooseNAction.SetAside: chooseAction = deck.setCardAside; break;
            }


            setChooseN(chooseRow, CardModel.getRowName(chooseRow), chooseAction, "add to your hand.", c.chooseN, c.chooseShowN > 0 ? c.chooseShowN : row.Count, CardModel.chooseToCardTypeExclude(c.chooseCardType), sendRow, State.CHOOSE_N, true);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.chooseNRemain > 0;
    }

    public void Target(Card c, RowEffected player)
    {
        c.setTargetActive(true);
    }

    public bool TargetCondition(Card c, RowEffected player)
    {
        return c.chooseNRemain > 0;
    }

    public static void endChooseN(Row displayRow, RowEffected player)
    {
        Deck activeDeck = Game.activeDeck;
        Card activeCard = Game.activeCard;
        activeDeck.getRowByType(RowEffected.Skip).setVisibile(false);
        displayRow.setVisibile(false);
        Game.shadowCamera.enabled = false;
        activeDeck.disactiveAllInDeck(false);
        if (flashCoroutine != null)
        {
            Game.loadingScreen.stopDisplayTextFlash(flashCoroutine);
            flashCoroutine = null;
        }
        if (activeCard == null || activeCard.doneMultiSelection(player))
        {
            Game.state = State.FREE;
        }
        else
        {
            Game.state = State.MULTISTEP;
            TargetController.ShowTargets(activeCard, player);
        }
    }

    public static void setChooseN(RowEffected chooseRow, string rowName, System.Action<Row, RowEffected, Card> action, string actionName, int numChoose, int numShow, List<CardType> exclude, RowEffected sendRow, State newState, bool skipable)
    {

        Deck activeDeck = Game.activeDeck;
        Card activeCard = Game.activeCard;
        Row row = activeDeck.getRowByType(chooseRow);
        row.chooseNRemain = row.Count > numChoose ? numChoose : row.Count;
        Game.state = newState;
        RowEffected player = Game.player;
        if (player == RowEffected.Player)
        {
            Game.shadowCamera.enabled = true;
            if (rowName.Length > 0)
            {
                flashCoroutine = Game.loadingScreen.displayTextFlash();
                displayText = "Choose {0} cards from " + rowName + " to " + actionName;
                Game.loadingScreen.displayRowText.text = string.Format(displayText, numChoose);
            }
        }

        Debug.Log(Game.state);
        Debug.Log("setting up choice");
        float cardHorizontalSpacing = Card.getBaseWidth() * 1.025f;
        float cardThickness = Card.getBaseThickness();
        float attachmentVerticalSpacing = Card.getBaseHeight() * 0.2f;
        Row displayRow = activeDeck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerChooseN));
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
        Debug.Log(row.Count + " " + row + " " + numShow);
        for (int i = 0; revealed < numShow && i < row.Count; i++)
        {

            Debug.Log("Revealing: " + row[i].cardName + " " + row[i].cardType);
            if (!exclude.Contains(row[i].cardType))
            {
                Card clone = GameObject.Instantiate(row[i]) as Card;
                clone.setVisible(true);
                if (!clone.textureLoaded)
                {
                    clone.loadCardFront();
                }
                clone.setLayer("Display", true);
                displayRow.Add(clone);
                revealed++;
                if (Game.state == State.REVEAL)
                {
                    Debug.Log("Revealing: " + clone.cardName);
                    row[i].beenRevealed = true;
                }
            }
        }
        if (skipable)
        {
            activeDeck.getRowByType(RowEffected.Skip).setVisibile(true);
        }
        Game.reorganizeRow(cardHorizontalSpacing, cardThickness, attachmentVerticalSpacing, displayRow, displayRow.center);
        displayRow.setActivateRowCardTargets(true, true, false);
    }

    public static void chooseCard(Card cardClone, RowEffected player)
    {
        Deck activeDeck = Game.activeDeck;
        Card activeCard = Game.activeCard;
        Debug.Log("Choosing Card: " + cardClone.cardName);
        MoveLogger.logChooseCard(cardClone, player);
        Row row = activeDeck.getRowByType(chooseNRow);
        Row displayRow = activeDeck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerChooseN));

        displayRow.Remove(cardClone);

        Card realCard = row[row.IndexOf(cardClone)];
        if (activeCard != null)
        {
            activeCard.chooseNRemain--;
        }
        row.chooseNRemain--;
        if (flashCoroutine != null)
        {
            Game.loadingScreen.displayRowText.text = string.Format(displayText, row.chooseNRemain);
        }
        chooseNAction.Invoke(row, chooseNSendRow, realCard);

        Debug.Log("Choose Row result : " + activeDeck.getRowByType(chooseNRow));
        Debug.Log("hand result : " + activeDeck.getRowByType(CardModel.getHandRow(player)));

        cardClone.Destroy();
        Game.reorganizeGroup();

        if (row.chooseNRemain <= 0)
        {
            endChooseN(displayRow, player);
        }
    }
}