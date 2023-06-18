using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameObject cardGameObject;
    private Card baseCard;
    private GameObject targetGameObject;
    private Target baseTarget;
    public Row playerHand = new Row(true, false, RowEffected.PlayerHand);
    public Row meleeRow = new Row(true, true, RowEffected.Melee);
    public Row rangedRow = new Row(true, true, RowEffected.Ranged);
    public Row siegeRow = new Row(true, true, RowEffected.Siege);

    public Row enemyMeleeRow = new Row(true, false, RowEffected.Melee);
    public Row enemyRangedRow = new Row(true, false, RowEffected.Ranged);
    public Row enemySiegeRow = new Row(true, false, RowEffected.Siege);

    public List<Row> rows = new List<Row>();

    public Row unitDeck = new Row(false, false, RowEffected.UnitDeck);
    public Row powerDeck = new Row(false, false, RowEffected.PowerDeck);
    public Row kingDeck = new Row(false, false, RowEffected.KingDeck);

    public Row unitGraveyard = new Row(false, false, RowEffected.UnitGraveyard);
    public Row powerGraveyard = new Row(false, false, RowEffected.PowerGraveyard);
    public Row cardsInSpecial = new Row(false, false, RowEffected.All); // special cards

    private GameObject areasObject;
    private Areas areas;

    private static int FRONTS_NUMBER = 102;
    // TODO - remove max amount of cards in each range group

    void Awake()
    {
        areasObject = GameObject.Find("Areas");
        areas = areasObject.GetComponent<Areas>();
        cardGameObject = GameObject.Find("Card");
        baseCard = cardGameObject.GetComponent<Card>();
        targetGameObject = GameObject.Find("Target");
        baseTarget = targetGameObject.GetComponent<Target>();
        baseCard.setBaseScale();
        meleeRow.center = areas.getMeleeRowCenterVector();
        rangedRow.center = areas.getRangedRowCenterVector();
        siegeRow.center = areas.getSiegeRowCenterVector();
        enemyMeleeRow.center = areas.getEnemyMeleeRowCenterVector();
        enemyRangedRow.center = areas.getEnemyRangedRowCenterVector();
        enemySiegeRow.center = areas.getEnemySiegeRowCenterVector();
        rows.Add(meleeRow);
        rows.Add(rangedRow);
        rows.Add(siegeRow);
        rows.Add(enemyMeleeRow);
        rows.Add(enemyRangedRow);
        rows.Add(enemySiegeRow);
        rows.Add(playerHand);
    }

    public void buildDeck(int numPowers, int numUnits, int numKings)
    {
        List<int> uniqueValues = new List<int>();

        Debug.Log("generating values");

        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {
            uniqueValues.Add(cardIndex);
        }
        
        
        for(int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {

            int cardId = uniqueValues[Random.Range(0, uniqueValues.Count)];

            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            Debug.Log("making card" + clone.cardName);
            if (CardModel.isPower(clone.cardType))
            {
                powerDeck.Add(clone);
            }
            else if (CardModel.isUnit(clone.cardType))
            {
                unitDeck.Add(clone);
            }
            else if (clone.cardType == CardType.King)
            {
                kingDeck.Add(clone);
            }
            uniqueValues.Remove(cardId);
        }
        int numCards = numPowers + numUnits + numKings;
        Debug.Log("dealing cards");
        while(numCards > 0)
        {
            
            Debug.Log("cards left to deal: " + numCards);
            Card card = baseCard;
            if (numPowers > 0)
            {
                card = powerDeck[0];
                powerDeck.Remove(card);
                numPowers--;
            }
            else if (numUnits > 0)
            {
                card = unitDeck[0];
                unitDeck.Remove(card);
                numUnits--;
            }
            else if(numKings > 0)
            {
                card = kingDeck[0];
                kingDeck.Remove(card);
                numKings--;
            }
            card.loadMaterial();
            playerHand.Add(card);
            numCards = numPowers + numUnits + numKings;
        }
        playerHand.center = areas.getDeckCenterVector();
    }

    public void buildTargets()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            Row row = rows[i];
            buildRowTarget(row.center, row);
        }
    }
    public Target buildCardTarget(Vector3 center)
    {
        Target clone = Instantiate(baseTarget) as Target;
        clone.setNotFlashing();
        clone.tag = "CloneTarget";
        clone.transform.position = new Vector3(center.x, center.y, -1f);
        clone.setBaseLoc();
        return clone;
    }
    public Target buildRowTarget(Vector3 center, Row row)
    {
        Target clone = Instantiate(baseTarget) as Target;
        clone.setNotFlashing();
        clone.tag = "CloneTarget";
        clone.scale(Mathf.Max(row.Count + 1, 8), 1);
        row.target = clone;
        clone.transform.position = new Vector3(center.x, center.y, -1f);
        clone.setBaseLoc();
        return clone;
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
            clone.loadMaterial();
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            playerHand.Add(clone);
        }
    }

    public IEnumerable<Card> getCards()
    {
        foreach (Card c in playerHand)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getSwordCards()
    {
        foreach (Card c in meleeRow)
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

        for (int i = meleeRow.Count - 1; i >= 0; i--)
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

    public bool sendCardToDeathList(Card card)
    {
        bool ifSucceeded = false;

        unitGraveyard.Add(card);
        if (card.getCardType() == CardType.Melee)
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
        if (group == 0 || group == 1)
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
        for (int i = 0; i < rows.Count; i++)
        {
            Row row = rows[i];
            Card weatherCard = clearWeatherRow(row);
            Debug.Log(weatherCard);
            if (row.isPlayer && weatherCard != null)
            {
                powerGraveyard.Add(weatherCard);
            }
        }
    }
    private Card clearWeatherRow(Row row)
    {
        Card ret = null;
        for (int i = 0; i < row.Count; i++)
        {
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
        switch (row)
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
