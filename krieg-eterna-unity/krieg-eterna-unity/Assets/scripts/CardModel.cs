using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardModel : MonoBehaviour
{
    public List<string> smallFronts;
    public Texture2D[] bigFronts;
    public List<string> names;
    public List<CardType> cardTypes;
    public List<int> strength;
    public List<int> playerCardDraw;
    public List<CardDrawType> cardDrawType;
    public List<int> playerCardDestroy;
    public List<DestroyType> destroyType;
    public List<int> playerCardReturn;
    public List<CardReturnType> cardReturnType;
    public List<float> strengthModifier;
    public List<StrengthModType> strengthModType;
    public List<int> graveyardCardDraw;
    public List<int> enemyCardDraw;
    public List<int> enemyCardDestroy;
    public List<int> enemyReveal;
    public List<float> rowMultiple;
    public List<RowEffected> rowEffected;
    public List<int> setAside;
    public List<SetAsideType> setAsideType;
    public List<bool> attach;
    public List<int> strengthCondition;
    public List<int> chooseN;
    public List<RowEffected> chooseRow;
    public List<int> chooseShowN;
    public List<ChooseCardType> chooseCardType;
    public List<bool> playInRow;

    public int[] isSpecial;
    public string spriteFolder = "Images";
    /* 
     * > isSpecial table of values:
     * [1] - gold card
     * [2] - spy card
     * [3] - manekin card
     * [4] - destroy card
     * [5] - weather card
     * [6] - gold spy
     *
     * > Power table of values:
     * [0+] - normal
     * [-1] - sword weather
     * [-2] - bow weather
     * [-3] - trebuchet weather
     * [-4] - clean weather
     */


    public void readTextFile()
    {
        StreamReader inp_stm = new StreamReader("Assets/Resources/CardSheet.tsv");
        List<string> stringList = new List<string>();
        smallFronts = new List<string>();
        names = new List<string>();
        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();

        // start from second row
        for (int i = 1; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split("	".ToCharArray());
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim();  //removed the blank spaces
            }
            smallFronts.Add(temp[0]);
            names.Add(temp[0]);

            cardTypes.Add((CardType)System.Enum.Parse(typeof(CardType), temp[1], true));

            AddOptional(temp, strength, 3);
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
        }
    }

    private void AddOptional(string[] temp, List<int> numList, int indexNum)
    {
        if (!System.String.IsNullOrEmpty(temp[indexNum]))
        {
            numList.Add(int.Parse(temp[indexNum]));
        }
        else
        {
            numList.Add(0);
        }
    }
    private void AddOptionalBool(string[] temp, List<bool> boolList, int indexBool)
    {
        if (!System.String.IsNullOrEmpty(temp[indexBool]))
        {
            boolList.Add(bool.Parse(temp[indexBool]));
        }
        else
        {
            boolList.Add(false);
        }
    }

    private void AddOptionalFloat(string[] temp, List<float> typeList, int indexType)
    {
        if (!System.String.IsNullOrEmpty(temp[indexType]))
        {
            typeList.Add(float.Parse(temp[indexType]));
        }
        else
        {
            typeList.Add(0f);
        }
    }

    private void AddOptionalType<T>(string[] temp, List<T> typeList, int indexType)
    {
        if (!System.String.IsNullOrEmpty(temp[indexType]))
        {
            typeList.Add((T)System.Enum.Parse(typeof(T), temp[indexType], true));
        }
        else
        {
            typeList.Add(default(T));
        }
    }

    private void AddTypePair<T>(string[] temp, List<int> numList, int indexNum, List<T> typeList, int indexType)
    {
        if (!System.String.IsNullOrEmpty(temp[indexNum]))
        {
            numList.Add(int.Parse(temp[indexNum]));
            typeList.Add((T)System.Enum.Parse(typeof(T), temp[indexType], true));
        }
        else
        {
            numList.Add(0);
            typeList.Add(default(T));
        }
    }
    private void AddTypePairFloat<T>(string[] temp, List<float> numList, int indexNum, List<T> typeList, int indexType)
    {
        if (!System.String.IsNullOrEmpty(temp[indexNum]))
        {
            numList.Add(float.Parse(temp[indexNum]));
            typeList.Add((T)System.Enum.Parse(typeof(T), temp[indexType], true));
        }
        else
        {
            numList.Add(0);
            typeList.Add(default(T));
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
            case RowEffected.PlayerSwitchFront: return RowEffected.Player;
            case RowEffected.PlayerSwitchBack: return RowEffected.Player;
            case RowEffected.EnemySwitchFront: return RowEffected.Enemy;
            case RowEffected.EnemySwitchBack: return RowEffected.Enemy;
            case RowEffected.PlayerSiegeKing: return RowEffected.Player;
            case RowEffected.EnemyPlayable: return RowEffected.Enemy;
            case RowEffected.PlayerHand: return RowEffected.Player;
            case RowEffected.EnemyKing: return RowEffected.Enemy;
            case RowEffected.PlayerKing: return RowEffected.Player;
            case RowEffected.EnemyMax: return RowEffected.Enemy;
            case RowEffected.PlayerMax: return RowEffected.Player;
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
                case RowEffected.PlayerSwitchFront: return RowEffected.EnemySwitchFront;
                case RowEffected.PlayerSwitchBack: return RowEffected.EnemySwitchBack;
                case RowEffected.EnemySwitchFront: return RowEffected.PlayerSwitchFront;
                case RowEffected.EnemySwitchBack: return RowEffected.PlayerSwitchBack;
            }
        }

        return generalRow;
    }
}
