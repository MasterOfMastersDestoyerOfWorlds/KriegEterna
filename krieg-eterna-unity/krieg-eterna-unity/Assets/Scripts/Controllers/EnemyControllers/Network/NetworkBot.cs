using UnityEngine;
using System.Collections.Generic;
public class NetworkBot : EnemyControllerInterface
{
    List<(PacketType, int)> pendingMoves = new List<(PacketType, int)>();
    public Move NextMove(List<Move> possibleMoves)
    {
        (PacketType, int) message = Game.steamManager.NetworkingTest.getNextMessage();
        PacketType type = message.Item1;
        int moveId = message.Item2;
        if(type == PacketType.MOVE){
            foreach (Move m in possibleMoves)
            {
                if (m.id == moveId)
                {
                    Debug.Log("!!!!!!Choosing Move: " + m);
                    return m;
                }
            }
            Debug.Log("!!!!!!!!!!!!!!!!Move : " + moveId + " not found in Moves: " + Move.getPossibleMoves(RowEffected.Enemy).Count);
        }
        else{
            Debug.Log("F, not right state");
            pendingMoves.Add(message);
        }
        return null;
    }
    public List<Card> ChooseDiscard(int numDiscard)
    {
        (PacketType, int) message = Game.steamManager.NetworkingTest.getNextMessage();
        PacketType type = message.Item1;
        int discardId = message.Item2;
        List<Card> discard = new List<Card>();
        if(type == PacketType.DISCARD){
            Row hand = Game.activeDeck.getRowByType(CardModel.getHandRow(RowEffected.Enemy));
            foreach (Card c in hand)
            {
                if (c.index == discardId)
                {
                    discard.Add(c);      
                    Debug.Log("Discarding Card: " + c.cardName);
                    return discard;
                }
            }
            Debug.Log("Card not found in Enemy Hand: " + discardId);
        }
        else{
            Debug.Log("F, not right state");
            pendingMoves.Add(message);
        }
        return discard;
    }
}