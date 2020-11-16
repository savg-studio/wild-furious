using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //input constants
    private const string AXIS_H = "Horizontal", AXIS_V = "Vertical";

    //RB
    protected Rigidbody2D carRb;
    //movement variables
    public float speed = 8.0f;
    public float verticalSpeed = 4.0f;
    public float maxSpeed = 35;
    public float acceleration = 1f;

    //dash variables
    public float dashTimeGap = 0.3f;
    public float dashCoolDown = 0.8f;
    public bool dashAvailable = true;    
    public float _startDashTime=0.3f;

    public float dashSpeed = 20;
    protected bool dashingUp = false;
    protected bool dashingDown = false;
    protected float _dashPressedTime; //time when dash button was pressed last
    protected float _lastDashTime = 0; //time when dash was last used
    protected float _dashTime;

    // Additional variables for special power-ups
    public bool inverted = false;
    
    
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

    public virtual void Movement()
    {
        Vector3 translation = new Vector3(speed * Time.deltaTime, 0, 0);
        this.transform.Translate(translation);

        float verticalAxis = Input.GetAxisRaw(AXIS_V);
        if (inverted) verticalAxis = -verticalAxis;
        if (Mathf.Abs(verticalAxis) > 0.2f)
        {
            Vector3 translation2 = new Vector3(0, verticalAxis * verticalSpeed * Time.deltaTime, 0);
            this.transform.Translate(translation2);
        }
    }

    public virtual void DashManagement()
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
        if(speed < maxSpeed)
        {
            speed *= 1 + (acceleration * Time.fixedDeltaTime);
        }
    }

}