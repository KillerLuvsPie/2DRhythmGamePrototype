using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Environment = System.Environment;
using UnityEngine;
using UnityEditor;

public static class SaveLoadManager
{
    //SAVE VARUABLES
    public static List<GameObject> inputList;
    //SAVE CHART FUNCTION
    public static void SaveChart(string fileName)
    {
        string path = Application.dataPath + "/Resources/Charts";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path + "/" + fileName + ".chrt");
        bf.Serialize(file, inputList);
        file.Close();
    }
    //LOAD CHART FUNCTION
    public static void LoadChart(string fileName)
    {
        string path = Application.dataPath + "/Resources/Charts";
        if(File.Exists(path + "/" + fileName + "chrt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path + "/" + fileName + ".chrt");
            inputList = (List<GameObject>)bf.Deserialize(file);
            file.Close();
        }
    }
    //SAVE GAME FUNCTION
    public static void Save(string fileName)
    {
        //Check Folder Path
        string saveGamePath;
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            saveGamePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
            saveGamePath += "/My Games/Brick Break Prototype/";
        }
        else
        {
            saveGamePath = Application.persistentDataPath + "/";
        }
        //Check Folder Path END
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveGamePath + "/" + fileName + ".chrt");
        //bf.Serialize(file, highScoresBaseGame);
        file.Close();
    }

    //LOAD FUNCTION
    public static void Load()
    {
        //Check Folder Path
        string saveGamePath;
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            saveGamePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
            saveGamePath += "/My Games/Brick Break Prototype/";
        }
        else
        {
            saveGamePath = Application.persistentDataPath + "/";
        }
        //Check Folder Path END
        if (File.Exists(saveGamePath + "/GameSave.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveGamePath + "/GameSave.sav", FileMode.Open);
            //highScoresBaseGame = (List<int>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            //SETUP SAVE FILE IF IT DOES NOT EXIST
        }
    }
}
