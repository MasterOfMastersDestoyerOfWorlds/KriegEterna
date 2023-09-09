using System.Collections.Generic;
using UnityEngine;
using System;
public class AltEffectController : EffectControllerInterface
{

    public void Play(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        
        c.setEffect(targetCard);
        Deck deck = Game.activeDeck;
        Debug.Log("Setting up Alt Effect: " + targetCard);

        RowEffected displayRowType = CardModel.getRowFromSide(player, RowEffected.PlayerAltEffectRow);
        Row displayRow = deck.getRowByType(displayRowType);
        List<AltEffect> effects = c.altEffects;
        deck.getRowByType(RowEffected.Skip).setVisibile(false);
        displayRow.setVisibile(false);
        Game.shadowCamera.enabled = false;
        displayRow.cardTargetsActivated = false;
        deck.disactiveAllInDeck(false);
        Game.state = State.MULTISTEP;
        if (c.canAutoPlayAltEffect)
        {
            Debug.Log("AUTOPLAYING");
            PlayController.Play(c, targetRow, targetCard, player);
        }
        else
        {
            TargetController.ShowTargets(c, player);
        }
    }
    public bool PlayCondition(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        return c.altEffects.Count > 0 && !c.isEffectSet;
    }



    public void Target(Card c, RowEffected player)
    {
        Deck deck = Game.activeDeck;
        Debug.Log("Setting up Effect Choice");

        RowEffected displayRowType = CardModel.getRowFromSide(player, RowEffected.PlayerAltEffectRow);
        Row displayRow = deck.getRowByType(displayRowType);
        List<AltEffect> effects = c.altEffects;
        c.scaleBig();
        Game.shadowCamera.enabled = true;
        Deck activeDeck = Game.activeDeck;
        Card activeCard = Game.activeCard;
        Debug.Log("setting up choice");
        float cardHorizontalSpacing = Card.getBaseWidth() * 2.5f;
        float cardThickness = Card.getBaseThickness();
        float attachmentVerticalSpacing = Card.getBaseHeight();
        displayRow.RemoveAll((x) => true);
        int playable = 0;
        for (int i = 0; i < effects.Count; i++)
        {
            AltEffect effect = effects[i];

            Debug.Log("Revealing: " + effect.cardName + " " + effect.cardType);
            effect.setVisible(true);
            effect.setLayer("Display", true);
            displayRow.Add(effect);
            if (effect.isPlayable(player))
            {
                effect.setTargetActive(true);
                playable++;
            }
        }
        if (playable > 0)
        {
            displayRow.cardTargetsActivated = true;
        }
        Debug.Log(displayRow.Count + " " + displayRow);
        Game.reorganizeRow(cardHorizontalSpacing, cardThickness, attachmentVerticalSpacing, displayRow, displayRow.center);

        deck.getRowByType(RowEffected.Skip).setVisibile(false);
    }

    public bool TargetCondition(Card c, RowEffected player)
    {
        Debug.Log("AltEffectController: ");

        Debug.Log("AltEffectController: " + c.altEffects.Count);
        Debug.Log("AltEffectController: " + !c.isEffectSet);
        return c.altEffects.Count > 0 && !c.isEffectSet;
    }

    public bool ShoudlReorganizeGroup()
    {
        return false;
    }
}