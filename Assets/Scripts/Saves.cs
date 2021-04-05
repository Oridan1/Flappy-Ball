using UnityEngine;
using System.Collections;
using System;// libreria para guardado  de datos 
using System.Runtime.Serialization.Formatters.Binary; //  lib para guardado de datos
using System.IO;


public class Saves : MonoBehaviour
{
    public static Saves control;

    // Use this for initialization
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else
        {
            if (control != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Save(int level, Color color)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData(); // crea un nuevo player data 
        data.level = level;// al crear uno  nuevo  lo  sobreescribe con los datos deseados    
        data.color = ColorParse(color);
        bf.Serialize(file, data);
        file.Close();
    }

    public PlayerData Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        return new PlayerData();
    }

    ColorSerial ColorParse(Color color)
    {
        ColorSerial colorSerial = new ColorSerial();
        colorSerial.r = color.r;
        colorSerial.g = color.g;
        colorSerial.b = color.b;
        colorSerial.a = color.a;
        return colorSerial;
    }
}

[Serializable]
public class PlayerData
{
    public int level = 1;
    public ColorSerial color = new ColorSerial();
}

[Serializable]
public class ColorSerial
{
    public float r = 1;
    public float g;
    public float b;
    public float a = 1;    
}