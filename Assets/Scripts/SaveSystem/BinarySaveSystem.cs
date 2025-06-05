using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BinarySaveSystem
{
    private static string savePath => Application.persistentDataPath + "/save.dat";

    public static void Save(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static GameData Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                return formatter.Deserialize(stream) as GameData;
            }
        }
        else
        {
            GameData data = new GameData();
            Save(data);
            return data;
        }
    }
}
