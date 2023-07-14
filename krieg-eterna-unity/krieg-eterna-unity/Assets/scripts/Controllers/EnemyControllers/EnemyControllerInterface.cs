using System;
using System.Collections.Generic;
public interface EnemyControllerInterface
{
    public Move NextMove(List<Move> possibleMoves) => throw new NotImplementedException();
    public List<Card> ChooseDiscard(int numDiscard) => throw new NotImplementedException();

}

public class Move
{
    public Card c;
    public Card targetCard;
    public RowEffected targetRow;
    public RowEffected player;
    public bool executedMove;

    public bool isButton;

    public Move(Card c, Card targetCard, RowEffected targetRow, RowEffected player, bool isButton)
    {
        this.c = c;
        this.targetCard = targetCard;
        this.targetRow = targetRow;
        this.player = player;
        executedMove = false;
        this.isButton = isButton;
    }

    public override string ToString(){
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
                moveList.Add(new Move(c, null, hand.uniqueType, player, false));
            }
        }
        else if (state == State.MULTISTEP || state == State.ACTIVE_CARD)
        {
            List<Tuple<Card, RowEffected>> possibleTargets = deck.getActiveCardTargetsAndRows();
            foreach (Tuple<Card, RowEffected> pair in possibleTargets)
            {
                moveList.Add(new Move(activeCard, pair.Item1, pair.Item2, player, false));
            }
            List<Row> possibleRowTargets = deck.getActiveRowTargets();
            foreach (Row r in possibleRowTargets)
            {
                moveList.Add(new Move(activeCard, null, r.uniqueType, player, false));
            }
        }
        else if (state == State.CHOOSE_N)
        {
            Row chooseRow = deck.getRowByType(ChooseNController.chooseNRow);
            foreach (Card c in chooseRow)
            {
                moveList.Add(new Move(activeCard, c, CardModel.getRowFromSide(player, RowEffected.PlayerChooseN), player, false));
            }
        }
        List<Row> buttonRows = deck.getRowsByType(RowEffected.Button);
        foreach (Row button in buttonRows)
        {
            if (button.isButtonClickable(player))
            {
                moveList.Add(new Move(null, null, button.uniqueType, player, true));
            }
        }
        return moveList;
    }
}