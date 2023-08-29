using UnityEngine;
public class EnemyDrawController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        RowEffected enemy = CardModel.getEnemy(player);
        Deck deck = Game.activeDeck;
        //TODO: hook this into enemy ai
        Debug.Log("Enemy Card Draw");
        int cardsDrawn = 0;
        for (int i = 0; i < c.enemyCardDrawRemain; i++)
        {
            deck.drawCard(deck.getRowByType(RowEffected.UnitDeck), enemy);
            cardsDrawn++;
        }
        c.enemyCardDrawRemain -= cardsDrawn;
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.enemyCardDrawRemain > 0;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}