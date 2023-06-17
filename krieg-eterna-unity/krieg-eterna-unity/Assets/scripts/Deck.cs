using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameObject cardGameObject;
    private Card baseCard;
    public Row playerHand = new Row(true, false);
    public Row meleeRow = new Row(true, true);
    public Row rangedRow = new Row(true, true);
    public Row siegeRow = new Row(true, true);

     public Row enemyMeleeRow = new Row(true, false);
    public Row enemyRangedRow = new Row(true, false);
    public Row enemySiegeRow = new Row(true, false);

    public List<Row> rows = new List<Row>(); 


    public Row unitGraveyard = new Row(false, false);
    public Row powerGraveyard = new Row(false, false);
    public Row cardsInSpecial = new Row(false, false); // special cards

    private static int FRONTS_NUMBER = 102;
    // TODO - remove max amount of cards in each range group

    void Awake()
    {
        cardGameObject = GameObject.Find("Card");
        baseCard = cardGameObject.GetComponent<Card>();
        rows.Add(meleeRow);
        rows.Add(rangedRow);
        rows.Add(siegeRow);
        rows.Add(enemyMeleeRow);
        rows.Add(enemyRangedRow);
        rows.Add(enemySiegeRow);
    }

    /// <summary>
    /// method for buildiing new deck - adding cards to player's deck
    /// </summary>
    /// <param name="numberOfCards">how many cards have to be added to player's deck</param>
    public void buildDeck(int numberOfCards)
    {
        List<int> uniqueValues = new List<int>();

        for (int cardIndex = 0; cardIndex < numberOfCards; cardIndex++)
        {
            // For unique cards set
            int cardId;
            do
            {
                cardId = Random.Range(1, FRONTS_NUMBER);
            } while (uniqueValues.Contains(cardId));

            if(cardIndex == 1){
                cardId = 87;
            }

            
            if(cardIndex == 2){
                cardId = 19;
            }

            uniqueValues.Add(cardId);
            
            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setFront(cardId);
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            playerHand.Add(clone);
        }
    }

    /// <summary>
    /// Adding 2 random cards to player's deck
    /// </summary>
    /// <param name="whichPlayer"></param>
    public void addTwoRandomCards()
    {
        // TODO - Cards aren't unique!!!!!!!!!!!!!!!!!!!!!
        for (int i = 0; i < 2; i++)
        {
            int cardId = Random.Range(0, FRONTS_NUMBER);
            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setFront(cardId);
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            playerHand.Add(clone);
        }
    }

    public IEnumerable<Card> getCards()
    {
        foreach(Card c in playerHand)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getSwordCards()
    {
        foreach(Card c in meleeRow)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getBowCards()
    {
        foreach (Card c in rangedRow)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getTrebuchetCards()
    {
        foreach (Card c in siegeRow)
        {
            yield return c;
        }
    }

    /// <summary>
    /// Send cards from desk to death deck
    /// </summary>
    /// <returns>true if succeeded</returns>
    public bool sendCardsToDeathList()
    {
        bool ifSucceeded = false;

        for (int i = meleeRow.Count -1; i >=0; i--)
        {
            unitGraveyard.Add(meleeRow[i]);
            ifSucceeded = meleeRow.Remove(meleeRow[i]);
        }
        for (int i = rangedRow.Count - 1; i >= 0; i--)
        {
            unitGraveyard.Add(rangedRow[i]);
            ifSucceeded = rangedRow.Remove(rangedRow[i]);
        }
        for (int i = siegeRow.Count - 1; i >= 0; i--)
        {
            unitGraveyard.Add(siegeRow[i]);
            ifSucceeded = siegeRow.Remove(siegeRow[i]);
        }

        return ifSucceeded;
    }

    /// <summary>
    /// Sending card to death list
    /// </summary>
    /// <param name="card">card we want to send</param>
    /// <returns>true if succeeded</returns>
    public bool sendCardToDeathList(Card card)
    {
        bool ifSucceeded = false;

        unitGraveyard.Add(card);
        if(card.getCardType() == CardType.Melee)
            ifSucceeded = meleeRow.Remove(card);
        if (card.getCardType() == CardType.Ranged)
            ifSucceeded = rangedRow.Remove(card);
        if (card.getCardType() == CardType.Siege)
            ifSucceeded = siegeRow.Remove(card);

        Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        card.transform.position = player1DeathAreaVector;
        
        float x = card.transform.position.x;
        float y = card.transform.position.y;
        float z = card.transform.position.z;

        card.transform.position = new Vector3(x, y * -1f, z);
        
        

        return ifSucceeded;
    }

    public void disactiveAllInDeck()
    {
        if (playerHand.Count > 0)
        {
            foreach (Card c in getCards())
            {
                if (c.isActive())
                {
                    c.setActive(false);
                    c.resetTransform();
                }
            }
        }
    }

    public void removeMaxPowerCard()
    {
        float maxPower = 0;
        Card maxCard = null;
        foreach (Card card in meleeRow)
        {
            // checing if card is not a gold one and has no weather effect
            if ((card.weatherEffect == false && card.getRowMultiple() > maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 > maxPower && card.getIsSpecial() != 1))
            {
                maxPower = card.getRowMultiple();
                maxCard = card;
            }
        }
        foreach (Card card in rangedRow)
        {
            // checing if card is not a gold one
            if ((card.weatherEffect == false && card.getRowMultiple() > maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 > maxPower && card.getIsSpecial() != 1))
            {
                maxPower = card.getRowMultiple();
                maxCard = card;
            }
        }
        foreach (Card card in siegeRow)
        {
            // checing if card is not a gold one
            if ((card.weatherEffect == false && card.getRowMultiple() > maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 > maxPower && card.getIsSpecial() != 1))
            {
                maxPower = card.getRowMultiple();
                maxCard = card;
            }
        }
        if (maxCard != null)
            sendCardToDeathList(maxCard);
    }

    public float getPowerSum(int group)
    {
        float result = 0;

        if (group == 0 || group == 3)
        {
            foreach (Card card in getTrebuchetCards())
            {
                if (card.weatherEffect == false)
                    result += card.getRowMultiple();
                else
                    result++;
            }
        }
        if (group == 0 || group == 2)
        { 
            foreach (Card card in getBowCards())
            {
                if (card.weatherEffect == false)
                    result += card.getRowMultiple();
                else
                    result++;
            }
        }
        if(group == 0 || group == 1)
        { 
            foreach (Card card in getSwordCards())
            {
                if (card.weatherEffect == false)
                    result += card.getRowMultiple();
                else
                    result++;
            }
        }

        return result;
    }

    public void clearAllWeatherEffects()
    {
        Debug.Log("Clearing all weather");
        for (int i = 0; i < rows.Count; i++){
            Row row = rows[i];
            Card weatherCard = clearWeatherRow(row);
            Debug.Log(weatherCard);
            if(row.isPlayer && weatherCard != null){
                powerGraveyard.Add(weatherCard);
            }
        }
    }
    private Card clearWeatherRow(Row row){
        Card ret = null;
        for (int i = 0; i < row.Count; i++){
            ret = row.Find(isWeather);
            row.RemoveAll(isWeather);
        }
        return ret;
    }
    private static bool isWeather(Card c)
    {
        return c.cardType == CardType.Weather;
    }

    public void applyWeatherEffect(float rowMultiple, RowEffected row)
    {
        switch(row)
        {
            case RowEffected.Melee:
                foreach (Card card in getSwordCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = true;
                }
                break;
            case RowEffected.Ranged:
                foreach (Card card in getBowCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = true;
                }
                break;
            case RowEffected.Siege:
                foreach (Card card in getTrebuchetCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = true;
                }
                break;
            case RowEffected.All:
                foreach (Card card in getSwordCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = false;
                }
                foreach (Card card in getBowCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = false;
                }
                foreach (Card card in getTrebuchetCards())
                {
                    if (card.getIsSpecial() == 0)
                        card.weatherEffect = false;
                }
                break;
        }
    }
}
