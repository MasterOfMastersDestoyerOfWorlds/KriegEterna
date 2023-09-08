using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Reflection;

public class AltEffect : Card
{


    public override void initCard(int index)
    {
        this.index = index;
        if (index > CardModel.names.Count)
        {
            Debug.LogError("Out of Bounds! expected max index: " + (cardModel.numCardEffects - 1) + " got: " + index + " num card names:" + CardModel.names.Count);
        }
        setEffectFromCardModel(index);
        this.cardName = CardModel.names[index];
        this.name = cardName;
        this.strengthText.text = "";
        this.setVisible(false);
        this.effectDescriptionText.text = this.effectDescription;
        this.effectDescriptionText.alpha = 0;
        this.cardColider = effectDescriptionTextObj.GetComponent<BoxCollider2D>();
        this.targetObj.transform.localScale = new Vector3(14.2f, 3.81f, 1);
        Material targetMaterial = this.getTargetMaterial();
        targetMaterial.SetFloat("_RectWidth", 0.79f);
        targetMaterial.SetFloat("_RectHeight", 0.67f);
        this.loadCardBackMaterial();
    }

    public override void setVisible(bool state)
    {
        Material material = getCardFrontMaterial();
        material.SetInt("_Transparent", 1);
        this.effectDescriptionText.alpha = state ? 1 : 0;
        this.visible = state;
    }

}