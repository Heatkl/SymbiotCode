using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void Loadd(int index)
    {
        SceneManager.LoadScene(index);
    }
}
