using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private GameObject cardGameObject;
    private Card baseCard;

    public Areas areas;

    public List<Row> rows;
    private GameObject areasObject;

    private static int FRONTS_NUMBER = 102;
    // TODO - remove max amount of cards in each range group

    void Awake()
    {
        areasObject = GameObject.Instantiate(Resources.Load("Prefabs/Areas") as GameObject, transform.position, transform.rotation);
        areas = areasObject.GetComponent<Areas>();
        cardGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Card") as GameObject, new Vector3(-1000f, 0f, 0f), transform.rotation);
        baseCard = cardGameObject.GetComponent<Card>();
        baseCard.setBaseScale();
        rows = new List<Row>(){
        new Row(true, true, false, RowEffected.PlayerHand, new List<RowEffected>() { RowEffected.BothHand, RowEffected.PlayerHand, RowEffected.Player, RowEffected.Played}, areas.getDeckCenterVector),
        new Row(true, true, false, RowEffected.EnemyHand, new List<RowEffected>() { RowEffected.BothHand, RowEffected.EnemyHand, RowEffected.Enemy , RowEffected.Played}, areas.getDeckCenterVector),
        new Row(true, true, false, RowEffected.PlayerMelee, new List<RowEffected>() { RowEffected.PlayerMelee, RowEffected.PlayerPlayable, RowEffected.All, RowEffected.Melee, RowEffected.PlayerSwitchFront, RowEffected.Player, RowEffected.MeleeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getMeleeRowCenterVector,  () => areas.getScoreDisplayCenterVector(areas.getMeleeRowCenterVector)),
        new Row(true, true, false, RowEffected.PlayerRanged, new List<RowEffected>() {RowEffected.PlayerRanged, RowEffected.PlayerPlayable, RowEffected.All, RowEffected.Ranged, RowEffected.PlayerSwitchFront, RowEffected.Player, RowEffected.RangedFull , RowEffected.Played, RowEffected.CleanUp}, areas.getRangedRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getRangedRowCenterVector)),
        new Row(true, true, false, RowEffected.PlayerSiege, new List<RowEffected>() { RowEffected.PlayerSiege, RowEffected.PlayerPlayable, RowEffected.All, RowEffected.Siege, RowEffected.Player, RowEffected.SiegeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getSiegeRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getSiegeRowCenterVector)),
        new Row(true, true, false, RowEffected.EnemyMelee, new List<RowEffected>() {RowEffected.EnemyMelee, RowEffected.EnemyPlayable, RowEffected.All, RowEffected.Melee, RowEffected.EnemySwitchFront, RowEffected.Enemy, RowEffected.MeleeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getEnemyMeleeRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getEnemyMeleeRowCenterVector)),
        new Row(true, true, false, RowEffected.EnemyRanged, new List<RowEffected>() {RowEffected.EnemyRanged, RowEffected.EnemyPlayable, RowEffected.All, RowEffected.Ranged, RowEffected.EnemySwitchFront, RowEffected.Enemy, RowEffected.RangedFull , RowEffected.Played, RowEffected.CleanUp}, areas.getEnemyRangedRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getEnemyRangedRowCenterVector)),
        new Row(true, true, false, RowEffected.EnemySiege, new List<RowEffected>() {RowEffected.EnemySiege, RowEffected.EnemyPlayable, RowEffected.All, RowEffected.Siege, RowEffected.Enemy, RowEffected.SiegeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getEnemySiegeRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getEnemySiegeRowCenterVector)),
        new Row(false, false, true, RowEffected.UnitDeck, new List<RowEffected>() { RowEffected.DrawableDeck, RowEffected.UnitDeck }, areas.getUnitDeckCenterVector),
        new Row(false, false, true, RowEffected.PowerDeck, new List<RowEffected>() { RowEffected.DrawableDeck, RowEffected.PowerDeck }, areas.getPowerDeckCenterVector),
        new Row(false, false, true, RowEffected.KingDeck, new List<RowEffected>() { RowEffected.KingDeck }, areas.getKingDeckCenterVector),
        new Row(false, false, false, RowEffected.UnitGraveyard, new List<RowEffected>() { RowEffected.UnitGraveyard , RowEffected.Played, RowEffected.PlayableGraveyard, RowEffected.Graveyard}, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, RowEffected.PowerGraveyard, new List<RowEffected>() { RowEffected.PowerGraveyard , RowEffected.Played, RowEffected.PlayableGraveyard, RowEffected.Graveyard}, areas.getPowerGraveyardCenterVector),
        new Row(false, false, false, RowEffected.KingGraveyard, new List<RowEffected>() { RowEffected.KingGraveyard , RowEffected.Played, RowEffected.Graveyard}, areas.getKingGraveyardCenterVector),
        new Row(true, false, false, RowEffected.PlayerMeleeKing, new List<RowEffected>() { RowEffected.PlayerMeleeKing, RowEffected.PlayerKing, RowEffected.Player, RowEffected.King, RowEffected.MeleeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getMeleeKingCenterVector),
        new Row(true, false, false, RowEffected.PlayerRangedKing, new List<RowEffected>() { RowEffected.PlayerRangedKing, RowEffected.PlayerKing, RowEffected.Player, RowEffected.King, RowEffected.RangedFull , RowEffected.Played, RowEffected.CleanUp}, areas.getRangedKingCenterVector),
        new Row(true, false, false, RowEffected.PlayerSiegeKing, new List<RowEffected>() { RowEffected.PlayerSiegeKing, RowEffected.PlayerKing, RowEffected.Player, RowEffected.King, RowEffected.SiegeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getSiegeKingCenterVector),
        new Row(true, false, false, RowEffected.EnemyMeleeKing, new List<RowEffected>() { RowEffected.EnemyMeleeKing, RowEffected.EnemyKing, RowEffected.Enemy, RowEffected.King, RowEffected.MeleeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getEnemyMeleeKingCenterVector),
        new Row(true, false, false, RowEffected.EnemyRangedKing, new List<RowEffected>() {  RowEffected.EnemyRangedKing, RowEffected.EnemyKing, RowEffected.Enemy, RowEffected.King, RowEffected.RangedFull , RowEffected.Played, RowEffected.CleanUp}, areas.getEnemyRangedKingCenterVector),
        new Row(true, false, false, RowEffected.EnemySiegeKing, new List<RowEffected>() { RowEffected.EnemySiegeKing, RowEffected.EnemyKing, RowEffected.Enemy, RowEffected.King, RowEffected.SiegeFull , RowEffected.Played, RowEffected.CleanUp}, areas.getEnemySiegeKingCenterVector),
        new Row(false, false, false, RowEffected.PlayerSetAside, new List<RowEffected>() { RowEffected.PlayerSetAside, RowEffected.Player , RowEffected.Played, RowEffected.SetAside}, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, RowEffected.EnemySetAside, new List<RowEffected>() { RowEffected.EnemySetAside, RowEffected.Enemy , RowEffected.Played, RowEffected.SetAside}, areas.getUnitGraveyardCenterVector),
        new Row(false, true, false, RowEffected.ChooseN, new List<RowEffected>() { RowEffected.ChooseN }, areas.getCenterFront),
        new Row(false, RowEffected.Pass, "Pass", areas.getPassButtonCenterVector, Game.playerPass),
        new Row(false, RowEffected.Skip,"Skip" , areas.getSkipButtonCenterVector, () => Game.skipActiveCardEffects())
        };
        Debug.Log("Making Deck");
    }

    void Start()
    {
        updateRowCenters();
    }

    public void buildDeck(int numPowers, int numUnits, int numKings, List<string> choosePower,
            List<string> chooseUnit, List<string> chooseKing, List<string> chooseUnitGraveyard,
            List<string> choosePowerGraveyard, List<string> enemyPower,
            List<string> enemyUnit, List<string> enemyKing)
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

            int cardId = uniqueValues[UnityEngine.Random.Range(0, uniqueValues.Count)];

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

        Debug.Log("Dealing Player Cards");
        while (chooseUnitGraveyard.Count + choosePowerGraveyard.Count > 0)
        {
            if (chooseUnitGraveyard.Count > 0)
            {
                Card card = units.getCardByName(chooseUnitGraveyard[0]);
                chooseUnitGraveyard.RemoveAt(0);
                card.loadCardFrontMaterial();
                this.sendCardToGraveyard(units, RowEffected.None, card);
            }
            else if (choosePowerGraveyard.Count > 0)
            {
                Card card = powers.getCardByName(choosePowerGraveyard[0]);
                choosePowerGraveyard.RemoveAt(0);
                card.loadCardFrontMaterial();
                this.sendCardToGraveyard(powers, RowEffected.None, card);
            }
        }
        dealHand(numPowers, numUnits, numKings, choosePower, chooseUnit, chooseKing, RowEffected.PlayerHand);
        dealHand(numPowers, numUnits, numKings, enemyPower, enemyUnit, enemyKing, RowEffected.EnemyHand);
        getRowByType(RowEffected.EnemyHand).setVisibile(false);
        updateRowCenters();
    }

    public void dealHand(int numPowers, int numUnits, int numKings,
    List<string> choosePower, List<string> chooseUnit, List<string> chooseKing, RowEffected playerHand)
    {

        Row powers = getRowByType(RowEffected.PowerDeck);
        Row units = getRowByType(RowEffected.UnitDeck);
        Row kings = getRowByType(RowEffected.KingDeck);
        int numCards = numPowers + numUnits + numKings;
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
            card.loadCardFrontMaterial();
            getRowByType(playerHand).Add(card);
            numCards = numPowers + numUnits + numKings;

        }
    }
    public Card getCardByName(string cardName)
    {
        List<Row> searchRows = getRowsByType(RowEffected.Played);
        foreach (Row r in searchRows)
        {
            Card c = r.getCardByName(cardName);
            if (c != null)
            {
                return c;
            }
        }
        return null;
    }
    public Card dealCardToRow(string cardName, RowEffected row)
    {

        Row powers = getRowByType(RowEffected.PowerDeck);
        Row units = getRowByType(RowEffected.UnitDeck);
        Row kings = getRowByType(RowEffected.KingDeck);
        Row deck;
        if (powers.ContainsCard(cardName))
        {
            deck = powers;
        }
        else if (units.ContainsCard(cardName))
        {
            deck = units;
        }
        else if (kings.ContainsCard(cardName))
        {
            deck = kings;
        }
        else
        {
            return null;
        }
        Card card = deck.getCardByName(cardName);
        deck.Remove(card);
        card.loadCardFrontMaterial();
        card.resetSelectionCounts();
        getRowByType(row).Add(card);
        return card;

    }

    public void dealListToRow(List<string> choosePower, List<string> chooseUnit, List<string> chooseKing, RowEffected row)
    {
        Row powers = getRowByType(RowEffected.PowerDeck);
        Row units = getRowByType(RowEffected.UnitDeck);
        Row kings = getRowByType(RowEffected.KingDeck);
        Card card = baseCard;
        for (int i = 0; i < choosePower.Count; i++)
        {
            card = powers[0];
            if (choosePower.Count > 0)
            {
                card = powers.getCardByName(choosePower[0]);
                choosePower.RemoveAt(0);
            }
            powers.Remove(card);
        }
        for (int i = 0; i < chooseUnit.Count; i++)
        {
            card = units[0];
            if (chooseUnit.Count > 0)
            {
                card = units.getCardByName(chooseUnit[0]);
                chooseUnit.RemoveAt(0);
            }
            units.Remove(card);
        }
        for (int i = 0; i < chooseKing.Count; i++)
        {
            card = kings[0];
            if (chooseKing.Count > 0)
            {
                card = kings.getCardByName(chooseKing[0]);
                chooseKing.RemoveAt(0);
            }
            kings.Remove(card);
        }
        card.loadCardFrontMaterial();
        getRowByType(row).Add(card);
    }

    public void resetDeck()
    {
        List<Row> playedRows = getRowsByType(RowEffected.Played);
        List<Card> playedCards = getCardsInRowByType(RowEffected.Played);
        foreach (Row r in playedRows)
        {
            foreach (Card c in playedCards)
            {
                if (r.Contains(c))
                {
                    resetCard(r, c);
                }


            }
        }
    }
    public void resetCard(Row currentRow, Card c)
    {
        currentRow.Remove(c);
        c.setTargetActive(false);
        foreach (Card attachment in c.attachments)
        {
            resetCard(currentRow, attachment);
        }
        c.attachments.RemoveAll(delegate (Card c) { return true; });
        RowEffected deck = CardModel.isPower(c.cardType) ? RowEffected.PowerDeck : (CardModel.isUnit(c.cardType) ? RowEffected.UnitDeck : RowEffected.KingDeck);
        this.getRowByType(deck).Add(c);
    }





    public void updateRowCenters()
    {
        foreach (Row row in rows)
        {
            row.centerRow();
        }
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

    public int maxStrength(RowEffected rowType)
    {
        List<Row> destroyRows = getRowsByType(rowType);
        int max = 0;
        foreach (Row r in destroyRows)
        {
            RowEffected player = CardModel.getPlayerFromRow(r.uniqueType);
            int temp = r.maxStrength(this, player);
            if (temp > max)
            {
                max = temp;
            }
        }
        return max;
    }

    public List<Card> getVisibleCards(State state)
    {
        List<Card> cards = new List<Card>();
        foreach (Row row in rows)
        {
            if (((state != State.CHOOSE_N && state != State.REVEAL) && (row.hasType(RowEffected.Player) || row.hasType(RowEffected.Enemy)))
            || ((state == State.CHOOSE_N || state == State.REVEAL) && (row.hasType(RowEffected.PlayerHand) || row.hasType(RowEffected.ChooseN))))
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
    public List<Tuple<Card, RowEffected>> getActiveCardTargetsAndRows()
    {
        List<Tuple<Card, RowEffected>> cardTargets = new List<Tuple<Card, RowEffected>>();
        foreach (Row row in rows)
        {
            if (row.isPlayer)
            {
                foreach (Card card in row)
                {
                    if (card.isTargetActive())
                    {
                        cardTargets.Add(new Tuple<Card, RowEffected>(card, row.uniqueType));
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
            if (row.target != null && row.target.isTargetActive())
            {
                rowTargets.Add(row);
            }
        }
        return rowTargets;
    }

    public void activateRowsByType(bool state, bool individualCards, RowEffected type)
    {
        Debug.Log("Activating Type: " + type);
        List<Row> rowList = getRowsByType(type);
        foreach (Row row in rowList)
        {
            Debug.Log("Activating Row: " + row + " Count: " + Game.activeDeck.getRowByType(row.uniqueType).Count);
            row.setActivateRowCardTargets(state, individualCards);
        }
    }
    public void activateAllRowsByType(bool state, bool individualCards, List<RowEffected> types)
    {

        foreach (RowEffected type in types)
        {
            Debug.Log("Activating Row: " + type + " Count: " + Game.activeDeck.getRowByType(type).Count);
            this.activateRowsByType(state, individualCards, type);
        }
    }
    public void activateRowsByTypeExclude(bool state, bool individualCards, RowEffected type, RowEffected exclude)
    {
        
        Debug.Log("Activating Type: " + type + " Exclude: " + exclude);
        List<Row> rowList = getRowsByType(type);
        foreach (Row row in rowList)
        {
            if (row.uniqueType != exclude)
            { 
                Debug.Log("Activating Row: " + type + " Count: " + Game.activeDeck.getRowByType(row.uniqueType).Count);
                row.setActivateRowCardTargets(state, individualCards);
            }
        }
    }

    public int countCardsInRows(RowEffected type)
    {
        int sum = 0;
        foreach (Row row in rows)
        {
            if (row.hasType(type))
            {
                sum += row.Count;
            }
        }
        return sum;
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
    public List<Card> getCardsInRowByType(RowEffected type)
    {
        List<Card> cardList = new List<Card>();
        foreach (Row row in rows)
        {
            foreach (Card c in row)
            {
                cardList.Add(c);
            }
        }
        return cardList;
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

    public void sendCardToGraveyard(Row currentRow, RowEffected playerHand, Card c)
    {
        currentRow.Remove(c);
        c.setTargetActive(false);
        c.clearAttachments(this);
        //c.resetSelectionCounts();
        RowEffected graveyard = CardModel.isPower(c.cardType) ? RowEffected.PowerGraveyard : (CardModel.isUnit(c.cardType) ? RowEffected.UnitGraveyard : RowEffected.KingGraveyard);
        this.getRowByType(graveyard).Add(c);
    }

    public void addCardToHand(Row currentRow, RowEffected playerHand, Card c)
    {
        currentRow.Remove(c);
        c.clearAttachments(this);
        c.setTargetActive(false);
        c.resetSelectionCounts();
        if (currentRow.uniqueType == RowEffected.UnitGraveyard && c.graveyardCardDrawRemain > 0)
        {
            drawCardGraveyard(c, null);
        }
        getRowByType(playerHand).Add(c);
    }

    public void addCardToHandMultiply(Row currentRow, RowEffected playerHand, Card c)
    {
        currentRow.Remove(c);
        c.clearAttachments(this);
        c.setTargetActive(false);
        c.resetSelectionCounts();
        Game.activeCard.strengthMultiple++;
        if (currentRow.uniqueType == RowEffected.UnitGraveyard && c.graveyardCardDrawRemain > 0)
        {
            drawCardGraveyard(c, null);
        }
        getRowByType(playerHand).Add(c);
    }

    public RowEffected getKingRow(RowEffected player)
    {

        List<Row> kingRows;
        kingRows = getRowsByType(CardModel.getRowFromSide(player, RowEffected.PlayerKing));

        for (int i = 0; i < kingRows.Count; i++)
        {
            if (kingRows[i].Count > 0)
            {
                return kingRows[i].uniqueType;
            }
        }
        return RowEffected.None;
    }
    public Row getKingRowFromPlayRow(Row cardRow)
    {
        return getRowByTypes(new List<RowEffected> { RowEffected.King, cardRow.getPlayer(), cardRow.getFullRowType() });
    }

    public void drawCard(Row drawDeck, RowEffected player)
    {
        if (drawDeck.hasType(RowEffected.DrawableDeck))
        {
            if (drawDeck.Count > 0)
            {
                Card c = drawDeck[0];
                c.resetSelectionCounts();
                drawDeck.Remove(c);
                getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerHand)).Add(c);
                c.loadCardFrontMaterial();
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

        Debug.Log("Drawing from Graveyard: " + c.cardName + targetCard);
        Row graveyard = getRowByType(c.rowEffected);
        int cardsDrawn = 0;
        if (graveyard.Count >= c.graveyardCardDrawRemain)
        {

            for (int i = 0; i < c.graveyardCardDrawRemain; i++)
            {
                Card drawC = graveyard[graveyard.Count - 1];
                graveyard.Remove(drawC);
                getRowByType(RowEffected.PlayerHand).Add(drawC);
                cardsDrawn++;
                if (i == 0 && drawC.strength < c.strengthCondition)
                {
                    c.graveyardCardDrawRemain++;
                }
                if (CardModel.isUnit(drawC.cardType) && drawC.graveyardCardDraw > 0)
                {
                    c.graveyardCardDrawRemain += drawC.graveyardCardDraw;
                }
            }
        }

        c.graveyardCardDrawRemain -= cardsDrawn;
    }

    public void setCardAside(Row currentRow, Card c, SetAsideType setAsideType)
    {
        RowEffected player = CardModel.getPlayerFromRow(currentRow.uniqueType);
        Debug.Log("GREMLIN: card: " + c.cardName + " player: " + player + " row: " + currentRow.uniqueType + " setAsideType: " + c.setAsideType);
        switch (setAsideType)
        {
            case SetAsideType.King: c.setAsideReturnRow = getKingRow(player); break;
            case SetAsideType.EnemyKing: c.setAsideReturnRow = getKingRow(player); break;
            case SetAsideType.EitherKing: c.setAsideReturnRow = getKingRow(player); break;
            case SetAsideType.Enemy: c.setAsideReturnRow = CardModel.getRowFromSide(player, RowEffected.EnemyHand); break;
            case SetAsideType.Player: c.setAsideReturnRow = CardModel.getRowFromSide(player, RowEffected.PlayerHand); break;
        }
        Debug.Log(c.setAsideReturnRow);
        currentRow.Remove(c);
        getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerSetAside)).Add(c);

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


    public Card getPlayableCard(RowEffected player)
    {
        foreach (Card c in getRowByType(CardModel.getRowFromSide(player, RowEffected.PlayerHand)))
        {
            if (c.isPlayable(player))
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
    public void scoreRows(RowEffected rowType)
    {
        foreach (Row r in getRowsByType(rowType))
        {
            r.scoreRow(this, CardModel.getPlayerFromRow(r.uniqueType));
        }
    }

    public List<RowEffected> getMaxScoreRows(RowEffected rowType)
    {
        List<Row> rows = getRowsByType(rowType);
        List<RowEffected> maxRows = new List<RowEffected>();
        float max = -1;
        for (int i = 0; i < rows.Count; i++)
        {
            Row row = rows[i];
            float temp = row.scoreRow(this, CardModel.getPlayerFromRow(row.uniqueType));
            if (temp > max)
            {
                maxRows = new List<RowEffected>() { row.uniqueType };
                max = temp;
            }
            else if (temp == max)
            {
                maxRows.Add(row.uniqueType);
            }
        }
        return maxRows;
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

    public Row getCardRow(Card card)
    {
        foreach (Row r in rows)
        {

            if (r.ContainsIncludeAttachments(card))
            {
                return r;
            }
        }
        return null;
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

    public Card getStrongestCard(RowEffected rowEffected, Card exclude)
    {
        List<Row> searchRows = getRowsByType(rowEffected);
        int maxStrength = 0;
        Card strongest = null;
        foreach (Row row in searchRows)
        {
            foreach (Card card in row)
            {
                if (card.calculatedStrength > maxStrength && !card.Equals(exclude))
                {
                    maxStrength = card.calculatedStrength;
                    strongest = card;
                }
            }
        }
        return strongest;
    }
    public delegate List<Card> CardSelect(Row r);

    public void sendAllToGraveYard(RowEffected rowType, CardSelect cardSelection)
    {
        Debug.Log("Sending all to graveyard: " + rowType);
        List<Row> destroyRows = getRowsByType(rowType);
        List<Card> graveyardList = new List<Card>();
        List<Row> graveyardRowList = new List<Row>();

        foreach (Row r in destroyRows)
        {
            if (r.Count > 0)
            {
                List<Card> selection = cardSelection(r);
                Debug.Log(r + " " + selection + " graveyardList: " + graveyardList);
                foreach (Card c in selection)
                {

                    graveyardList.Add(c);
                    graveyardRowList.Add(r);
                }
            }
        }
        for (int i = 0; i < graveyardList.Count; i++)
        {   
            
            Debug.Log(graveyardRowList[i] + " " + graveyardList[i]);
            sendCardToGraveyard(graveyardRowList[i], RowEffected.None, graveyardList[i]);
        }
    }

    internal void sendListToGraveyard(List<Card> discardList, RowEffected currentRow)
    {
        Row r  = getRowByType(currentRow);
        foreach(Card c in discardList){
            sendCardToGraveyard(r, RowEffected.None, c);
        }
    }
}
