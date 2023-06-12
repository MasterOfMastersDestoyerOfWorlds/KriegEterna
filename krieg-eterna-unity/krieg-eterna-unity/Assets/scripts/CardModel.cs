using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardModel : MonoBehaviour
{
    public List<string> smallFronts;
    public Texture2D[] bigFronts;
    public List<string> names;
    public List<float> rowMultiple;
    public List<RowEffected> rowEffected;
    public List<CardType> cardTypes;
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
			Debug.Log("Assets/Images/"+temp[0]);
			smallFronts.Add(temp[0]);
            
            cardTypes.Add((CardType) System.Enum.Parse(typeof(CardType), temp[1], true));
            if(!System.String.IsNullOrEmpty(temp[12])){
                rowMultiple.Add(float.Parse(temp[12]));
                rowEffected.Add((RowEffected) System.Enum.Parse(typeof(RowEffected), temp[13], true));
            }else{
                rowMultiple.Add(0);
                rowEffected.Add(RowEffected.None);
            }
            
			names.Add(temp[1]);

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
}
