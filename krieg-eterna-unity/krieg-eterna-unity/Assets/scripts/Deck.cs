using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameObject cardGameObject;
    private Card baseCard;
    public List<Card> playerHand = new List<Card>();
    public List<Card> meleeRow = new List<Card>();
    public List<Card> rangedRow = new List<Card>();
    public List<Card> siegeRow = new List<Card>();
    public List<Card> unitGraveyard = new List<Card>();
    public List<Card> cardsInSpecial = new List<Card>(); // special cards

    private static int FRONTS_NUMBER = 102;
    // TODO - remove max amount of cards in each range group
    private static int MAX_NUMBER_OF_CARDS_IN_GROUP = 7;
    private static int SWORD_GROUP_AMOUNT = 7;
    private static int SWORD_GOLD_GROUP_AMOUNT = 5;
    private static int BOW_GROUP_AMOUNT = 5;
    private static int BOW_GOLD_GROUP_AMOUNT = 1;
    private static int TREBUCHET_GROUP_AMOUNT = 3;
    private static int TREBUCHET_GOLD_GROUP_AMOUNT = 0;

    void Awake()
    {
        cardGameObject = GameObject.Find("Card");
        baseCard = cardGameObject.GetComponent<Card>();
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
                cardId = Random.Range(0, FRONTS_NUMBER);
            } while (uniqueValues.Contains(cardId));
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

    public IEnumerable<Card> getDeathCards()
    {
        foreach (Card c in unitGraveyard)
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getSpecialCards()
    {
        foreach (Card c in cardsInSpecial)
        {
            yield return c;
        }
    }


    /// <summary>
    /// adding card from swords to deck
    /// </summary>
    /// <param name="card">card we want to move</param>
    /// <returns>true if operation succeeded</returns>
    public bool moveCardToDeckFromSwords(Card card)
    {
        playerHand.Add(card);
        return meleeRow.Remove(card);
    }

    /// <summary>
    /// adding card from bows to deck
    /// </summary>
    /// <param name="card">card we want to move</param>
    /// <returns>true if operation succeeded</returns>
    public bool moveCardToDeckFromBows(Card card)
    {
        playerHand.Add(card);
        return rangedRow.Remove(card);
    }

    /// <summary>
    /// adding card from trebuchets to deck
    /// </summary>
    /// <param name="card">card we want to move</param>
    /// <returns>true if operation succeeded</returns>
    public bool moveCardToDeckFromTrebuchets(Card card)
    {
        playerHand.Add(card);
        return siegeRow.Remove(card);
    }

    /// <summary>
    /// Adding spy card to opponent sword deck
    /// </summary>
    /// <param name="card">spy card we want to add</param>
    public void addSpy(Card card)
    {
        Vector3 newVector = new Vector3(-2.53f + meleeRow.Count * 1.05f, 1.66495f, -0.1f);
        card.transform.position = newVector;

        meleeRow.Add(card);
    }

    /// <summary>
    /// Adding weather and destroy cards to special box
    /// </summary>
    /// <param name="card">Crd we wnt to add</param>
    public void addToSpecial(Card card)
    {        
        cardsInSpecial.Add(card);
        playerHand.Remove(card);
    }

    /// <summary>
    /// Deleting weather from special box
    /// </summary>
    public void deleteFromSpecial()
    {
        foreach(Card c in getSpecialCards())
        {
            if(c.isSpecial == 5)
                sendCardToDeathList(c);
        }
        cardsInSpecial.Clear();
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

    /// <summary>
    /// disactivating cards in deck
    /// </summary>
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

    /// <summary>
    /// Removing card with highest power value
    /// </summary>
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

    /// <summary>
    /// Get sum of the card's powers from group or all caards
    /// </summary>
    /// <param name="group">number of group (0 - all, 1 - sword, 2 - bow, 3 - trebuchet)</param>
    /// <returns>sum of powers of cards in group(s)</returns>
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

    /// <summary>
    /// Tagging cards touched by weather card
    /// </summary>
    /// <param name="cardGroup">range of card</param>
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

    /// <summary>
    /// Flip player cards
    /// </summary>
    public void flipGroupCards()
    {
        foreach(Card card in getSwordCards())
        {
            //card.flip(true, true);
            card.mirrorTransform();
        }
        foreach (Card card in getBowCards())
        {
            //card.flip(true, true);
            card.mirrorTransform();
        }
        foreach (Card card in getTrebuchetCards())
        {
            //card.flip(true, true);
            card.mirrorTransform();
        }
        foreach (Card card in getDeathCards())
        {
            //card.flip(false, true);

            float x = card.transform.position.x;
            float y = card.transform.position.y;
            float z = card.transform.position.z;

            card.transform.position = new Vector3(x, y * -1f, z);
        }
        foreach(Card card in getSpecialCards())
        {
            float x = card.transform.position.x;
            float y = card.transform.position.y;
            float z = card.transform.position.z;

            card.transform.position = new Vector3(x, y * -1f, z);
        }
    }
}
