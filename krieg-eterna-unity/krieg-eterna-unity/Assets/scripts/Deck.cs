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
        new Row(true, false, true, RowEffected.PlayerHand, new List<RowEffected>() { RowEffected.BothHand, RowEffected.PlayerHand, RowEffected.Player}, areas.getDeckCenterVector),
        new Row(true, false, true, RowEffected.EnemyHand, new List<RowEffected>() { RowEffected.BothHand, RowEffected.EnemyHand, RowEffected.Enemy }, areas.getDeckCenterVector),
        new Row(true, true, true, RowEffected.PlayerMelee, new List<RowEffected>() { RowEffected.PlayerMelee, RowEffected.PlayerPlayable, RowEffected.All, RowEffected.Melee, RowEffected.Player, RowEffected.MeleeFull }, areas.getMeleeRowCenterVector),
        new Row(true, true, true, RowEffected.PlayerRanged, new List<RowEffected>() {RowEffected.PlayerRanged, RowEffected.PlayerPlayable, RowEffected.All, RowEffected.Ranged, RowEffected.Player, RowEffected.RangedFull }, areas.getRangedRowCenterVector),
        new Row(true, true, true, RowEffected.PlayerSiege, new List<RowEffected>() { RowEffected.PlayerSiege, RowEffected.PlayerPlayable, RowEffected.All, RowEffected.Siege, RowEffected.Player, RowEffected.SiegeFull }, areas.getSiegeRowCenterVector),
        new Row(true, true, true, RowEffected.EnemyMelee, new List<RowEffected>() {RowEffected.EnemyMelee, RowEffected.EnemyPlayable, RowEffected.All, RowEffected.Melee, RowEffected.Enemy, RowEffected.MeleeFull }, areas.getEnemyMeleeRowCenterVector),
        new Row(true, true, true, RowEffected.EnemyRanged, new List<RowEffected>() {RowEffected.EnemyRanged, RowEffected.EnemyPlayable, RowEffected.All, RowEffected.Ranged, RowEffected.Enemy, RowEffected.RangedFull }, areas.getEnemyRangedRowCenterVector),
        new Row(true, true, true, RowEffected.EnemySiege, new List<RowEffected>() {RowEffected.EnemySiege, RowEffected.EnemyPlayable, RowEffected.All, RowEffected.Siege, RowEffected.Enemy, RowEffected.SiegeFull }, areas.getEnemySiegeRowCenterVector),
        new Row(false, false, false, RowEffected.UnitDeck, new List<RowEffected>() { RowEffected.DrawableDeck, RowEffected.UnitDeck }, areas.getUnitDeckCenterVector),
        new Row(false, false, false, RowEffected.PowerDeck, new List<RowEffected>() { RowEffected.DrawableDeck, RowEffected.PowerDeck }, areas.getPowerDeckCenterVector),
        new Row(false, false, false, RowEffected.KingDeck, new List<RowEffected>() { RowEffected.KingDeck }, areas.getKingDeckCenterVector),
        new Row(false, false, false, RowEffected.UnitGraveyard, new List<RowEffected>() { RowEffected.UnitGraveyard }, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, RowEffected.PowerGraveyard, new List<RowEffected>() { RowEffected.PowerGraveyard }, areas.getPowerGraveyardCenterVector),
        new Row(true, true, false, RowEffected.PlayerMeleeKing, new List<RowEffected>() { RowEffected.PlayerMeleeKing, RowEffected.PlayerKing, RowEffected.Player, RowEffected.King, RowEffected.MeleeFull }, areas.getMeleeKingCenterVector),
        new Row(true, true, false, RowEffected.PlayerRangedKing, new List<RowEffected>() { RowEffected.PlayerRangedKing, RowEffected.PlayerKing, RowEffected.Player, RowEffected.King, RowEffected.RangedFull }, areas.getRangedKingCenterVector),
        new Row(true, true, false, RowEffected.PlayerSiegeKing, new List<RowEffected>() { RowEffected.PlayerSiegeKing, RowEffected.PlayerKing, RowEffected.Player, RowEffected.King, RowEffected.SiegeFull }, areas.getSiegeKingCenterVector),
        new Row(true, true, false, RowEffected.EnemyMeleeKing, new List<RowEffected>() { RowEffected.EnemyMeleeKing, RowEffected.EnemyKing, RowEffected.Enemy, RowEffected.King, RowEffected.MeleeFull }, areas.getMeleeKingCenterVector),
        new Row(true, true, false, RowEffected.EnemyRangedKing, new List<RowEffected>() {  RowEffected.EnemyRangedKing, RowEffected.EnemyKing, RowEffected.Enemy, RowEffected.King, RowEffected.RangedFull }, areas.getRangedKingCenterVector),
        new Row(true, true, false, RowEffected.EnemySiegeKing, new List<RowEffected>() { RowEffected.EnemySiegeKing, RowEffected.EnemyKing, RowEffected.Enemy, RowEffected.King, RowEffected.SiegeFull }, areas.getSiegeKingCenterVector),
        new Row(false, false, false, RowEffected.PlayerSetAside, new List<RowEffected>() { RowEffected.PlayerSetAside, RowEffected.Player }, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, RowEffected.EnemySetAside, new List<RowEffected>() { RowEffected.EnemySetAside, RowEffected.Enemy }, areas.getUnitGraveyardCenterVector),
        new Row(false, false, true, RowEffected.ChooseN, new List<RowEffected>() { RowEffected.ChooseN }, areas.getCenterFront) // choose N
        };
    }

    void Start()
    {

        updateRowCenters();
    }

    public void buildDeck(int numPowers, int numUnits, int numKings, List<string> choosePower,
            List<string> chooseUnit, List<string> chooseKing, List<string> chooseUnitGraveyard,
            List<string> choosePowerGraveyard)
    {
        List<int> uniqueValues = new List<int>();

        Debug.Log("generating values");

        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {
            uniqueValues.Add(cardIndex);
        }

        Row powers = getRowByType(RowEffected.PowerDeck);
        Row units = getRowByType(RowEffected.UnitDeck);
        Row kings = getRowByType(RowEffected.KingDeck);
        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {

            int cardId = uniqueValues[Random.Range(0, uniqueValues.Count)];

            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.setIndex(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            Debug.Log("Making Card: " + clone.cardName);
            if (CardModel.isPower(clone.cardType))
            {
                powers.Add(clone);
            }
            else if (CardModel.isUnit(clone.cardType))
            {
                units.Add(clone);
            }
            else if (clone.cardType == CardType.King)
            {
                kings.Add(clone);
            }
            uniqueValues.Remove(cardId);
        }


        int numCards = numPowers + numUnits + numKings;
        Debug.Log("Dealing Cards");
        while (chooseUnitGraveyard.Count + choosePowerGraveyard.Count > 0)
        {
            if (chooseUnitGraveyard.Count > 0)
            {
                Card card = units.getCardByName(chooseUnitGraveyard[0]);
                chooseUnitGraveyard.RemoveAt(0);
                this.sendCardToGraveyard(units, card);
            }
            else if (choosePowerGraveyard.Count > 0)
            {
                Card card = powers.getCardByName(choosePowerGraveyard[0]);
                choosePowerGraveyard.RemoveAt(0);
                this.sendCardToGraveyard(powers, card);
            }
        }
        while (numCards > 0)
        {

            Debug.Log("Cards Left to Deal: " + numCards);
            Card card = baseCard;
            if (numPowers > 0)
            {
                card = powers[0];
                if (choosePower.Count > 0)
                {
                    card = powers.getCardByName(choosePower[0]);
                    choosePower.RemoveAt(0);
                }
                powers.Remove(card);
                numPowers--;
            }
            else if (numUnits > 0)
            {
                card = units[0];
                if (chooseUnit.Count > 0)
                {
                    card = units.getCardByName(chooseUnit[0]);
                    chooseUnit.RemoveAt(0);
                }
                units.Remove(card);
                numUnits--;
            }
            else if (numKings > 0)
            {
                card = kings[0];
                if (chooseKing.Count > 0)
                {
                    card = kings.getCardByName(chooseKing[0]);
                    chooseKing.RemoveAt(0);
                }
                kings.Remove(card);
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

    public void disactiveAllInDeck(bool multistep)
    {
        foreach (Row row in rows)
        {
            foreach (Card c in row)
            {
                if (c.isTargetActive() && !multistep)
                {
                    c.setTargetActive(false);
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
                row.cardTargetsActivated = false;
            }
        }
    }

    public List<Card> getVisibleCards()
    {
        List<Card> cards = new List<Card>();
        foreach (Row row in rows)
        {
            if (row.hasType(RowEffected.Player) || row.hasType(RowEffected.Enemy) || row.hasType(RowEffected.ChooseN))
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
                    if (card.isTargetActive())
                    {
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
            if (row.hasType(type))
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
    public Row getRowByTypes(List<RowEffected> types)
    {
        foreach (Row row in rows)
        {
            if (row.hasAllTypes(types))
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

    public void sendCardToGraveyard(Row currentRow, Card c)
    {
        currentRow.Remove(c);
        c.clearAttachments(this);
        if (CardModel.isPower(c.cardType))
        {
            this.getRowByType(RowEffected.PowerGraveyard).Add(c);
        }
        else if (CardModel.isUnit(c.cardType))
        {
            this.getRowByType(RowEffected.UnitGraveyard).Add(c);
        }
    }

    public void addCardToHand(Row currentRow, RowEffected playerHand, Card c)
    {
        currentRow.Remove(c);
        c.clearAttachments(this);
        c.resetSelectionCounts();
        getRowByType(playerHand).Add(c);
    }

    public RowEffected getKingRow(bool player)
    {
        List<Row> kingRows;
        if (player)
        {
            kingRows = getRowsByType(RowEffected.PlayerKing);
        }
        else
        {
            kingRows = getRowsByType(RowEffected.EnemyKing);
        }
        for (int i = 0; i < kingRows.Count; i++)
        {
            if (kingRows[i].Count > 0)
            {
                return kingRows[i].uniqueType;
            }
        }
        return RowEffected.None;
    }
    public Row getKingRowFromPlayRow(Row cardRow){
        return getRowByTypes(new List<RowEffected>{RowEffected.King, cardRow.getPlayer(), cardRow.getFullRowType()});
    }

    public void drawCard(Row drawDeck, bool player)
    {
        if (drawDeck.hasType(RowEffected.DrawableDeck))
        {
            if (drawDeck.Count > 0)
            {
                Card c = drawDeck[0];
                c.resetSelectionCounts();
                drawDeck.Remove(c);
                if (player)
                {
                    getRowByType(RowEffected.PlayerHand).Add(c);
                }
                else
                {
                    getRowByType(RowEffected.EnemyHand).Add(c);
                }

                c.loadMaterial();
            }
            else
            {
                Debug.Log("shuffle time");
            }
        }
        else
        {
            Debug.LogError("You weren't supposed to do that, drawDeck type error");
        }
    }

    public void drawCardGraveyard(Card c, Card targetCard)
    {
        Row graveyard = getRowByType(c.rowEffected);
        if (graveyard.Count >= c.graveyardCardDrawRemain)
        {

            for (int i = 0; i < c.graveyardCardDrawRemain; i++)
            {
                Card drawC = graveyard[0];
                graveyard.Remove(drawC);
                getRowByType(RowEffected.PlayerHand).Add(drawC);
                if (i == 0 && drawC.strength < c.strengthCondition)
                {
                    c.graveyardCardDrawRemain++;
                }
            }
        }
    }

    public void setCardAside(Row currentRow, Card c)
    {
        switch (c.setAsideType)
        {
            case SetAsideType.King: c.setAsideReturnRow = getKingRow(true); break;
            case SetAsideType.EnemyKing: c.setAsideReturnRow = getKingRow(false); break;
            case SetAsideType.Enemy: c.setAsideReturnRow = RowEffected.EnemyHand; break;
            case SetAsideType.Player: c.setAsideReturnRow = RowEffected.PlayerHand; break;
        }
        currentRow.Remove(c);
        if (currentRow.hasType(RowEffected.Enemy))
        {
            getRowByType(RowEffected.EnemySetAside).Add(c);
        }
        else if (currentRow.hasType(RowEffected.Player))
        {
            getRowByType(RowEffected.PlayerSetAside).Add(c);
        }
        else
        {
            Debug.LogError("You weren't supposed to do that, setCardAside error");
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
            if (c.isPlayable(this))
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

    public float scoreRow(RowEffected rowType)
    {
        //Row row = getRowByType(rowType);
        return 0;//row.scoreRow(this);
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
