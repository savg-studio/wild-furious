using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking: MonoBehaviour
{

    public Vector3 cameraOffset = new Vector3(0.0f, 0f, -20f);
    private Transform target;
    

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;

    }

    // Update is called once per frame


    private void LateUpdate()
    {       
            this.transform.position = target.TransformPoint(cameraOffset);
            this.transform.LookAt(target);
       
    }
}
