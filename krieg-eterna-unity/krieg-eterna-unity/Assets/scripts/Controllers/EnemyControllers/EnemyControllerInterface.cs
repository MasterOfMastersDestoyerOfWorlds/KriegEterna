using System;
using System.Collections.Generic;
using UnityEngine;
public interface EnemyControllerInterface
{
    public Move NextMove(List<Move> possibleMoves) => throw new NotImplementedException();
    public List<Card> ChooseDiscard(int numDiscard) => throw new NotImplementedException();

}

public class Move
{
    public string name;
    public Card c;
    public Card targetCard;
    public RowEffected targetRow;
    public RowEffected player;
    public bool executedMove;

    public bool isButton;
    public bool activate;

    public Move(Card c, Card targetCard, RowEffected targetRow, RowEffected player, bool isButton, bool activate)
    {
        if (targetCard == null && targetRow == RowEffected.None)
        {
            name = c.cardName;
        }
        else if (targetCard != null)
        {
            name = targetCard.cardName;
        }
        else if (targetRow != RowEffected.None)
        {
            name = System.Enum.GetName(typeof(RowEffected), targetRow);
        }
        this.c = c;
        this.targetCard = targetCard;
        this.targetRow = targetRow;
        this.player = player;
        executedMove = false;
        this.activate = activate;
        this.isButton = isButton;
    }

    public override string ToString()
    {
        return "ActiveCard: " + c + " targetCard: " + targetCard + " targetRow: " + targetRow + " player: " + player + " executedMove? " + executedMove;
    }

    public static bool movePossible(Move m)
    {
        Deck deck = Game.activeDeck;
        State state = Game.state;
        Row row = deck.getRowByType(m.targetRow);
        if (state == State.MULTISTEP || state == State.ACTIVE_CARD)
        {
            List<Card> possibleTargets = deck.getActiveCardTargets();
            if (possibleTargets.Contains(m.targetCard))
            {
                return true;
            }
            List<Row> possibleRowTargets = deck.getActiveRowTargets();
            if (possibleRowTargets.Exists((Row r) => r.isTypeUnique(m.targetRow)))
            {
                return true;
            }
        }
        else if (state == State.CHOOSE_N)
        {
            Row chooseRow = deck.getRowByType(ChooseNController.chooseNRow);
            if (chooseRow.Contains(m.targetCard))
            {
                return true;
            }
        }
        else if (row.isButton && row.isButtonClickable(m.player))
        {
            return true;
        }
        return false;
    }
    public static List<Move> getPossibleMoves(RowEffected player)
    {
        List<Move> moveList = new List<Move>();
        Deck deck = Game.activeDeck;
        State state = Game.state;
        Card activeCard = Game.activeCard;
        if (state == State.FREE)
        {
            Row hand = deck.getRowByType(CardModel.getHandRow(player));
            foreach (Card c in hand)
            {
                moveList.Add(new Move(c, null, RowEffected.None, player, false, true));
            }
        }
        else if (state == State.MULTISTEP || state == State.ACTIVE_CARD)
        {
            List<Tuple<Card, RowEffected>> possibleTargets = deck.getActiveCardTargetsAndRows();
            foreach (Tuple<Card, RowEffected> pair in possibleTargets)
            {
                RowEffected row = pair.Item2;
                if (activeCard.Equals(pair.Item1))
                {
                    row = RowEffected.None;
                }
                moveList.Add(new Move(activeCard, pair.Item1, row, player, false, false));
            }
            List<Row> possibleRowTargets = deck.getActiveRowTargets();
            foreach (Row r in possibleRowTargets)
            {
                moveList.Add(new Move(activeCard, null, r.uniqueType, player, false, false));
            }
        }
        else if (state == State.CHOOSE_N)
        {
            RowEffected rowtype = CardModel.getRowFromSide(player, RowEffected.PlayerChooseN);
            Row virtualChooseRow = deck.getRowByType(rowtype);
            foreach (Card c in virtualChooseRow)
            {
                moveList.Add(new Move(activeCard, c, rowtype, player, false, false));
            }
        }
        List<Row> buttonRows = deck.getRowsByType(RowEffected.Button);
        foreach (Row button in buttonRows)
        {
            if (button.isButtonClickable(player))
            {
                moveList.Add(new Move(null, null, button.uniqueType, player, true, false));
            }
        }
        return moveList;
    }
}