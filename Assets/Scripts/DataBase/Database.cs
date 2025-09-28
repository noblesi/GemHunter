using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class Database
{
    public static readonly string DBFileName = "Database.dat";
    public static readonly int maxChapter = 3;

    public static DatabaseItem DBItem { get; private set; } = new DatabaseItem();
    public static bool IsReaded { get; private set; } = false;

    public static void Write()
    {
        Logger.Log("Database::Write - Save data in Database");

        string path = Path.Combine(Application.persistentDataPath, DBFileName);

        string json = JsonConvert.SerializeObject(DBItem, Formatting.Indented);

        File.WriteAllText(path, json);
    }

    public static void Read()
    {
        Logger.Log("Database::Read - Load data in Database");

        IsReaded = true;

        string path = Path.Combine(Application.persistentDataPath, DBFileName);

        if(File.Exists(path))
        {
            Logger.Log("Database::Read - File Exist in Folder");

            string json = File.ReadAllText(path);

            DBItem = JsonConvert.DeserializeObject<DatabaseItem>(json);
        }
        else
        {
            Logger.Log("Database::Read - File Not Exist in Folder");
            Reset();
        }

        IsReaded = false;
    }

    public static void Reset()
    {
        if (DBItem == null) DBItem = new DatabaseItem();

        DBItem.Reset();

        Write();
    }
}

[System.Serializable]
public class DatabaseItem
{
    public DBItem_Player player;
    public DBItem_Goods goods;
    public DBItem_Chapter[] chapters;

    public DatabaseItem()
    {
        player = new DBItem_Player();
        goods = new DBItem_Goods();
        chapters = new DBItem_Chapter[Database.maxChapter];
        for(int i = 0; i < chapters.Length; ++i)
        {
            chapters[i] = new DBItem_Chapter();
        }
    }

    public void Reset()
    {
        player.Reset();
        goods.Reset();

        for(int i = 0; i < chapters.Length; ++i)
        {
            chapters[i].Reset();
        }

        chapters[0].isUnlock = true;
    }
}

[System.Serializable]
public class DBItem_Player
{
    public int level;
    public float experience;

    public void Reset()
    {
        level = 1;
        experience = 0f;
    }
}

[System.Serializable]
public class DBItem_Goods
{
    public int heart;
    public float heartTimer;
    public string heartLastTime;
    public float gem;

    public readonly int maxHeart = 50;
    public readonly float heartRefillTime = 20 * 60;

    public void Reset()
    {
        heart = maxHeart;
        heartTimer = 0f;
        heartLastTime =  string.Empty;
        gem = 0;
    }
}

[System.Serializable]
public class DBItem_Chapter
{
    public bool isUnlock;
    public int bestStage;

    public void Reset()
    {
        isUnlock = false;
        bestStage = 1;
    }
}
