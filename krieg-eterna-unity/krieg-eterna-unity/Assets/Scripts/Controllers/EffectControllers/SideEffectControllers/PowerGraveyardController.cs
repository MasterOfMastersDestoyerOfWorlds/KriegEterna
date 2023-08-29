using UnityEngine;
public class PowerGraveyardController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerHand = CardModel.getHandRow(player);
        Debug.Log("Sending Card to Graveyard");
        deck.sendCardToGraveyard(deck.getRowByType(playerHand), RowEffected.None, c);
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return !c.attach && !c.playInRow && !Game.activeDeck.getRowByType(RowEffected.PowerGraveyard).Contains(c);
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}