using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class CardModel : MonoBehaviour
{
    public static List<string> smallFronts;
    public static Texture2D[] bigFronts;
    //DO NOT MOVE COLUMN LIST BELOW COLUMNS
    //DO NOT MOVE COLUMN LIST BELOW COLUMNS
    public static List<IDataColumn> columns = new List<IDataColumn>();
    //DO NOT MOVE COLUMN LIST BELOW COLUMNS
    //DO NOT MOVE COLUMN LIST BELOW COLUMNS
    public static DataColumn<string> names = new DataColumn<string>("CardName", false);
    public static DataColumn<string> effectText = new DataColumn<string>("EffectText", true);
    public static DataColumn<CardType> cardTypes = new DataColumn<CardType>("CardType", false);
    public static DataColumn<int> strength = new DataColumn<int>("Strength", false);
    public static DataColumn<int> playerCardDraw = new DataColumn<int>("PlayerCardDraw");
    public static DataColumn<CardDrawType> cardDrawType = new DataColumn<CardDrawType>("CardDrawType");
    public static DataColumn<int> playerCardDestroy = new DataColumn<int>("PlayerCardDestroy");
    public static DataColumn<DestroyType> destroyType = new DataColumn<DestroyType>("DestroyType");
    public static DataColumn<int> playerCardReturn = new DataColumn<int>("PlayerCardReturn");
    public static DataColumn<CardReturnType> cardReturnType = new DataColumn<CardReturnType>("CardReturnType");
    public static DataColumn<float> strengthModifier = new DataColumn<float>("StrengthModifier");
    public static DataColumn<bool> strengthModRow = new DataColumn<bool>("StrengthModRow");
    public static DataColumn<StrengthModType> strengthModType = new DataColumn<StrengthModType>("StrengthModType");
    public static DataColumn<int> graveyardCardDraw = new DataColumn<Int32>("GraveyardCardDraw");
    public static DataColumn<int> enemyCardDraw = new DataColumn<int>("EnemyCardDraw");
    public static DataColumn<int> enemyCardDestroy = new DataColumn<int>("EnemyCardDestroy");
    public static DataColumn<int> enemyReveal = new DataColumn<int>("EnemyReveal");
    public static DataColumn<float> rowMultiple = new DataColumn<float>("RowMultiple");
    public static DataColumn<RowEffected> rowEffected = new DataColumn<RowEffected>("EffectedRow");
    public static DataColumn<bool> clearWeather = new DataColumn<bool>("ClearWeather");
    public static DataColumn<int> setAside = new DataColumn<int>("SetAside");
    public static DataColumn<SetAsideType> setAsideType = new DataColumn<SetAsideType>("SetAsideType");
    public static DataColumn<bool> attach = new DataColumn<bool>("Attach");
    public static DataColumn<int> strengthCondition = new DataColumn<int>("StengthCondition");
    public static DataColumn<int> chooseN = new DataColumn<int>("ChooseN");
    public static DataColumn<ChooseNAction> chooseNAction = new DataColumn<ChooseNAction>("ChooseNAction");
    public static DataColumn<RowEffected> chooseRow = new DataColumn<RowEffected>("ChooseRow");
    public static DataColumn<int> chooseShowN = new DataColumn<int>("ChooseShowN");
    public static DataColumn<ChooseCardType> chooseCardType = new DataColumn<ChooseCardType>("ChooseCardType");
    public static DataColumn<bool> chooseSkippable = new DataColumn<bool>("ChooseSkippable");
    public static DataColumn<bool> playInRow = new DataColumn<bool>("PlayInRow");
    public static DataColumn<bool> playNextRound = new DataColumn<bool>("PlayNextRound");
    public static DataColumn<bool> isAltEffect = new DataColumn<bool>("IsAltEffect");
    public static DataColumn<bool> canAutoPlayAltEffect = new DataColumn<bool>("CanAutoPlayAltEffect");
    public static DataColumn<string> mainCardName = new DataColumn<string>("MainCardName");
    public static DataColumn<string> effectDescription = new DataColumn<string>("EffectDescription");
    public static DataColumn<bool> protect = new DataColumn<bool>("Protect");
    public static DataColumn<RowEffected> autoPlaceRow = new DataColumn<RowEffected>("AutoPlaceRow");
    public int numCardEffects;

    public int[] isSpecial;
    public string spriteFolder = "Images";

    public void readTextFile()
    {
        if(columns == null){

        }
        TextAsset bindata = Resources.Load("CardSheet") as TextAsset;
        string[] lines = bindata.text.Split("\n".ToCharArray());
        List<string> stringList = new List<string>();
        smallFronts = new List<string>();
        foreach (string line in lines)
        {
            stringList.Add(line);
        }
        string[] colNames = stringList[0].Split("	".ToCharArray());
        
        Debug.Log("------------------------------------------------- ");
        for (int i = 0; i < colNames.Length; i++)
        {
            string name = colNames[i];
            IDataColumn info = columns.Find((x) => x.columnName == System.Text.RegularExpressions.Regex.Replace(name, @"\s+", ""));
            if(info != null){
                info.columnIdx = i;
            }
            else{
                Debug.LogError("MISSING COLUMN NAME: " + name);
            }
        }
        numCardEffects = stringList.Count - 1;
        // start from second row
        for (int i = 1; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split("	".ToCharArray());
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim();  //removed the blank spaces
            }
            smallFronts.Add(temp[0]);
            foreach (IDataColumn dataColumn in columns)
            {
                dataColumn.AddOptional(temp, i);
            }
            /*
            AddOptional(temp, strength, strength.info.columnIdx);
            AddTypePair(temp, playerCardDraw, 4, cardDrawType, 5);
            AddTypePair(temp, playerCardDestroy, 6, destroyType, 7);
            AddTypePair(temp, playerCardReturn, 8, cardReturnType, 9);
            AddTypePairFloat(temp, strengthModifier, 10, strengthModType, 11);
            AddOptional(temp, graveyardCardDraw, 12);
            AddOptional(temp, enemyCardDraw, 13);
            AddOptional(temp, enemyCardDestroy, 14);
            AddOptional(temp, enemyReveal, 15);
            AddOptionalFloat(temp, rowMultiple, 16);
            AddOptionalType(temp, rowEffected, 17);
            AddTypePair(temp, setAside, 18, setAsideType, 19);
            AddOptionalBool(temp, attach, 20);
            AddOptional(temp, strengthCondition, 21);
            AddTypePair(temp, chooseN, 22, chooseRow, 23);
            AddOptional(temp, chooseShowN, 24);
            AddOptionalType(temp, chooseCardType, 25);
            AddOptionalBool(temp, playInRow, 26);
            */
        }
    }



    public Texture2D getCardBack(int index)
    {
        CardType type = cardTypes[index];
        string postfix = "";
        if (isPower(type))
        {
            postfix = "Power";
        }
        else if (type == CardType.King)
        {
            postfix = "King";
        }
        Texture2D s = (Texture2D)Resources.Load("Images/CardBacks/Flag" + postfix, typeof(Texture2D));
        return s;
    }

    public Texture2D getSmallFront(int index)
    {
        Texture2D s = (Texture2D)Resources.Load("Images/" + smallFronts[index], typeof(Texture2D));
        return s;
    }

    public Texture2D getBigFront(int index)
    {
        return bigFronts[index];
    }

    public string getName(int index)
    {
        return names[index];
    }

    public float getRowMultiple(int index)
    {
        return rowMultiple[index];
    }

    public RowEffected getRowEffected(int index)
    {
        return rowEffected[index];
    }

    public CardType getCardType(int index)
    {
        return cardTypes[index];
    }

    public int getIsSpecial(int index)
    {
        return 0;
    }
    public static bool isUnit(CardType type)
    {
        return type == CardType.Melee || type == CardType.Ranged ||
                type == CardType.Switch || type == CardType.Siege;
    }
    public static bool isUnitOrSpy(CardType type)
    {
        return type == CardType.Melee || type == CardType.Ranged ||
                type == CardType.Switch || type == CardType.Siege || type == CardType.Spy;
    }
    public static bool isPower(CardType type)
    {
        return type == CardType.Power || type == CardType.Spy ||
            type == CardType.Weather || type == CardType.Decoy;
    }
    public static RowEffected getHandRow(RowEffected player)
    {
        if (player == RowEffected.Enemy)
        {
            return RowEffected.EnemyHand;
        }
        return RowEffected.PlayerHand;
    }
    public static RowEffected getEnemy(RowEffected player)
    {
        if (player == RowEffected.Enemy)
        {
            return RowEffected.Player;
        }
        return RowEffected.Enemy;
    }

    public static RowEffected getPlayableRow(RowEffected player, CardType type)
    {
        switch (type)
        {
            case CardType.Melee: return getRowFromSide(player, RowEffected.PlayerMelee);
            case CardType.Ranged: return getRowFromSide(player, RowEffected.PlayerRanged);
            case CardType.Siege: return getRowFromSide(player, RowEffected.PlayerSiege);
            case CardType.Switch: return getRowFromSide(player, RowEffected.PlayerMelee);
        }
        return RowEffected.None;
    }
    public static bool rowIsUnique(RowEffected type)
    {
        switch (type)
        {
            case RowEffected.PlayerMelee: return true;
            case RowEffected.PlayerRanged: return true;
            case RowEffected.PlayerSiege: return true;
            case RowEffected.PlayerMeleeKing: return true;
            case RowEffected.PlayerRangedKing: return true;
            case RowEffected.PlayerSiegeKing: return true;
            case RowEffected.EnemyMelee: return true;
            case RowEffected.EnemyRanged: return true;
            case RowEffected.EnemySiege: return true;
            case RowEffected.EnemyMeleeKing: return true;
            case RowEffected.EnemyRangedKing: return true;
            case RowEffected.EnemySiegeKing: return true;
        }
        return false;
    }

    public static bool isDisplayRow(RowEffected type)
    {
        switch (type)
        {
            case RowEffected.PlayerChooseN: return true;
            case RowEffected.EnemyChooseN: return true;
            case RowEffected.ChooseN: return true;
        }
        return false;
    }

    public static RowEffected getPlayableRow(RowEffected player, RowEffected type)
    {
        switch (type)
        {
            case RowEffected.Melee: return getRowFromSide(player, RowEffected.PlayerMelee);
            case RowEffected.Ranged: return getRowFromSide(player, RowEffected.PlayerRanged);
            case RowEffected.Siege: return getRowFromSide(player, RowEffected.PlayerSiege);
        }
        return RowEffected.None;
    }
    public static RowEffected getFullRow(RowEffected generalRow)
    {
        switch (generalRow)
        {
            case RowEffected.PlayerMelee: return RowEffected.MeleeFull;
            case RowEffected.PlayerRanged: return RowEffected.RangedFull;
            case RowEffected.PlayerSiege: return RowEffected.SiegeFull;
            case RowEffected.EnemyMelee: return RowEffected.MeleeFull;
            case RowEffected.EnemyRanged: return RowEffected.RangedFull;
            case RowEffected.EnemySiege: return RowEffected.SiegeFull;
        }
        return RowEffected.None;
    }
    public static RowEffected getRowNoPlayer(RowEffected generalRow)
    {
        switch (generalRow)
        {
            case RowEffected.PlayerMelee: return RowEffected.Melee;
            case RowEffected.PlayerRanged: return RowEffected.Ranged;
            case RowEffected.PlayerSiege: return RowEffected.Siege;
            case RowEffected.EnemyMelee: return RowEffected.Melee;
            case RowEffected.EnemyRanged: return RowEffected.Ranged;
            case RowEffected.EnemySiege: return RowEffected.Siege;
        }
        return RowEffected.None;
    }

    public static RowEffected getPlayerFromRow(RowEffected generalRow)
    {
        switch (generalRow)
        {
            case RowEffected.PlayerPlayable: return RowEffected.Player;
            case RowEffected.PlayerMelee: return RowEffected.Player;
            case RowEffected.PlayerRanged: return RowEffected.Player;
            case RowEffected.PlayerSiege: return RowEffected.Player;
            case RowEffected.PlayerMeleeKing: return RowEffected.Player;
            case RowEffected.PlayerRangedKing: return RowEffected.Player;
            case RowEffected.PlayerMeleeOrRanged: return RowEffected.Player;
            case RowEffected.PlayerRangedOrSiege: return RowEffected.Player;
            case RowEffected.PlayerKing: return RowEffected.Player;
            case RowEffected.PlayerHand: return RowEffected.Player;
            case RowEffected.PlayerSiegeKing: return RowEffected.Player;
            case RowEffected.PlayerMax: return RowEffected.Player;
            case RowEffected.EnemyMeleeOrRanged: return RowEffected.Enemy;
            case RowEffected.EnemyMeleeOrSiege: return RowEffected.Enemy;
            case RowEffected.EnemyRangedOrSiege: return RowEffected.Enemy;
            case RowEffected.EnemyPlayable: return RowEffected.Enemy;
            case RowEffected.EnemyKing: return RowEffected.Enemy;
            case RowEffected.EnemyMax: return RowEffected.Enemy;
            case RowEffected.EnemyHand: return RowEffected.Enemy;
            case RowEffected.EnemyMelee: return RowEffected.Enemy;
            case RowEffected.EnemyRanged: return RowEffected.Enemy;
            case RowEffected.EnemySiege: return RowEffected.Enemy;
            case RowEffected.EnemyMeleeKing: return RowEffected.Enemy;
            case RowEffected.EnemyRangedKing: return RowEffected.Enemy;
            case RowEffected.EnemySiegeKing: return RowEffected.Enemy;
        }

        return RowEffected.None;
    }
    public static List<CardType> chooseToCardType(ChooseCardType type)
    {
        switch (type)
        {
            case ChooseCardType.Unit: return new List<CardType>() { CardType.Melee, CardType.Ranged, CardType.Siege, CardType.Switch };
            case ChooseCardType.Power: return new List<CardType>() { CardType.Power, CardType.Decoy, CardType.Spy, CardType.Weather };
            case ChooseCardType.King: return new List<CardType>() { CardType.King };
        }
        return null;
    }
    public static List<CardType> chooseToCardTypeExclude(ChooseCardType type)
    {
        switch (type)
        {
            case ChooseCardType.Unit: return new List<CardType>() { CardType.Power, CardType.Decoy, CardType.Spy, CardType.Weather, CardType.King };
            case ChooseCardType.Power: return new List<CardType>() { CardType.Melee, CardType.Ranged, CardType.Siege, CardType.Switch, CardType.King };
            case ChooseCardType.UnitOrPower: return new List<CardType>() { CardType.King };
            case ChooseCardType.King: return new List<CardType>() { CardType.Melee, CardType.Ranged, CardType.Siege, CardType.Switch, CardType.Power, CardType.Decoy, CardType.Spy, CardType.Weather };
        }
        return null;
    }
    public static RowEffected getRowFromSide(RowEffected player, RowEffected generalRow)
    {
        if (player == RowEffected.Enemy)
        {
            switch (generalRow)
            {
                case RowEffected.PlayerPlayable: return RowEffected.EnemyPlayable;
                case RowEffected.PlayerMelee: return RowEffected.EnemyMelee;
                case RowEffected.PlayerRanged: return RowEffected.EnemyRanged;
                case RowEffected.PlayerSiege: return RowEffected.EnemySiege;
                case RowEffected.EnemyPlayable: return RowEffected.PlayerPlayable;
                case RowEffected.PlayerHand: return RowEffected.EnemyHand;
                case RowEffected.EnemyKing: return RowEffected.PlayerKing;
                case RowEffected.PlayerKing: return RowEffected.EnemyKing;
                case RowEffected.EnemyMax: return RowEffected.PlayerMax;
                case RowEffected.PlayerMax: return RowEffected.EnemyMax;
                case RowEffected.EnemyHand: return RowEffected.PlayerHand;
                case RowEffected.EnemyMelee: return RowEffected.PlayerMelee;
                case RowEffected.EnemyRanged: return RowEffected.PlayerRanged;
                case RowEffected.EnemySiege: return RowEffected.PlayerSiege;
                case RowEffected.EnemySetAside: return RowEffected.PlayerSetAside;
                case RowEffected.PlayerSetAside: return RowEffected.EnemySetAside;
                case RowEffected.EnemyMeleeKing: return RowEffected.PlayerMeleeKing;
                case RowEffected.EnemyRangedKing: return RowEffected.PlayerRangedKing;
                case RowEffected.EnemySiegeKing: return RowEffected.PlayerSiegeKing;
                case RowEffected.PlayerMeleeKing: return RowEffected.EnemyMeleeKing;
                case RowEffected.PlayerRangedKing: return RowEffected.EnemyRangedKing;
                case RowEffected.PlayerSiegeKing: return RowEffected.EnemySiegeKing;
                case RowEffected.PlayerMeleeOrRanged: return RowEffected.EnemyMeleeOrRanged;
                case RowEffected.PlayerMeleeOrSiege: return RowEffected.EnemyMeleeOrSiege;
                case RowEffected.PlayerRangedOrSiege: return RowEffected.EnemyRangedOrSiege;
                case RowEffected.EnemyMeleeOrRanged: return RowEffected.PlayerMeleeOrRanged;
                case RowEffected.EnemyMeleeOrSiege: return RowEffected.PlayerMeleeOrSiege;
                case RowEffected.EnemyRangedOrSiege: return RowEffected.PlayerRangedOrSiege;
                case RowEffected.PlayerChooseN: return RowEffected.EnemyChooseN;
                case RowEffected.EnemyChooseN: return RowEffected.PlayerChooseN;
            }
        }

        return generalRow;
    }

    internal static string getRowName(RowEffected chooseRow)
    {
        switch (chooseRow)
        {
            case RowEffected.PlayerMelee: return "your Melee row";
            case RowEffected.PlayerRanged: return "your Ranged row";
            case RowEffected.PlayerSiege: return "your Siege row";
            case RowEffected.PlayerHand: return "your hand";
            case RowEffected.EnemyHand: return "your opponent's hand";
            case RowEffected.EnemyMelee: return "your opponent's Melee row";
            case RowEffected.EnemyRanged: return "your opponent's Ranged row";
            case RowEffected.EnemySiege: return "your opponent's Siege row";
            case RowEffected.UnitGraveyard: return "the Unit Graveyard";
            case RowEffected.PowerGraveyard: return "the Power Graveyard";
            case RowEffected.UnitDeck: return "the Unit Deck";
            case RowEffected.PowerDeck: return "the Power Deck";
        }
        return chooseRow.ToString();
    }
}
