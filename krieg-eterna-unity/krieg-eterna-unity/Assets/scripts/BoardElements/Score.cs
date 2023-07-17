using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    public int score;
    public int group;

    public RowEffected rowEffected;
    
    private GameObject deckObject;
    private Deck deck;
    private Text _MyText;

    void Awake()
    {
        string currentGameObjectName = this.gameObject.name;

        string playerDeckName = "Deck(Clone)";
        deckObject = GameObject.Find(playerDeckName);
        deck = deckObject.GetComponent<Deck>();

        _MyText = GameObject.Find(currentGameObjectName).GetComponent<Text>();
    }

    void Update()
    {
        //s_MyText.text = Game.activeDeck.scoreRow(rowEffected).ToString();
    }
}
