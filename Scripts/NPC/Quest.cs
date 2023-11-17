using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [Header("Quest UI Objects")]
    public GameObject questButton;
    public Text questText;
    public int rewardAmount;
    public Action questAction;

    [Header("Quest text")]
    [TextArea(3,10)]
    public string startText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HeroClosely();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HeroFar();
        }
    }

    private void HeroClosely()
    {
        questButton.SetActive(true);
    }

    private void HeroFar()
    {
        questButton.SetActive(false);
    }
}
