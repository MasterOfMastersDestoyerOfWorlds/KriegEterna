using UnityEngine;
using System.Collections.Generic;
public class RandomBot : EnemyControllerInterface
{
    public Move NextMove(List<Move> possibleMoves)
    {
        return possibleMoves[Random.Range(0, possibleMoves.Count)];
    }
    public List<Card> ChooseDiscard(int numDiscard)
    {
        Row hand = Game.activeDeck.getRowByType(CardModel.getHandRow(RowEffected.Enemy));
        List<int> indexes = new List<int>();
        for (int i = 0; i < hand.Count; i++)
        {
            indexes.Add(i);
        }
        List<Card> listDiscard = new List<Card>();
        for (int i = 0; i < numDiscard; i++)
        {
            int idx = Random.Range(0, indexes.Count);
            int rIdx = indexes[idx];
            indexes.RemoveAt(idx);
            listDiscard.Add(hand[rIdx]);
        }
        return listDiscard;
    }
}