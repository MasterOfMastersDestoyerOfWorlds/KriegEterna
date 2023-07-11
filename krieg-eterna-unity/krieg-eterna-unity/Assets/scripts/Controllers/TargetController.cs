using UnityEngine;
public class TargetController
{
    
    public static void ShowTargets(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Showing Targets for: " + c.cardName + " " + c.cardType);
        RowEffected enemyPlayable = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        if (CardModel.isUnit(c.cardType) && c.strengthModType == StrengthModType.Adjacent)
        {
            RowEffected playableRow = CardModel.getPlayableRow(player, c.cardType);
            Row r = deck.getRowByType(playableRow);
            Debug.Log(r + " " + r.Count);
            if (r.Count > 0)
            {
                deck.getRowByType(playableRow).setActivateRowCardTargets(true, true);
            }
            else
            {
                c.setTargetActive(true);
            }
        }
        else
        {
            switch (c.cardType)
            {
                case CardType.Switch:
                    deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerMelee))
                        .setActivateRowCardTargets(true, false);
                    deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerRanged))
                        .setActivateRowCardTargets(true, false);
                    break;

                default:
                    PowerController.TargetPower(c, player);
                    break;
            }
        }
        Game.reorganizeGroup();
    }

}

