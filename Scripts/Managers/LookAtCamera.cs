using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform mainCamera;
    Quaternion rot;
    private void Start()
    {
        rot = transform.rotation;
        mainCamera = Camera.main.transform;
        Debug.Log(rot);
    }
    void Update()
    {
        transform.rotation = rot;
    }
}
