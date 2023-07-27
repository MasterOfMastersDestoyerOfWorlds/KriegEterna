using UnityEngine;
using System.Collections.Generic;
public class RandomBot : EnemyControllerInterface
{
    public Move NextMove(List<Move> possibleMoves)
    {
        if(possibleMoves.Count == 0){
            return new Move(null, null, RowEffected.Pass, RowEffected.Enemy, true, false);
        }
        if(Game.state == State.FREE && Game.playerPassed && Game.playerTotalScore < Game.enemyTotalScore){
            return new Move(null, null, RowEffected.Pass, RowEffected.Enemy, true, false);
        }
        return possibleMoves[Game.random.Next(possibleMoves.Count)];
    }
    public List<Card> ChooseDiscard(int numDiscard)
    {
        Row hand = Game.activeDeck.getRowByType(CardModel.getHandRow(RowEffected.Enemy));
        List<int> indexes = new List<int>();
        for (int i = 0; i < hand.Count; i++)
        {
            if(hand[i].cardType != CardType.King){
                indexes.Add(i);
            }
        }
        List<Card> listDiscard = new List<Card>();
        for (int i = 0; i < numDiscard; i++)
        {
            int idx = Game.random.Next(indexes.Count);
            int rIdx = indexes[idx];
            indexes.RemoveAt(idx);
            listDiscard.Add(hand[rIdx]);
        }
        return listDiscard;
    }
}