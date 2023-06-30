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

    public Texture2D getSmallFront(int index)
    {
		Texture2D s = (Texture2D)Resources.Load("Images/"+smallFronts[index], typeof(Texture2D));
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
    public static bool isUnit(CardType type){
        return type == CardType.Melee || type == CardType.Ranged || 
                type == CardType.Switch || type == CardType.Siege;
    }
    public static bool isPower(CardType type){
        return type == CardType.Power || type == CardType.Spy || 
            type == CardType.Weather || type == CardType.Decoy;
    }
    public static RowEffected getHandRow(RowEffected player){
        if(player == RowEffected.Enemy){
            return RowEffected.EnemyHand;
        }
        return RowEffected.PlayerHand;
    }
    public static RowEffected getEnemy(RowEffected player){
        if(player == RowEffected.Enemy){
            return RowEffected.Player;
        }
        return RowEffected.Enemy;
    }
    public static RowEffected getPlayerRow(RowEffected player, RowEffected generalRow){
        if(player == RowEffected.Enemy){
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
            }
        }else{
            switch (generalRow)
            {   
                case RowEffected.PlayerMelee: return RowEffected.PlayerMelee;
                case RowEffected.PlayerRanged: return RowEffected.PlayerRanged;
                case RowEffected.PlayerSiege: return RowEffected.PlayerSiege;
                case RowEffected.PlayerPlayable: return RowEffected.PlayerPlayable;
                case RowEffected.EnemyPlayable: return RowEffected.EnemyPlayable;
                case RowEffected.PlayerHand: return RowEffected.PlayerHand;
                case RowEffected.EnemyHand: return RowEffected.EnemyHand;
                case RowEffected.EnemyKing: return RowEffected.EnemyKing;
                case RowEffected.EnemyMax: return RowEffected.EnemyMax;
                case RowEffected.PlayerMax: return RowEffected.PlayerMax;
                case RowEffected.PlayerKing: return RowEffected.PlayerKing;
                case RowEffected.EnemyMelee: return RowEffected.EnemyMelee;
                case RowEffected.EnemyRanged: return RowEffected.EnemyRanged;
                case RowEffected.EnemySiege: return RowEffected.EnemySiege;
            }
        }
        return generalRow;
    }
}
