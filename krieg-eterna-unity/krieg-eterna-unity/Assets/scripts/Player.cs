using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public float score;
    public int health;
    public bool isPlaying;
    public int player;

    private GameObject lifeObject;
    private HealthDiamond life;
    private HealthDiamond firstHealthDiamond;
    private HealthDiamond secondHealthDiamond;


    void Awake() 
    {
        lifeObject = GameObject.Instantiate( Resources.Load("Prefabs/HealthDiamond") as GameObject, transform.position, transform.rotation);
        life = lifeObject.GetComponent<HealthDiamond>();

        // creating two instance of life diamond image
        firstHealthDiamond = Instantiate(life) as HealthDiamond;
        secondHealthDiamond = Instantiate(life) as HealthDiamond;

        // settings of instances
        Debug.Log("Player creating - P" + player);
        if (player == (int)WhichPlayer.PLAYER2)
        {
            firstHealthDiamond.moveTo(-8.05f, 2.26f);
            secondHealthDiamond.moveTo(-7.31f, 2.26f);
        }
        else
        {
            firstHealthDiamond.moveTo(-8.05f, -1.93f);
            secondHealthDiamond.moveTo(-7.31f, -1.93f);
        }

        firstHealthDiamond.enableSprite();
        secondHealthDiamond.enableSprite();

        string objectGameName = this.gameObject.name + "Deck";
        health = 2;
        score = 0;
        isPlaying = true;
        if (this.gameObject.name.Equals("Player1"))
            player = (int)WhichPlayer.PLAYER1;
        else
            player = (int)WhichPlayer.PLAYER2;

            
    }

    void Update()
    {
        this.score = Game.activeDeck.scoreRow(RowEffected.PlayerMelee);
    }

    /// <summary>
    /// Method which updating player's health visualisation
    /// </summary>
    public void updateHealthDiamonds()
    {
        if (health == 1)
            secondHealthDiamond.setVisibility(false);
        else if (health == 0 || health == -1)
            firstHealthDiamond.setVisibility(false);
    }

    /// <summary>
    /// Getting Health Diamond
    /// </summary>
    /// <param name="whichHealth">number indicates which health we want to get</param>
    /// <returns>HealthDiamond of health</returns>
    public HealthDiamond getHealthDiamond(int whichHealth)
    {
        if (whichHealth == 1)
            return firstHealthDiamond;
        else
            return secondHealthDiamond;
    }

    /// <summary>
    /// Moving cards from each group of desk to plce where dead cards are
    /// </summary>
    /// <param name="activePlayerNumber">which player is playing</param>
    public void moveCardsFromDeskToDeathArea(int activePlayerNumber)
    {
        Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        Vector3 player2DeathAreaVector = new Vector3(8.51f, 4.6f, -0.1f);

        foreach (Card card in Game.activeDeck.getRowByType(RowEffected.UnitGraveyard))
        {
            if (player == (int)WhichPlayer.PLAYER2)
            {
                card.transform.position = player1DeathAreaVector;
                 if (activePlayerNumber == (int)WhichPlayer.PLAYER1)
                 {
                     float x = card.transform.position.x;
                     float y = card.transform.position.y;
                     float z = card.transform.position.z;

                     card.transform.position = new Vector3(x, y * -1f, z);
                 }
            }
            else
            {
                card.transform.position = player2DeathAreaVector;
                if (activePlayerNumber == (int)WhichPlayer.PLAYER1)
                {
                    float x = card.transform.position.x;
                    float y = card.transform.position.y;
                    float z = card.transform.position.z;

                    card.transform.position = new Vector3(x, y * -1f, z);
                }
            }
        }
    }

    private enum WhichPlayer { PLAYER1 = 1, PLAYER2 };
}
