using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //RB
    Rigidbody2D carRb;
    //movement variables
    public float speed = 1.0f;
    public float verticalSpeed = 8.0f;
    public float maxSpeed = 16;

    //dash variables
    public float dashTimeGap = 0.3f;
    public float dashCoolDown = 5.0f;
    public bool dashAvailable = true;    
    public float _startDashTime=0.1f;

    public float dashSpeed = 20;
    private bool dashingUp = false;
    private bool dashingDown = false;
    private float _dashPressedTime; //time when dash button was pressed last
    private float _lastDashTime = 0; //time when dash was last used
    private float _dashTime;


    //input variables
    private const string AXIS_H = "Horizontal", AXIS_V = "Vertical";
    
    
    // Start is called before the first frame update
    void Start()
    {
        carRb = this.GetComponent<Rigidbody2D>();
        _dashPressedTime = Time.time;
        _dashTime = _startDashTime;

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

    public void DashManagement()
    {   

        if (dashAvailable)
        {
            
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                float timeSinceLastPress = Time.time - _dashPressedTime;
                
                if(timeSinceLastPress <= dashTimeGap)
                {                   
                    
                    dashingUp = true;
                    _lastDashTime = Time.time;
                }
                else
                {                  
                    
                }
                _dashPressedTime = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                float timeSinceLastPress = Time.time - _dashPressedTime;

                if (timeSinceLastPress <= dashTimeGap)
                {
                    
                    dashingDown = true;
                    _lastDashTime = Time.time;
                }
                else
                {
                    
                }
                _dashPressedTime = Time.time;
            }
        }        
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

    public void DashCooldown()
    {
        dashAvailable = (Time.time - _lastDashTime) > dashCoolDown;
    }

    public void SpeedAceleration()
    {
        if(speed<=maxSpeed)
        speed += 0.02f;
    }

}