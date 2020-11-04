using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public float verticalSpeed = 8.0f;
    public float maxSpeed = 20;
    private const string AXIS_H = "Horizontal", AXIS_V = "Vertical";
    public bool dashAvailable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        

    }
    private void FixedUpdate()
    {
        SpeedAceleration();

    }

    public void Movement()
    {

        Vector3 translation = new Vector3(speed * Time.deltaTime, 0, 0);
        this.transform.Translate(translation);


        if (Mathf.Abs(Input.GetAxisRaw(AXIS_V)) > 0.2f)
        {
            Vector3 translation2 = new Vector3(0, Input.GetAxisRaw(AXIS_V) * verticalSpeed * Time.deltaTime, 0);
            this.transform.Translate(translation2);

        }
    }

    public bool Dash(bool dashAvailable)
    {

        return false;
    }

    public void SpeedAceleration()
    {
        if(speed<=maxSpeed)
        speed += 0.02f;
    }

}