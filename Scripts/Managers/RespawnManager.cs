using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    
    public Dictionary<DateTime, int> respawnTime = new();
    Dictionary<DateTime, int> copy;

    private void Awake()
    {
        
    }
    public void RespawnSeconds(int unit, int seconds)
    {
        respawnTime.Add(DateTime.Now.AddSeconds(seconds), unit);
    }
    public void RespawnMinutes(int unit, int minutes)
    {
        respawnTime.Add(DateTime.Now.AddMinutes(minutes), unit);
    }

    public void RespawnHours(int unit, int hours)
    {
        respawnTime.Add(DateTime.Now.AddHours(hours), unit);
    }

    private void Update()
    {
        CheckSpawn();
    }

    async void CheckSpawn()
    {
        copy = new Dictionary<DateTime, int>(respawnTime);
        foreach(var resp in copy)
        {
            if (resp.Key < DateTime.Now)
            {
                GameManager.instance.NPC.GetChild(resp.Value).gameObject.SetActive(true);
                respawnTime.Remove(resp.Key);
            }
            else
            {
                GameManager.instance.NPC.GetChild(resp.Value).gameObject.SetActive(false);
            }
                await Task.Yield();
        }
    }


}
