using System.Collections.Generic;
using UnityEngine;
public static class PowerController
{
    public static List<EffectControllerInterface> mainControllerList = new List<EffectControllerInterface>(){
        new KingController(),
        new PlayInRowController(),
        new SpyController(),
        new PlayerDestroyController(),
        new EnemyDestroyController(),
        new MoveController(),
        new ReturnController(),
        new SetAsideController(),
        new PlayerDrawController(),
        new ChooseNController(),
        new AttachController(),
    };

    public static List<EffectControllerInterface> sideEffectControllerList = new List<EffectControllerInterface>(){
        new VoidController(),
        new AutoDrawController(),
        new EnemyDrawController(),
        new GraveyardDrawController(),
        new PowerGraveyardController(),
        new SwitchSidesRoundEndController(),
        new TelescopeController(),
        new WeatherController()
    };
    public static void PlayPower(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        Debug.Log("Playing Power Card targetRow:" + targetRow);
        foreach (EffectControllerInterface controller in mainControllerList)
        {
            if (controller.PlayCondition(c, targetRow, targetCard, player))
            {
                Debug.Log("Controller: " + controller);
                controller.Play(c, targetRow, targetCard, player);
                break;
            }
        }
        if (c.doneMultiSelection(player))
        {
            Debug.Log("Done MultiSelection, Doing Side Effects");
            foreach (EffectControllerInterface controller in sideEffectControllerList)
            {
                if (controller.PlayCondition(c, targetRow, targetCard, player))
                {
                    Debug.Log("Side Effect Controller: " + controller);
                    controller.Play(c, targetRow, targetCard, player);
                }
            }
        }

        Debug.Log("Done Playing Power Card targetRow:" + targetRow);
    }

    public static void TargetPower(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        RowEffected enemyPlayable = CardModel.getRowFromSide(player, RowEffected.EnemyPlayable);
        Debug.Log("Setup Power Targets: " + c.cardName);
        bool flag = false;
        foreach (EffectControllerInterface controller in mainControllerList)
        {
            if (controller.TargetCondition(c, player))
            {
                Debug.Log("Target Controller: " + controller);
                controller.Target(c, player);
                flag = true;
                break;
            }
        }
        if (!flag && c.rowEffected != RowEffected.None)
        {
            if (c.rowEffected == RowEffected.EnemyMax)
            {
                deck.activateAllRowsByType(true, false, deck.getMaxScoreRows(enemyPlayable));
            }
            if (c.cardType != CardType.Weather)
            {
                deck.activateRowsByType(true, true, c.rowEffected);
            }else{
                deck.activateRowsByType(true, false, c.rowEffected);
            }
        }
        else
        {
            c.setTargetActive(true);
        }
    }
}