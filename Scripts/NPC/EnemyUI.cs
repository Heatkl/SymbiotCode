using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    TMP_Text percent;
    Slider hpSlider;
    //Transform mainCamera;
    Transform _transform;
    Quaternion rot;


    private void Awake()
    {
        hpSlider = GetComponentInChildren<Slider>();
        percent = hpSlider.gameObject.GetComponentInChildren<TMP_Text>();
        _transform = transform;
        rot = _transform.rotation;
    }
    private void Start()
    {
        
        //mainCamera = Camera.main.transform;
        //ActivateEnemyRewards("GemReward/MeatReward", 12, 55);
    }
    void Update()
    {
        _transform.rotation = rot;
    }


    public void ChangeValue(float value)
    {
        hpSlider.value = value;
        percent.text = (int)((hpSlider.value / hpSlider.maxValue) * 100) + "%";
    }

    public void SetValue(float maxValue)
    {
        hpSlider.maxValue = maxValue;
        hpSlider.value = maxValue;
    }

    public void ActivateEnemyRewards(string jsonNames, List<int> quantity)
    {
        string[] rewardsName = jsonNames.Split("/");
        for(int i = 0; i < rewardsName.Length; i++)
        {
            GameObject reward = _transform.Find("EnemyUI/RewardPanel/"+rewardsName[i]).gameObject;
            reward.SetActive(true);
            reward.GetComponentInChildren<TMP_Text>().text = quantity[i].ToString();
        }
    }

    
}
