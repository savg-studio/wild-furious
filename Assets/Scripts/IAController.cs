using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{

    public float speed = 5.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = new Vector3(speed * Time.deltaTime, 0, 0);
        this.transform.Translate(translation);
    }
}
