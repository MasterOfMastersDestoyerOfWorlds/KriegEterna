using UnityEngine;
public class AutoDrawController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Card Draw type:" + c.cardDrawType + " amount: " + c.playerCardDrawRemain);
        if (c.cardDrawType == CardDrawType.Unit || (c.cardDrawType == CardDrawType.RoundEnd && Game.state == State.ROUND_END))
        {
            int cardsDrawn = 0;
            for (int i = 0; i < c.playerCardDrawRemain; i++)
            {
                deck.drawCard(deck.getRowByType(RowEffected.UnitDeck), player);
                cardsDrawn++;
            }
            c.playerCardDrawRemain -= cardsDrawn;
        }
        else if (c.cardDrawType == CardDrawType.RoundEnd)
        {
            Game.roundEndCards.Add(c);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.playerCardDrawRemain > 0;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}