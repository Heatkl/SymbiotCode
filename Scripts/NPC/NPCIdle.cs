using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : NPCBehaviour
{

    private void Start()
    {
        StartSettings();
    }
    private void Update()
    {
        MainLogic();
    }
    //public override void IdleWalk()
    //{
    //    base.IdleWalk();
    //    Debug.Log("Derived Class Method");
    //}

}
