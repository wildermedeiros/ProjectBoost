using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveAndLoadSystem // cannot be instanciated 
{
    public static void SaveData (Player player)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "player.game");
        //string path = Application.persistentDataPath + "/player.game";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player);

        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();

        Debug.Log("Arquivo salvo");
    }

    public static PlayerData LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.game"); 
        //string path = Application.persistentDataPath + "/player.game";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            PlayerData playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();

            return playerData;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
