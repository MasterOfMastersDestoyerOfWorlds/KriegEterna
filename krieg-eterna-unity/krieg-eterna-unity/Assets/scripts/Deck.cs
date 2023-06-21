using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameObject cardGameObject;
    private Card baseCard;
    private GameObject targetGameObject;
    private Target baseTarget;

    private Areas areas;

    public List<Row> rows;
    private GameObject areasObject;

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
        rows = new List<Row>(){
        new Row(true, false, true, RowEffected.PlayerHand, new List<RowEffected>() { RowEffected.BothHand, RowEffected.PlayerHand}, areas.getDeckCenterVector),
        new Row(true, false, true, RowEffected.EnemyHand, new List<RowEffected>() { RowEffected.BothHand, RowEffected.EnemyHand }, areas.getDeckCenterVector),
        new Row(true, true, true, RowEffected.PlayerMelee, new List<RowEffected>() { RowEffected.PlayerMelee, RowEffected.Player, RowEffected.All, RowEffected.Melee }, areas.getMeleeRowCenterVector),
        new Row(true, true, true, RowEffected.PlayerRanged, new List<RowEffected>() {RowEffected.PlayerRanged, RowEffected.Player, RowEffected.All, RowEffected.Ranged }, areas.getRangedRowCenterVector),
        new Row(true, true, true, RowEffected.PlayerSiege, new List<RowEffected>() { RowEffected.PlayerSiege, RowEffected.Player, RowEffected.All, RowEffected.Siege }, areas.getSiegeRowCenterVector),
        new Row(true, true, true, RowEffected.EnemyMelee, new List<RowEffected>() {RowEffected.EnemyMelee, RowEffected.Enemy, RowEffected.All, RowEffected.Melee }, areas.getEnemyMeleeRowCenterVector),
        new Row(true, true, true, RowEffected.EnemyRanged, new List<RowEffected>() {RowEffected.EnemyRanged, RowEffected.Enemy, RowEffected.All, RowEffected.Ranged }, areas.getEnemyRangedRowCenterVector),
        new Row(true, true, true, RowEffected.EnemySiege, new List<RowEffected>() {RowEffected.EnemySiege, RowEffected.Enemy, RowEffected.All, RowEffected.Siege }, areas.getEnemySiegeRowCenterVector),
        new Row(false, false, false, RowEffected.UnitDeck, new List<RowEffected>() { RowEffected.DrawableDeck, RowEffected.UnitDeck }, areas.getUnitDeckCenterVector),
        new Row(false, false, false, RowEffected.PowerDeck, new List<RowEffected>() { RowEffected.DrawableDeck, RowEffected.PowerDeck }, areas.getPowerDeckCenterVector),
        new Row(false, false, false, RowEffected.KingDeck, new List<RowEffected>() { RowEffected.KingDeck }, areas.getKingDeckCenterVector),
        new Row(false, false, false, RowEffected.UnitGraveyard, new List<RowEffected>() { RowEffected.UnitGraveyard }, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, RowEffected.PowerGraveyard, new List<RowEffected>() { RowEffected.PowerGraveyard }, areas.getPowerGraveyardCenterVector),
        new Row(true, true, false, RowEffected.PlayerMeleeKing, new List<RowEffected>() { RowEffected.PlayerMeleeKing, RowEffected.PlayerKing }, areas.getMeleeKingCenterVector),
        new Row(true, true, false, RowEffected.PlayerRangedKing, new List<RowEffected>() { RowEffected.PlayerRangedKing,RowEffected.PlayerKing }, areas.getRangedKingCenterVector),
        new Row(true, true, false, RowEffected.PlayerSiegeKing, new List<RowEffected>() { RowEffected.PlayerSiegeKing,RowEffected.PlayerKing }, areas.getSiegeKingCenterVector),
        new Row(true, true, false, RowEffected.EnemyMeleeKing, new List<RowEffected>() { RowEffected.EnemyMeleeKing, RowEffected.EnemyKing }, areas.getMeleeKingCenterVector),
        new Row(true, true, false, RowEffected.EnemyRangedKing, new List<RowEffected>() {  RowEffected.EnemyRangedKing, RowEffected.EnemyKing }, areas.getRangedKingCenterVector),
        new Row(true, true, false, RowEffected.EnemySiegeKing, new List<RowEffected>() { RowEffected.EnemySiegeKing, RowEffected.EnemyKing }, areas.getSiegeKingCenterVector),
        new Row(false, false, false, RowEffected.None, new List<RowEffected>() { RowEffected.All }, areas.getDeckCenterVector) // special cards
        };
    }

    void Start()
    {

        updateRowCenters();
    }

    public void buildDeck(int numPowers, int numUnits, int numKings)
    {
        List<int> uniqueValues = new List<int>();

        Debug.Log("generating values");

        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {
            uniqueValues.Add(cardIndex);
        }


        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {

            int cardId = uniqueValues[Random.Range(0, uniqueValues.Count)];

            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            Debug.Log("Making Card" + clone.cardName);
            if (CardModel.isPower(clone.cardType))
            {
                getRowByType(RowEffected.PowerDeck).Add(clone);
            }
            else if (CardModel.isUnit(clone.cardType))
            {
                getRowByType(RowEffected.UnitDeck).Add(clone);
            }
            else if (clone.cardType == CardType.King)
            {
                getRowByType(RowEffected.KingDeck).Add(clone);
            }
            uniqueValues.Remove(cardId);
        }
        int numCards = numPowers + numUnits + numKings;
        Debug.Log("Dealing Cards");
        while (numCards > 0)
        {

            Debug.Log("Cards Left to Deal: " + numCards);
            Card card = baseCard;
            if (numPowers > 0)
            {
                card = getRowByType(RowEffected.PowerDeck)[0];
                getRowByType(RowEffected.PowerDeck).Remove(card);
                numPowers--;
            }
            else if (numUnits > 0)
            {
                card = getRowByType(RowEffected.UnitDeck)[0];
                getRowByType(RowEffected.UnitDeck).Remove(card);
                numUnits--;
            }
            else if (numKings > 0)
            {
                card = getRowByType(RowEffected.KingDeck)[0];
                getRowByType(RowEffected.KingDeck).Remove(card);
                numKings--;
            }
            card.loadMaterial();
            getRowByType(RowEffected.PlayerHand).Add(card);
            numCards = numPowers + numUnits + numKings;

        }
        updateRowCenters();
    }

    public void updateRowCenters()
    {
        foreach (Row row in rows)
        {
            row.centerRow();
        }
    }

    public void buildTargets()
    {
        Debug.Log("Building Targets");
        for (int i = 0; i < rows.Count; i++)
        {
            Row row = rows[i];
            buildRowTarget(row);
        }
    }
    public Target buildRowTarget(Row row)
    {
        Vector3 center = row.center;
        Target clone = Instantiate(baseTarget) as Target;
        clone.setNotFlashing();
        clone.tag = "CloneTarget";
        if (row.wide)
        {
            clone.scale(Mathf.Max(row.Count + 1, 8), 1);
        }
        else
        {
            clone.scale(1, 1);
        }
        row.setTarget(clone);
        clone.transform.position = new Vector3(center.x, center.y, -1f);
        clone.setBaseLoc();
        return clone;
    }

    public void disactiveAllInDeck()
    {
        foreach (Row row in rows)
        {
            foreach (Card c in row)
            {
                if (c.isTargetActive())
                {
                    c.setTargetActive(false);
                    c.setPlayable(false);
                    c.resetTransform();
                }
                if (c.isBig)
                {
                    c.resetScale();
                    c.resetTransform();
                }
            }
            if (row.target != null)
            {
                row.target.setNotFlashing();
            }
        }
    }

    public List<Card> getVisibleCards()
    {
        List<Card> cards = new List<Card>();
        foreach (Row row in rows)
        {
            if (row.isPlayer)
            {
                foreach (Card card in row)
                {
                    cards.Add(card);
                }

            }
        }

        return cards;
    }
    public List<Card> getActiveCardTargets()
    {
        List<Card> cardTargets = new List<Card>();
        foreach (Row row in rows)
        {
            if (row.isPlayer)
            {
                foreach (Card card in row)
                {
                    if (card.isTargetActive()){
                        cardTargets.Add(card);
                    }
                }

            }
        }
        return cardTargets;
    }
    public List<Row> getActiveRowTargets()
    {
        List<Row> rowTargets = new List<Row>();
        foreach (Row row in rows)
        {
            if (row.target != null && row.target.isFlashing())
            {
                rowTargets.Add(row);
            }
        }
        return rowTargets;
    }

    public void activateRowsByType(bool state, bool individualCards, RowEffected type)
    {
        List<Row> rowList = getRowsByType(type);
        Debug.Log(getRowByType(RowEffected.PlayerMeleeKing).target);
        foreach (Row row in rowList)
        {
            row.setActivateRowCardTargets(state, individualCards);
        }
    }

    public List<Row> getRowsByType(RowEffected type)
    {
        List<Row> rowList = new List<Row>();
        foreach (Row row in rows)
        {
            if (row.isType(type))
            {
                rowList.Add(row);
            }
        }
        return rowList;
    }
    public Row getRowByType(RowEffected type)
    {
        foreach (Row row in rows)
        {
            if (row.isTypeUnique(type))
            {
                return row;
            }
        }
        return null;
    }
    public Row getRowByName(string name)
    {
        foreach (Row row in rows)
        {
            if (row.name == name)
            {
                return row;
            }
        }
        return null;
    }


    public IEnumerable<Card> getSwordCards()
    {
        foreach (Card c in getRowByType(RowEffected.PlayerMelee))
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getBowCards()
    {
        foreach (Card c in getRowByType(RowEffected.PlayerRanged))
        {
            yield return c;
        }
    }

    public IEnumerable<Card> getTrebuchetCards()
    {
        foreach (Card c in getRowByType(RowEffected.PlayerSiege))
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
        Row graveyard = getRowByType(RowEffected.UnitGraveyard);
        Row meleeRow = getRowByType(RowEffected.PlayerMelee);
        Row rangedRow = getRowByType(RowEffected.PlayerRanged);
        Row siegeRow = getRowByType(RowEffected.PlayerSiege);
        for (int i = meleeRow.Count - 1; i >= 0; i--)
        {
            graveyard.Add(meleeRow[i]);
            ifSucceeded = meleeRow.Remove(meleeRow[i]);
        }
        for (int i = rangedRow.Count - 1; i >= 0; i--)
        {
            graveyard.Add(rangedRow[i]);
            ifSucceeded = rangedRow.Remove(rangedRow[i]);
        }
        for (int i = siegeRow.Count - 1; i >= 0; i--)
        {
            graveyard.Add(siegeRow[i]);
            ifSucceeded = siegeRow.Remove(siegeRow[i]);
        }

        return ifSucceeded;
    }

    public bool sendCardToDeathList(Card card)
    {
        bool ifSucceeded = false;

        getRowByType(RowEffected.UnitGraveyard).Add(card);
        if (card.getCardType() == CardType.Melee)
            ifSucceeded = getRowByType(RowEffected.PlayerMelee).Remove(card);
        if (card.getCardType() == CardType.Ranged)
            ifSucceeded = getRowByType(RowEffected.PlayerRanged).Remove(card);
        if (card.getCardType() == CardType.Siege)
            ifSucceeded = getRowByType(RowEffected.PlayerSiege).Remove(card);

        Vector3 player1DeathAreaVector = new Vector3(8.51f, -4.6f, -0.1f);
        card.transform.position = player1DeathAreaVector;

        float x = card.transform.position.x;
        float y = card.transform.position.y;
        float z = card.transform.position.z;

        card.transform.position = new Vector3(x, y * -1f, z);



        return ifSucceeded;
    }


    public Card getPlayableCard()
    {
        foreach (Card c in getRowByType(RowEffected.PlayerHand))
        {
            if (c.isPlayable())
            {
                return c;
            }
        }
        return null;
    }


    public void removeMaxPowerCard()
    {
        float maxPower = 0;
        Card maxCard = null;
        foreach (Card card in getRowByType(RowEffected.PlayerMelee))
        {
            // checing if card is not a gold one and has no weather effect
            if ((card.weatherEffect == false && card.getRowMultiple() > maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 > maxPower && card.getIsSpecial() != 1))
            {
                maxPower = card.getRowMultiple();
                maxCard = card;
            }
        }
        foreach (Card card in getRowByType(RowEffected.PlayerRanged))
        {
            // checing if card is not a gold one
            if ((card.weatherEffect == false && card.getRowMultiple() > maxPower && card.getIsSpecial() != 1) || (card.weatherEffect == true && 1 > maxPower && card.getIsSpecial() != 1))
            {
                maxPower = card.getRowMultiple();
                maxCard = card;
            }
        }
        foreach (Card card in getRowByType(RowEffected.PlayerSiege))
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
        List<Row> scoringRows = getRowsByType(RowEffected.All);
        for (int i = 0; i < scoringRows.Count; i++)
        {
            Row row = scoringRows[i];
            Card weatherCard = clearWeatherRow(row);
            Debug.Log(weatherCard);
            if (row.isPlayer && weatherCard != null)
            {
                getRowByType(RowEffected.PowerGraveyard).Add(weatherCard);
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
