using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveData(string jsonString)
    {
        PlayerPrefs.SetString("Game", jsonString);
    }


    public string LoadData()
    {
        return PlayerPrefs.GetString("Game", ""); 
    }


}

