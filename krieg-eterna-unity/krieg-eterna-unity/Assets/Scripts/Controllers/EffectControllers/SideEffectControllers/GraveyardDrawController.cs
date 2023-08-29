using UnityEngine;
public class GraveyardDrawController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Graveyard Card Draw");
        deck.drawCardGraveyard(c, targetCard, CardModel.getHandRow(player));
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.graveyardCardDrawRemain > 0;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}