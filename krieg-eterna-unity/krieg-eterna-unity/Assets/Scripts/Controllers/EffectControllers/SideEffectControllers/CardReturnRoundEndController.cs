using UnityEngine;
public class CardReturnRoundEndController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Return Card Round End");
        Row handRow = deck.getRowByType(CardModel.getHandRow(CardModel.getPlayerFromRow(targetRow.uniqueType)));
        targetRow.Remove(c);
        handRow.Add(c);

    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.cardReturnType == CardReturnType.RoundEnd && Game.state == State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}