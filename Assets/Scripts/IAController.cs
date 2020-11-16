using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{

    

    //input constants
    private const string AXIS_H = "Horizontal", AXIS_V = "Vertical";

    //RB
    Rigidbody2D carRb;
    //movement variables
    public float speed = 8.0f;
    public float verticalSpeed = 4.0f;
    public float maxSpeed = 35;
    public float acceleration = 1f;

    //dash variables
    public float dashTimeGap = 0.3f;
    public float dashCoolDown = 0.8f;
    public bool dashAvailable = true;
    public float _startDashTime = 0.3f;

    public float dashSpeed = 20;
    private bool dashingUp = false;
    private bool dashingDown = false;
    public bool dashArriba = false;
    public bool dashAbajo = false;
    private float _dashPressedTime; //time when dash button was pressed last
    private float _lastDashTime = 0; //time when dash was last used
    private float _dashTime;

    // Additional variables for special power-ups
    public bool inverted = false;


    // Start is called before the first frame update
    void Start()
    {
        carRb = this.GetComponent<Rigidbody2D>();
        //_dashPressedTime = Time.time;
        //_dashTime = _startDashTime;

    }

    // Update is called once per frame
    void Update()
    {
        DashCooldown();
        Movement();
        DashManagement();
    }

    private void FixedUpdate()
    {
        SpeedAceleration();

        if (dashingUp)
        {
            DashingUp();
        }
        else if (dashingDown)
        {
            DashingDown();
        }
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
                Debug.Log("Dashear hacia abajo");
                dashAbajo = true;
            }
            else
            {
                Debug.Log("Dashear hacia arriba");
                dashArriba = true;
            }
        }


        

 


    }

    public void Movement()
    {

        Vector3 translation = new Vector3(speed * Time.deltaTime, 0, 0);
        this.transform.Translate(translation);

    }

    public void DashingUp()
    {
        if (_dashTime > 0.1)
        {
            _dashTime -= Time.deltaTime;
            carRb.velocity = Vector2.up * dashSpeed;

        }
        else
        {
            carRb.velocity = Vector2.up * 0;
            dashingUp = false;
            _dashTime = _startDashTime;
        }

    }

    public void DashingDown()
    {
        if (_dashTime > 0.1)
        {
            _dashTime -= Time.deltaTime;
            carRb.velocity = Vector2.down * dashSpeed;

        }
        else
        {
            carRb.velocity = Vector2.down * 0;
            dashingDown = false;
            _dashTime = _startDashTime;
        }

    }


    public void SpeedAceleration()
    {
        if (speed < maxSpeed)
        {
            speed *= 1 + (acceleration * Time.fixedDeltaTime);
        }
    }

    public void DashManagement()
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

    public void DashCooldown()
    {
        dashAvailable = (Time.time - _lastDashTime) > dashCoolDown;
    }
}
