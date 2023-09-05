using UnityEngine;
public class SwitchSidesRoundEndController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Switch Sides Round End");
        Row oppositeRow = deck.getRowByType(CardModel.getRowFromSide(RowEffected.Enemy, targetRow.uniqueType));
        Debug.Log("Swaping Rows: " + c + " from row: " + targetRow + " to row" + oppositeRow);
        targetRow.Remove(c);
        oppositeRow.Add(c);
        Debug.Log("Swaping Rows: " + c + " from row: " + targetRow + " to row" + oppositeRow);
        c.playerCardReturnRemain = 0;

    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.cardReturnType == CardReturnType.SwitchSidesRoundEnd && Game.state == State.ROUND_END;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}