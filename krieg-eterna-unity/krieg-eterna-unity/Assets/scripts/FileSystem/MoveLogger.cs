using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class MoveLogger
{
    static string fileName;
    static FileStream moveLogFile;
    static StreamWriter sw;
    public static void newLogFile()
    {
        if (!Game.testing)
        {
            string timeStamp = DateTime.Now.ToString("mmddyyyyHHMMss");
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = dir + "\\GameReplayLogs";
            Debug.Log("no dir found making");
            DirectoryInfo di = Directory.CreateDirectory(path);

            fileName = path + "\\" + timeStamp + ".log";
            moveLogFile = new FileStream(fileName, FileMode.Append);

            sw = new StreamWriter(moveLogFile);
        }
    }
    internal static void logSeed(int seed)
    {
        if (!Game.testing)
        {
            sw.WriteLine(seed);
            flushLogs();
        }
    }
    internal static void logMove(Card c, Row targetRow, Card targetCard, RowEffected player)
    {
        if (!Game.testing)
        {
            string targetRowStr = "-";
            string targetCardStr = "-";
            if (targetRow != null)
            {
                targetRowStr = targetRow.uniqueType.ToString();
            }
            if (targetCard != null)
            {
                targetCardStr = targetCard.cardName;
            }
            sw.WriteLine("Move: " + c.cardName + " " + targetRowStr + " " + targetCardStr + " " + player);
            flushLogs();
        }
    }
    internal static void logButtonPress(RowEffected button, RowEffected player)
    {
        if (!Game.testing)
        {
            sw.WriteLine("Button: " + button + " " + player);
            flushLogs();
        }
    }
    internal static void logChooseCard(Card cardClone, RowEffected player)
    {
        if (!Game.testing)
        {
            sw.WriteLine("Choose: " + CardModel.getRowFromSide(player, RowEffected.PlayerChooseN) + " " + cardClone.cardName + " " + player);
            flushLogs();
        }
    }

    internal static void logEnemyDiscard(Card card, RowEffected player)
    {
        if (!Game.testing)
        {
            sw.WriteLine("EnemyDiscard: " + card.cardName + " " + player);
            flushLogs();
        }

    }

    internal static void flushLogs()
    {
        if (!Game.testing)
        {
            sw.Flush();
            moveLogFile.Flush();
        }
    }
    internal static void closeLogs()
    {
        if (!Game.testing)
        {
            sw.Dispose();
            moveLogFile.Dispose();
        }
    }

    internal static void logRowAdd(Card card, RowEffected rowType, RowEffected player)
    {
        if (!Game.testing)
        {
            sw.WriteLine("RowAdd: " + card.cardName + " " + rowType + " " + player);
            flushLogs();
        }
    }

    internal static void logRowRemove(Card card, RowEffected rowType, RowEffected player)
    {
        if (!Game.testing)
        {
            sw.WriteLine("RowRemove: " + card.cardName + " " + rowType + " " + player);
            flushLogs();
        }
    }
    internal static void logTurnOver(RowEffected player)
    {
        if (!Game.testing)
        {
            sw.WriteLine("---------------------------TurnOver: " + player + "------------------------");
            flushLogs();
        }
    }
}