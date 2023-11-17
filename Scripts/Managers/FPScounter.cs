using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class FPScounter : MonoBehaviour
{
    TMP_Text fpsText;
    int counter = 0;
    int index = 0;
    float fps = 0;
    void Start()
    {
        fpsText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        
        SpinAndDisableButton();
    }

    public async void SpinAndDisableButton()
    {
        //if (index != 0)
          //  return;
            counter++;
        fps += 1.0f / Time.deltaTime;
        if (counter == 100)
        {
            index++;
            string _text = "";
                
                _text = string.Format("FPS: {0}, Gems: {1}, Crystalls: {2}, Meat: {3}, EXP: {4}, LEVEL: {5}", (int)(fps / counter), GameManager.instance.progress.gem, GameManager.instance.progress.crystall, GameManager.instance.progress.meat, GameManager.instance.progress.EXP, GameManager.instance.progress.lvl);
                await Task.Yield();
            fpsText.text = _text;
            fps = 0;
            counter = 0;
        }
    }
}
