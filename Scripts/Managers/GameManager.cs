using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public RespawnManager Respawner { get; private set; }
    public SaveManager Saver { get; private set; }
    public Transform NPC { get; private set; }
    public Progress progress = new Progress();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        

    }
    void Start()
    {
        Respawner = GetComponentInChildren<RespawnManager>();
        Saver = GetComponentInChildren<SaveManager>();
        NPC = GameObject.Find("NPC").transform;
        LoadData();
    }

    void LoadData()
    {
        string str = Saver.LoadData();
        Debug.Log(str);
        if (str == "")
            return;

        progress = JsonUtility.FromJson<Progress>(str);
        SetData();
    }

    void SetData()
    {
        if(progress.respawnTimeDate != null)
        for (int i = 0; i < progress.respawnTimeDate.Length; i++)
        {
            Respawner.respawnTime.Add(DateTime.FromBinary(Convert.ToInt64(progress.respawnTimeDate[i])), progress.respawnTimeObject[i]);
            Debug.Log(progress.respawnTimeDate[i]);
        }
        

        player.transform.position = progress.playerPos;
    }

    public void SaveData()
    {
        CompleteProgress();
        Saver.SaveData(JsonUtility.ToJson(progress));
        foreach (var resp in progress.respawnTimeDate)
        {
            Debug.Log(resp);
        }
    }

    void CompleteProgress()
    {
        //progress.respawnTime = Respawner.respawnTime;
        progress.respawnTimeDate = new string[Respawner.respawnTime.Count];
        progress.respawnTimeObject = new int[Respawner.respawnTime.Count];
        int index = 0;
        foreach(var prog in Respawner.respawnTime)
        {
            progress.respawnTimeDate[index] = (prog.Key.ToBinary().ToString());
            progress.respawnTimeObject[index] = prog.Value;
            index++;
        }
        progress.playerPos = player.transform.position;

    }
}
[Serializable]
public class Progress
{
    //Respawn
    //public Dictionary<DateTime, GameObject> respawnTime = new();
    public string[] respawnTimeDate;
    public int[] respawnTimeObject;
    //Player position
    public Vector3 playerPos;
    //Game settings and Game Data
    #region MAIN PLAYER CHARACTERISTICS
    public float mainSpeed = 1;
    public float mainHP = 10;
    public float mainDamage = 1;
    public float mainRadius = 1;
    public int mainCapacity = 10;
    public int mainTentaclesQuantity = 1;
    public int mainTentaclesGroup = 1;
    public int EXP = 0;
    public int lvl = 1;

    #endregion

    #region PLAYER RESOURCES 
    public int meat = 0;
    public int gem = 0;
    public int crystall = 0;
    public int DNA = 0;
    public int redReagent = 0;
    public int yellowReagent = 0;
    public int blackReagent = 0;
    public int greenReagent = 0;
    public int blueReagent = 0;


    #endregion 

    #region ALIEN UPDATE CHARACTERISTICS
    public float alienSpeedBonus = 1;
    public float alienHP = 0;
    public float alienDamage = 0;
    public float alienRadius = 0;
    public float alienCostBonus = 1;
    public float alienArmor = 0;
    public float alienSlowEnemy = 0;
    public float alienRegeneration = 0;
    public float alienExpBonus = 1;
    public float alienMiningBonus = 1;
    public int alienCapacity = 1;
    public int alienTentaclesGroup = 1;
    #endregion

    #region SHOP UPDATE CHARACTERISTICS
    public float shopResourcesBonus = 1;
    public float shopExpBonus = 1;
    public float shopDamageBonus = 1;
    #endregion

    #region PLAYER MINING DATA

    #endregion

    #region GAME PROPERTIES
    public float Damage => (mainDamage + alienDamage) * shopDamageBonus;

    public float HP => mainHP + alienHP;

    public float Speed => mainSpeed * alienSpeedBonus;

    public float Capacity => mainCapacity + alienCapacity;

    public float Radius => mainRadius + alienRadius;
    #endregion

    #region 
    public void UpdateEXP(int exp)
    {
        EXP += exp;
        if (EXP > 200) lvl++;
    }

    #endregion
}
