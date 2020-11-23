using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : PlayerController
{
    public bool dashUp = false;
    public bool dashDown = false;
    public float verticalAxis = 0;

    void Start()
    {
        carRb = this.GetComponent<Rigidbody2D>();
    }

    public override void Movement()
    {
        Vector3 translation = new Vector3(speed * Time.deltaTime, 0, 0);
        this.transform.Translate(translation);

        Vector3 translation2 = new Vector3(0, verticalAxis * verticalSpeed * Time.deltaTime, 0);
        this.transform.Translate(translation2);
    }

    public override void DashManagement()
    {
        if (dashAvailable)
        {
            dashAvailable = false;
            if (dashUp)
            {
                    dashUp = false;
                    dashingUp = true;
                    _lastDashTime = Time.time;
           
            }

            if (dashDown)
            {
                    dashDown = false;
                    dashingDown = true;
                    _lastDashTime = Time.time;                
            }
        }
    }
}
