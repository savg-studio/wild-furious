﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking: MonoBehaviour
{
    private Transform target;
    public float cameraXOffset = 20f;
    public float cameraYOffset = 0f;
    public float cameraZOffset = 0f;

    void Start()
    {
        target = GameObject.Find("Player").transform;

    }
    
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = target.transform.position.x;
        transform.position = pos + new Vector3 (cameraXOffset, cameraYOffset, cameraZOffset); 
    }
}
