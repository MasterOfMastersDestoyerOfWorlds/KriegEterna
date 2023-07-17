using UnityEngine;
using System.Collections.Generic;
public class TestBot : EnemyControllerInterface
{
    public string nextMoveName = "";
    public Move NextMove(List<Move> possibleMoves)
    {
        foreach (Move m in possibleMoves)
        {
            if (m.name == nextMoveName)
            {
                Debug.Log("!!!!!!Choosing Move: " + m);
                return m;
            }
        }
        Debug.Log("!!!!!!!!!!!!!!!!Move : " + nextMoveName + " not found in Moves: " + Move.getPossibleMoves(RowEffected.Enemy).Count);
        return null;
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