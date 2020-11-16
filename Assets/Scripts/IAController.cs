using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : PlayerController
{
    public bool dashArriba = false;
    public bool dashAbajo = false;

    // Start is called before the first frame update
    void Start()
    {
        carRb = this.GetComponent<Rigidbody2D>();
        //_dashPressedTime = Time.time;
        //_dashTime = _startDashTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PowerUp powerUp = collision.GetComponent<PowerUp>();
        if (powerUp != null) powerUp.OnCollected(gameObject);
        else
        {
            Transform posPlayer = collision.GetComponent<Transform>();
            if (this.transform.position.y > posPlayer.transform.position.y)
            {
                //Debug.Log("Dashear hacia abajo");
                dashAbajo = true;
            }
            else
            {
                //Debug.Log("Dashear hacia arriba");
                dashArriba = true;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(this.transform.position.y > -73)
        {
            dashAbajo = true;
        }

        if (this.transform.position.y < -92)
        {
            dashArriba = true;
        }


    }

    public override void Movement()
    {
        Vector3 translation = new Vector3(speed * Time.deltaTime, 0, 0);
        this.transform.Translate(translation);
    }

    public override void DashManagement()
    {

        if (dashAvailable)
        {
            dashAvailable = false;
            if (dashArriba)
            {
                    dashArriba = false;
                    dashingUp = true;
                    _lastDashTime = Time.time;
           
            }

            if (dashAbajo)
            {
                    dashAbajo = false;
                    dashingDown = true;
                    _lastDashTime = Time.time;                
            }
        }
    }
}
