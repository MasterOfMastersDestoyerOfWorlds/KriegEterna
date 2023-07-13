using System;
using System.Collections.Generic;
public interface EnemyControllerInterface
{
    public Move NextMove(List<Move> possibleMoves) => throw new NotImplementedException();

}

public class Move
{
    Card targetCard;
    RowEffected targetRow;
    RowEffected player;
    bool executedMove;

    public Move(Card targetCard, RowEffected targetRow, RowEffected player)
    {
        this.targetCard = targetCard;
        this.targetRow = targetRow;
        this.player = player;
        executedMove = false;
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
        if (state == State.FREE)
        {
            Row hand = deck.getRowByType(CardModel.getHandRow(player));
            foreach (Card c in hand)
            {
                moveList.Add(new Move(c, hand.uniqueType, player));
            }
        }
        else if (state == State.MULTISTEP || state == State.ACTIVE_CARD)
        {
            List<Tuple<Card, RowEffected>> possibleTargets = deck.getActiveCardTargetsAndRows();
            foreach (Tuple<Card, RowEffected> pair in possibleTargets)
            {
                moveList.Add(new Move(pair.Item1, pair.Item2, player));
            }
            List<Row> possibleRowTargets = deck.getActiveRowTargets();
            foreach (Row r in possibleRowTargets)
            {
                moveList.Add(new Move(null, r.uniqueType, player));
            }
        }
        else if (state == State.CHOOSE_N)
        {
            Row chooseRow = deck.getRowByType(ChooseNController.chooseNRow);
            foreach (Card c in chooseRow)
            {
                moveList.Add(new Move(c, RowEffected.ChooseN, player));
            }
        }
        List<Row> buttonRows = deck.getRowsByType(RowEffected.Button);
        foreach (Row button in buttonRows)
        {
            if (button.isButtonClickable(player))
            {
                moveList.Add(new Move(null, button.uniqueType, player));
            }
        }
        return moveList;
    }
}