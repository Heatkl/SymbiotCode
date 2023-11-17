using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : NPCBehaviour
{

    [Header("Additional Settings")]


    [Range(0, 10)]
    public int damage = 0;

    public bool canAccelerate = false;
#if UNITY_EDITOR
    [ConditionalHide("canAccelerate", true)]
#endif
    public float Acceleration = 0f;
#if UNITY_EDITOR
    [ConditionalHide("canAccelerate", true)]
#endif
    public float maxSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
