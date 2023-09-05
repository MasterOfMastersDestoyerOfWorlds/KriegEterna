using System.Collections.Generic;
using UnityEngine;
public static class PowerController
{
    public static List<EffectControllerInterface> mainControllerList = new List<EffectControllerInterface>(){
        new TelescopeTargetController(),
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
        new ClearWeatherTargetController(),
        new RowTargetController(),
        new DefaultTargetController(),
    };

    public static List<EffectControllerInterface> sideEffectControllerList = new List<EffectControllerInterface>(){
        new VoidController(),
        new AutoDrawController(),
        new EnemyDrawController(),
        new GraveyardDrawController(),
        new PowerGraveyardController(),
        new SwitchSidesRoundEndController(),
        new PlayNextRoundController(),
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
        Debug.Log("Setup Power Targets: " + c.cardName);
        foreach (EffectControllerInterface controller in mainControllerList)
        {
            if (controller.TargetCondition(c, player))
            {
                Debug.Log("Target Controller: " + controller);
                controller.Target(c, player);
                break;
            }
        }
    }
}