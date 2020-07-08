using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerDataController
{

    private static PlayerDataController _instance;
    public static PlayerDataController Instance
    {
        get
        {
            return _instance;
        }
    }

    public PlayerData Data;

    private string _filePath = Application.persistentDataPath + "/data.dat";

    public static void Initialize()
    {
        if (_instance == null)
            _instance = new PlayerDataController();
        _instance.LoadData();
    }

    private PlayerDataController()
    {
        if (!File.Exists(_filePath))
        {
            Data = new PlayerData();
            Achievements.AchievementController.Instance.InitializePlayerData(Data);
        }
        else
            LoadData();
    }

    public void WriteData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        // получаем поток, куда будем записывать сериализованный объект
        using (FileStream fs = new FileStream(_filePath, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, Data);
        }
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(_filePath))
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Open))
            {
                Data = (PlayerData)formatter.Deserialize(fs);
            }
        }
        else
        {
            Data = new PlayerData();
            Achievements.AchievementController.Instance.InitializePlayerData(Data);
            WriteData();
        }
    }
}
