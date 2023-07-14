using System;
using UnityEngine;
public class TargetController
{

    public static void ShowTargets(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Showing Targets for: " + c.cardName + " " + c.cardType);

        PowerController.TargetPower(c, player);

        Game.reorganizeGroup();
    }

    internal static void ShowTargets(Move nextMove)
    {
        ShowTargets(nextMove.c, nextMove.player);
    }
}

