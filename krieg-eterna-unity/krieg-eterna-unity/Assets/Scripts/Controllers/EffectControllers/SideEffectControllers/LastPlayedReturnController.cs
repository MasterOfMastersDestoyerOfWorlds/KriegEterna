using UnityEngine;
public class LastPlayedReturnController : EffectControllerInterface
{
    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected playerHand = CardModel.getHandRow(player);
        c.playerCardReturnRemain--;
        Card lastPlayed = Game.getLastCardPlayed(player);

        if (lastPlayed != null)
        {
            //Todo: separate enemy and player last card played
            Row lastPlayedRow = deck.getCardRow(lastPlayed);
            int index = lastPlayedRow.IndexOf(lastPlayed);
            if (index < 0 || index > lastPlayedRow.Count)
            {
                Debug.Log("REEEE: row:" + lastPlayedRow + " card:" + targetCard.cardName);
                
                lastPlayedRow.Add(c);
            }
            else{
                lastPlayedRow.Insert(index, c);
            }
            deck.getRowByType(playerHand).Remove(c);
            deck.addCardToHand(lastPlayedRow, playerHand, lastPlayed);
        }

    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        /* Debug.Log("LastPlayedReturnController: return: " + c.playerCardReturnRemain);
         Debug.Log("LastPlayedReturnController: returntype: " + c.cardReturnType);
         Debug.Log("LastPlayedReturnController: lastCard: " + Game.getLastCardPlayed(player));
         Debug.Log("LastPlayedReturnController: " + (c.playerCardReturnRemain > 0 && c.cardReturnType == CardReturnType.LastPlayedCard && Game.getLastCardPlayed(player) != null));*/
        return c.playerCardReturnRemain > 0 && c.cardReturnType == CardReturnType.LastPlayedCard && Game.getLastCardPlayed(player) != null;
    }
    public bool IsSideEffect(Card c, RowEffected player) => true;
}