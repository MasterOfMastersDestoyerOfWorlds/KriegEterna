using UnityEngine;
public class TargetController
{
    
    public static void ShowTargets(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Showing Targets for: " + c.cardName + " " + c.cardType);
        RowEffected playerPlayable = CardModel.getRowFromSide(player, RowEffected.PlayerPlayable);
        RowEffected playerKing = CardModel.getRowFromSide(player, RowEffected.PlayerKing);
        RowEffected enemyKing = CardModel.getRowFromSide(player, RowEffected.EnemyKing);
        RowEffected eitherKing = CardModel.getRowFromSide(player, RowEffected.King);
        RowEffected enemyPlayable = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        RowEffected enemy = CardModel.getEnemy(player);
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
                case CardType.Melee: c.setTargetActive(true); break;
                case CardType.Ranged: c.setTargetActive(true); break;
                case CardType.Siege: c.setTargetActive(true); break;
                case CardType.Switch:
                    deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerMelee))
                        .setActivateRowCardTargets(true, false);
                    deck.getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerRanged))
                        .setActivateRowCardTargets(true, false);
                    break;
                case CardType.King:
                    if (Game.state == State.MULTISTEP)
                    {
                        if (c.enemyCardDestroy > 0)
                        {
                            deck.activateRowsByType(true, true, enemyPlayable);
                        }
                        else if (c.moveRemain > 0)
                        {
                            if (c.cardReturnType == CardReturnType.Move)
                            {
                                deck.activateRowsByTypeExclude(true, false, c.rowEffected, c.moveRow);
                            }
                            else if (c.cardReturnType == CardReturnType.Swap)
                            {
                                deck.activateRowsByTypeExclude(true, true, c.rowEffected, c.moveRow);
                            }
                        }
                        else if (c.playerCardReturnRemain > 0)
                        {
                            switch (c.cardReturnType)
                            {
                                case CardReturnType.King: deck.activateRowsByType(true, true, deck.getKingRow(player)); break;
                                default: deck.activateRowsByType(true, true, playerPlayable); break;
                            }

                        }
                        else if (c.setAsideRemain > 0)
                        {
                            switch (c.setAsideType)
                            {
                                case SetAsideType.King:
                                    deck.activateRowsByType(true, true, playerKing); break;
                                case SetAsideType.EnemyKing:
                                    deck.activateRowsByType(true, true, enemyKing); break;
                                case SetAsideType.Enemy:
                                    deck.activateRowsByType(true, true, enemy); break;
                                case SetAsideType.Player:
                                    deck.activateRowsByType(true, true, player); break;

                            }
                        }
                        else if (c.playerCardDrawRemain > 0)
                        {
                            deck.activateRowsByType(true, false, RowEffected.DrawableDeck);
                        }

                    }
                    else
                    {
                        deck.activateRowsByType(true, false, playerKing);
                    }
                    break;
                case CardType.Spy:
                    if (Game.state == State.MULTISTEP)
                    {
                        deck.activateRowsByType(true, false, RowEffected.DrawableDeck);
                    }
                    else
                    {
                        if (c.rowEffected == RowEffected.EnemyPlayable)
                        {
                            deck.activateRowsByType(true, false, enemyPlayable);
                        }
                        else
                        {
                            c.setTargetActive(true);
                        }
                    }
                ; break;
                case CardType.Decoy:
                    deck.activateRowsByType(true, true, c.rowEffected); break;
                case CardType.Weather: c.setTargetActive(true); break;
                case CardType.Power:
                    PowerController.TargetPower(c, player);
                    break;
                default: break;
            }
        }
        Game.reorganizeGroup();
    }

}

