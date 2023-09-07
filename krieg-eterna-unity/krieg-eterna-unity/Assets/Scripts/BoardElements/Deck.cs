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

    private static int FRONTS_NUMBER = 118;
    // TODO - remove max amount of cards in each range group

    void Awake()
    {
        areasObject = GameObject.Instantiate(Resources.Load("Prefabs/Areas") as GameObject, transform.position, transform.rotation);
        areas = areasObject.GetComponent<Areas>();
        cardGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Card") as GameObject, new Vector3(-1000f, 0f, 0f), transform.rotation);
        baseCard = cardGameObject.GetComponent<Card>();
        baseCard.setBaseScale();
        rows = new List<Row>(){
        new Row(true, true, false, true, RowEffected.PlayerHand, new List<RowEffected>() {
            RowEffected.BothHand,
            RowEffected.PlayerHand,
            RowEffected.Player,
            RowEffected.Played}, areas.getDeckCenterVector),
        new Row(true, true, false, false, RowEffected.EnemyHand, new List<RowEffected>() {
            RowEffected.BothHand,
            RowEffected.EnemyHand,
            RowEffected.Enemy ,
            RowEffected.Played}, areas.getDeckCenterTopVector),
        new Row(true, true, false, true, RowEffected.PlayerMelee, new List<RowEffected>() {
            RowEffected.PlayerMelee,
            RowEffected.PlayerPlayable,
            RowEffected.All,
            RowEffected.Melee,
            RowEffected.MeleeOrRanged,
            RowEffected.MeleeOrSiege,
            RowEffected.PlayerMeleeOrRanged,
            RowEffected.PlayerMeleeOrSiege,
            RowEffected.Player,
            RowEffected.MeleeFull,
            RowEffected.Played,
            RowEffected.CleanUp}, areas.getMeleeRowCenterVector,  () => areas.getScoreDisplayCenterVector(areas.getMeleeRowCenterVector)),
        new Row(true, true, false, true, RowEffected.PlayerRanged, new List<RowEffected>() {
            RowEffected.PlayerRanged,
            RowEffected.PlayerPlayable,
            RowEffected.All,
            RowEffected.Ranged,
            RowEffected.MeleeOrRanged,
            RowEffected.RangedOrSiege,
            RowEffected.PlayerMeleeOrRanged,
            RowEffected.PlayerRangedOrSiege,
            RowEffected.Player,
            RowEffected.RangedFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, areas.getRangedRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getRangedRowCenterVector)),
        new Row(true, true, false, true, RowEffected.PlayerSiege, new List<RowEffected>() {
            RowEffected.PlayerSiege,
            RowEffected.PlayerPlayable,
            RowEffected.All,
            RowEffected.Siege,
            RowEffected.MeleeOrSiege,
            RowEffected.RangedOrSiege,
            RowEffected.PlayerMeleeOrSiege,
            RowEffected.PlayerRangedOrSiege,
            RowEffected.Player,
            RowEffected.SiegeFull,
            RowEffected.Played,
            RowEffected.CleanUp}, areas.getSiegeRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getSiegeRowCenterVector)),
        new Row(true, true, false, true, RowEffected.EnemyMelee, new List<RowEffected>() {
            RowEffected.EnemyMelee,
            RowEffected.EnemyPlayable,
            RowEffected.All,
            RowEffected.Melee,
            RowEffected.MeleeOrRanged,
            RowEffected.MeleeOrSiege,
            RowEffected.EnemyMeleeOrRanged,
            RowEffected.EnemyMeleeOrSiege,
            RowEffected.Enemy,
            RowEffected.MeleeFull,
            RowEffected.Played,
            RowEffected.CleanUp}, areas.getEnemyMeleeRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getEnemyMeleeRowCenterVector)),
        new Row(true, true, false, true, RowEffected.EnemyRanged, new List<RowEffected>() {
            RowEffected.EnemyRanged,
            RowEffected.EnemyPlayable,
            RowEffected.All,
            RowEffected.Ranged,
            RowEffected.MeleeOrRanged,
            RowEffected.RangedOrSiege,
            RowEffected.EnemyMeleeOrRanged,
            RowEffected.EnemyRangedOrSiege,
            RowEffected.Enemy,
            RowEffected.RangedFull,
            RowEffected.Played,
            RowEffected.CleanUp}, areas.getEnemyRangedRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getEnemyRangedRowCenterVector)),
        new Row(true, true, false, true, RowEffected.EnemySiege, new List<RowEffected>() {
            RowEffected.EnemySiege,
            RowEffected.EnemyPlayable,
            RowEffected.All,
            RowEffected.Siege,
            RowEffected.Enemy,
            RowEffected.SiegeFull,
            RowEffected.MeleeOrSiege,
            RowEffected.RangedOrSiege,
            RowEffected.EnemyMeleeOrSiege,
            RowEffected.EnemyRangedOrSiege,
            RowEffected.Played,
            RowEffected.CleanUp}, areas.getEnemySiegeRowCenterVector, () => areas.getScoreDisplayCenterVector(areas.getEnemySiegeRowCenterVector)),
        new Row(false, false, true, true, RowEffected.UnitDeck, new List<RowEffected>() {
            RowEffected.DrawableDeck,
            RowEffected.UnitDeck,
            RowEffected.Deck }, areas.getUnitDeckCenterVector),
        new Row(false, false, true, true, RowEffected.PowerDeck, new List<RowEffected>() {
            RowEffected.DrawableDeck,
            RowEffected.PowerDeck,
            RowEffected.Deck }, areas.getPowerDeckCenterVector),
        new Row(false, false, true, true, RowEffected.KingDeck, new List<RowEffected>() {
            RowEffected.KingDeck,
            RowEffected.Deck }, areas.getKingDeckCenterVector),
        new Row(false, false, false, true, RowEffected.UnitGraveyard, new List<RowEffected>() {
            RowEffected.UnitGraveyard ,
            RowEffected.Played,
            RowEffected.PlayableGraveyard,
            RowEffected.Graveyard}, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, true, RowEffected.PowerGraveyard, new List<RowEffected>() {
            RowEffected.PowerGraveyard ,
            RowEffected.Played,
            RowEffected.PlayableGraveyard,
            RowEffected.Graveyard}, areas.getPowerGraveyardCenterVector),
        new Row(false, false, false, true, RowEffected.KingGraveyard, new List<RowEffected>() {
            RowEffected.KingGraveyard ,
            RowEffected.Played,
            RowEffected.Graveyard}, areas.getKingGraveyardCenterVector),
        new Row(true, false, false, true, RowEffected.PlayerMeleeKing, new List<RowEffected>() {
            RowEffected.PlayerMeleeKing,
            RowEffected.PlayerKing,
            RowEffected.Player,
            RowEffected.King,
            RowEffected.MeleeFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, () => areas.getKingCenterVector(areas.getMeleeRowCenterVector)),
        new Row(true, false, false, true, RowEffected.PlayerRangedKing, new List<RowEffected>() {
            RowEffected.PlayerRangedKing,
            RowEffected.PlayerKing,
            RowEffected.Player,
            RowEffected.King,
            RowEffected.RangedFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, () => areas.getKingCenterVector(areas.getRangedRowCenterVector)),
        new Row(true, false, false, true, RowEffected.PlayerSiegeKing, new List<RowEffected>() {
            RowEffected.PlayerSiegeKing,
            RowEffected.PlayerKing,
            RowEffected.Player,
            RowEffected.King,
            RowEffected.SiegeFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, () => areas.getKingCenterVector(areas.getSiegeRowCenterVector)),
        new Row(true, false, false, true, RowEffected.EnemyMeleeKing, new List<RowEffected>() {
            RowEffected.EnemyMeleeKing,
            RowEffected.EnemyKing,
            RowEffected.Enemy,
            RowEffected.King,
            RowEffected.MeleeFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, () => areas.getKingCenterVector(areas.getEnemyMeleeRowCenterVector)),
        new Row(true, false, false, true, RowEffected.EnemyRangedKing, new List<RowEffected>() {
            RowEffected.EnemyRangedKing,
            RowEffected.EnemyKing,
            RowEffected.Enemy,
            RowEffected.King,
            RowEffected.RangedFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, () => areas.getKingCenterVector(areas.getEnemyRangedRowCenterVector)),
        new Row(true, false, false, true, RowEffected.EnemySiegeKing, new List<RowEffected>() {
            RowEffected.EnemySiegeKing,
            RowEffected.EnemyKing,
            RowEffected.Enemy,
            RowEffected.King,
            RowEffected.SiegeFull ,
            RowEffected.Played,
            RowEffected.CleanUp}, () => areas.getKingCenterVector(areas.getEnemySiegeRowCenterVector)),
        new Row(false, false, false, true, RowEffected.PlayerSetAside, new List<RowEffected>() {
            RowEffected.PlayerSetAside,
            RowEffected.Player ,
            RowEffected.Played,
            RowEffected.SetAside}, areas.getUnitGraveyardCenterVector),
        new Row(false, false, false, true, RowEffected.EnemySetAside, new List<RowEffected>() {
            RowEffected.EnemySetAside,
            RowEffected.Enemy ,
            RowEffected.Played,
            RowEffected.SetAside}, areas.getUnitGraveyardCenterVector),
        new Row(false, true, false, true, RowEffected.PlayerChooseN, new List<RowEffected>() {
            RowEffected.PlayerChooseN,
            RowEffected.ChooseN }, areas.getCenterFront),
        new Row(false, true, false, false, RowEffected.EnemyChooseN, new List<RowEffected>() {
            RowEffected.EnemyChooseN,
            RowEffected.ChooseN }, areas.getCenterFront),
        new Row(false, true, false, true, RowEffected.PlayerAltEffectRow, new List<RowEffected>() {
            RowEffected.PlayerAltEffectRow,
            RowEffected.AltEffectRow }, areas.getSiegeRowCenterVector),
        new Row(false, true, false, false, RowEffected.EnemyAltEffectRow, new List<RowEffected>() {
            RowEffected.EnemyAltEffectRow,
            RowEffected.AltEffectRow }, areas.getSiegeRowCenterVector),
        new Row(false, RowEffected.Pass, "Pass", areas.getPassButtonCenterVector, Game.playerPass, Game.canPass),
        new Row(false, RowEffected.Skip,"Skip" , areas.getSkipButtonCenterVector, () => Game.skipActiveCardEffects(), Game.canSkip)
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
        List<int> uniqueValues = new List<int>(FRONTS_NUMBER);

        Debug.Log("generating values");
        var sw = System.Diagnostics.Stopwatch.StartNew();
        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {
            uniqueValues.Add(cardIndex);
        }
        sw.Stop();
        Debug.Log("indexlist setup Time elapsed: " + sw.Elapsed);
        sw.Restart();
        Row powers = getRowByType(RowEffected.PowerDeck);
        Row units = getRowByType(RowEffected.UnitDeck);
        Row kings = getRowByType(RowEffected.KingDeck);
        List<Card> alts = new List<Card>();
        for (int cardIndex = 0; cardIndex < FRONTS_NUMBER; cardIndex++)
        {
            if (Game.random == null)
            {
                break;
            }
            int cardId = uniqueValues[Game.random.Next(uniqueValues.Count)];

            Card clone = Instantiate(baseCard) as Card;
            clone.tag = "CloneCard";
            clone.initCard(cardId);
            clone.setIsSpecial(clone.getCardModel().getIsSpecial(cardId));
            clone.setBaseLoc();
            if (clone.isAltEffect)
            {
                alts.Add(clone);
            }
            else if (CardModel.isPower(clone.cardType))
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
        foreach (Card c in alts)
        {
            Card main = this.getCardInDeckByName(c.mainCardName);
            if (main == null)
            {
                Debug.LogError("Unknown Card: " + c.mainCardName);
            }
            main.altEffects.Add(c);
        }
        sw.Stop();
        Debug.Log("card cloning Time elapsed: " + sw.Elapsed);
        sw.Restart();
        Debug.Log("Dealing Player Cards");
        while (chooseUnitGraveyard.Count + choosePowerGraveyard.Count > 0)
        {
            if (chooseUnitGraveyard.Count > 0)
            {
                Card card = units.getCardByName(chooseUnitGraveyard[0]);
                chooseUnitGraveyard.RemoveAt(0);
                card.loadCardFront();
                this.sendCardToGraveyard(units, RowEffected.None, card);
            }
            else if (choosePowerGraveyard.Count > 0)
            {
                Card card = powers.getCardByName(choosePowerGraveyard[0]);
                choosePowerGraveyard.RemoveAt(0);
                card.loadCardFront();
                this.sendCardToGraveyard(powers, RowEffected.None, card);
            }
        }
        if (!Game.net.isNetworkGame || Game.net.host)
        {
            dealHand(numPowers, numUnits, numKings, choosePower, chooseUnit, chooseKing, RowEffected.PlayerHand);
            dealHand(numPowers, numUnits, numKings, enemyPower, enemyUnit, enemyKing, RowEffected.EnemyHand);
        }
        else
        {
            dealHand(numPowers, numUnits, numKings, enemyPower, enemyUnit, enemyKing, RowEffected.EnemyHand);
            dealHand(numPowers, numUnits, numKings, choosePower, chooseUnit, chooseKing, RowEffected.PlayerHand);
        }
        getRowByType(RowEffected.EnemyHand).setVisibile(false);
        updateRowCenters();
        sw.Stop();
        Debug.Log("card dealing Time elapsed: " + sw.Elapsed);
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
    public Card getCardInDeckByName(string cardName)
    {
        List<Row> searchRows = getRowsByType(RowEffected.Deck);
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
        card.loadCardFront();
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
        card.loadCardFront();
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
                    resetCard(r, r.Find((x) => x.Equals(c)));
                }


            }
        }
    }
    public void resetCard(Row currentRow, Card c)
    {
        //Debug.Log("reseting Card: " + c + " " + currentRow.uniqueType);
        currentRow.Remove(c);
        c.setTargetActive(false);
        foreach (Card attachment in c.attachments)
        {
            resetCard(currentRow, attachment);
        }
        c.attachments.RemoveAll(delegate (Card c) { return true; });
        if (c.isClone)
        {
            c.Destroy();
            Debug.Log("Reeeeeeee");
        }
        else
        {
            RowEffected deck = CardModel.isPower(c.cardType) ? RowEffected.PowerDeck : (CardModel.isUnit(c.cardType) ? RowEffected.UnitDeck : RowEffected.KingDeck);
            this.getRowByType(deck).Add(c);
        }
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
        Debug.Log("Disactivating all in deck multistep: " + multistep);
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
                if (c.isAltEffect){
                    c.setVisible(false);
                }
            }
            if (row.target != null && !row.hasType(RowEffected.ChooseN))
            {
                row.target.setNotFlashing();
                row.target.setTargetActive(false);
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
            || ((state == State.CHOOSE_N || state == State.REVEAL) && (row.hasType(RowEffected.PlayerHand) || row.hasType(RowEffected.PlayerChooseN))))
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
            foreach (Card card in row)
            {
                if (card.isTargetActive())
                {
                    cardTargets.Add(card);
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
            foreach (Card card in row)
            {
                if (card.isTargetActive())
                {
                    cardTargets.Add(new Tuple<Card, RowEffected>(card, row.uniqueType));
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
            if (row.target != null && (row.target.isTargetActive() || row.target.isEnemyRowTargetActive()))
            {
                rowTargets.Add(row);
            }
        }
        return rowTargets;
    }

    public void activateRowsByType(bool state, bool individualCards, bool unitsOnly, RowEffected type)
    {
        Debug.Log("Activating Type: " + type);
        List<Row> rowList = getRowsByType(type);
        foreach (Row row in rowList)
        {
            Debug.Log("Activating Row: " + row + " Count: " + Game.activeDeck.getRowByType(row.uniqueType).Count);
            row.setActivateRowCardTargets(state, individualCards, unitsOnly);
        }
    }
    public void activateAllRowsByType(bool state, bool individualCards, bool unitsOnly, List<RowEffected> types)
    {

        foreach (RowEffected type in types)
        {
            Debug.Log("Activating Row: " + type + " Count: " + Game.activeDeck.getRowByType(type).Count);
            this.activateRowsByType(state, individualCards, unitsOnly, type);
        }
    }
    public void activateRowsByTypeExclude(bool state, bool individualCards, bool unitsOnly, RowEffected type, RowEffected exclude)
    {

        Debug.Log("Activating Type: " + type + " Exclude: " + exclude);
        List<Row> rowList = getRowsByType(type);
        foreach (Row row in rowList)
        {
            if (row.uniqueType != exclude)
            {
                Debug.Log("Activating Row: " + type + " Count: " + Game.activeDeck.getRowByType(row.uniqueType).Count);
                row.setActivateRowCardTargets(state, individualCards, unitsOnly);
            }
        }
    }

    public int countCardsInRows(RowEffected rowType)
    {
        int sum = 0;
        foreach (Row row in rows)
        {
            if (row.hasType(rowType))
            {
                sum += row.Count;
            }
        }
        return sum;
    }

    public int countUnitsInRows(RowEffected rowType)
    {
        int sum = 0;
        foreach (Row row in rows)
        {
            if (row.hasType(rowType))
            {
                foreach (Card c in row)
                {
                    if (CardModel.isUnit(c.cardType))
                    {
                        sum++;
                    }
                }
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
    public List<Card> getCardsInRowsByCardType(RowEffected rowType, CardType type)
    {
        List<Card> cardList = new List<Card>();
        foreach (Row row in rows)
        {
            if (row.hasType(rowType))
            {
                foreach (Card c in row)
                {
                    if (c.cardType == type)
                    {
                        cardList.Add(c);
                    }
                }
            }
        }
        return cardList;
    }
    public List<Card> getCardsInRowByType(RowEffected type)
    {
        List<Card> cardList = new List<Card>();
        foreach (Row row in rows)
        {
            if (row.hasType(type))
            {
                foreach (Card c in row)
                {
                    cardList.Add(c);
                }
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
        if (c.isClone)
        {
            c.Destroy();
        }
        else
        {
            Game.roundEndCards.Remove(c);
            c.setTargetActive(false);
            c.clearAttachments(this);
            //c.resetSelectionCounts();
            RowEffected graveyard = CardModel.isPower(c.cardType) ? RowEffected.PowerGraveyard : (CardModel.isUnit(c.cardType) ? RowEffected.UnitGraveyard : RowEffected.KingGraveyard);
            this.getRowByType(graveyard).Add(c);
            c.updateStrengthText(0);
        }
    }

    public void addCardToHand(Row currentRow, RowEffected playerHand, Card c)
    {
        currentRow.Remove(c);
        c.clearAttachments(this);
        c.setTargetActive(false);
        c.resetSelectionCounts();
        if (currentRow.uniqueType == RowEffected.UnitGraveyard && c.graveyardCardDrawRemain > 0)
        {
            drawCardGraveyard(c, null, playerHand);
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
            drawCardGraveyard(c, null, playerHand);
        }
        getRowByType(playerHand).Add(c);
    }

    public void sendCardToGraveyardMultiply(Row currentRow, RowEffected playerHand, Card c)
    {
        Game.activeCard.strengthMultiple++;
        sendCardToGraveyard(currentRow, playerHand, c);
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
                c.loadCardFront();
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

    public void drawCardGraveyard(Card c, Card targetCard, RowEffected playerHand)
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
                getRowByType(playerHand).Add(drawC);
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

    public void setCardAside(Row currentRow, RowEffected playerHand, Card c)
    {
        this.setCardAside(currentRow, c, Game.activeCard.setAsideType, CardModel.getPlayerFromRow(playerHand));
    }

    public void setCardAside(Row currentRow, Card c, SetAsideType setAsideType, RowEffected player)
    {
        Debug.Log("GREMLIN: card: " + c.cardName + " player: " + player + " row: " + currentRow.uniqueType + " setAsideType: " + c.setAsideType);
        RowEffected setAsideRow = CardModel.getRowFromSide(player, RowEffected.PlayerSetAside);
        RowEffected enemySetAside = CardModel.getRowFromSide(RowEffected.Enemy, setAsideRow);
        switch (setAsideType)
        {
            case SetAsideType.King: c.setAsideReturnRow = c.currentRow; break;
            case SetAsideType.EnemyKing: c.setAsideReturnRow = c.currentRow; setAsideRow = enemySetAside; break;
            case SetAsideType.EitherKing: c.setAsideReturnRow = c.currentRow; setAsideRow = CardModel.getRowFromSide(CardModel.getPlayerFromRow(c.currentRow), RowEffected.PlayerSetAside); break;
            case SetAsideType.Enemy: c.setAsideReturnRow = CardModel.getRowFromSide(player, RowEffected.EnemyHand); setAsideRow = enemySetAside; break;
            case SetAsideType.Player: c.setAsideReturnRow = CardModel.getRowFromSide(player, RowEffected.PlayerHand); break;
            case SetAsideType.AutoPlay: c.setAsideReturnRow = CardModel.getRowFromSide(player, c.autoPlaceRow); break;
        }
        Debug.Log(c.setAsideReturnRow);
        currentRow.Remove(c);
        getRowByType(setAsideRow).Add(c);

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
    public int totalScore(RowEffected rowType)
    {
        int total = 0;
        foreach (Row r in getRowsByType(rowType))
        {
            total += r.score;
        }
        return total;
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
        Row r = getRowByType(currentRow);
        foreach (Card c in discardList)
        {
            MoveLogger.logEnemyDiscard(c, RowEffected.Enemy);
            sendCardToGraveyard(r, RowEffected.None, c);
        }
    }

    internal void autoPlay(Row currentRow, RowEffected playerHand, Card c)
    {
        currentRow.Remove(c);
        getRowByType(CardModel.getRowFromSide(CardModel.getPlayerFromRow(playerHand), c.autoPlaceRow)).Add(c);
    }
}
