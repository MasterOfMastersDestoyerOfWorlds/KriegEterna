using System;
using UnityEngine;
public class TargetController
{

    public static void ShowTargets(Card c, RowEffected player)
    {
        if(c == null && Game.state == State.CHOOSE_N){
            return;
        }
        Deck deck = Game.activeDeck;
        Debug.Log("Showing Targets for: " + c.cardName + " " + c.cardType);
        if(Game.state != State.CHOOSE_N && Game.state != State.MULTISTEP){
            deck.disactiveAllInDeck(false);
        }else{
            deck.disactiveAllInDeck(true);
        }
        PowerController.TargetPower(c, player);
    }

    internal static void ShowTargets(Move nextMove)
    {
        ShowTargets(nextMove.c, nextMove.player);
    }
}

