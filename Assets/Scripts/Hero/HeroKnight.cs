using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollForce = 6.0f;
    [SerializeField] bool noBlood = false;
    [SerializeField] GameObject slideDust;

    private Animator animator;
    private Rigidbody2D rb;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool isWallSliding = false;
    private bool isGround = false;
    private bool isRolling = false;
    private int facingDirection = 1;
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private float rollDuration = 8.0f / 14.0f;
    private float rollCurrentTime;

    private bool CanDoubleJump;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // các cảm biến về tường, cảm biến về chạm mặt đất
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(isRolling)
            rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(rollCurrentTime > rollDuration)
            isRolling = false;

        //Check if character just landed on the ground
        if (!isGround && m_groundSensor.State())
        {
            isGround = true;
            animator.SetBool("Grounded", isGround);
        }

        //Check if character just started falling
        if (isGround && !m_groundSensor.State())
        {
            isGround = false;
            animator.SetBool("Grounded", isGround);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
        }

        // Move
        if (!isRolling ){
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
            
        }
            

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        animator.SetBool("WallSlide", isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !isRolling)
        {
            animator.SetBool("noBlood", noBlood);
            animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !isRolling)
            animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !isRolling)
        {
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 3)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animator.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !isRolling)
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !isRolling && !isWallSliding)
        {
            isRolling = true;
            animator.SetTrigger("Roll");
            
            rb.velocity = new Vector2(facingDirection * rollForce, rb.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && !isRolling)
        {
            if(isGround){
                Jump();
                CanDoubleJump = true;
            }
            else if(CanDoubleJump ){
                Debug.Log("Nhảy lần 2");
                jumpForce = jumpForce / 1.5f;
                Jump();
                jumpForce = jumpForce * 1.5f;
                CanDoubleJump = false;
            }
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle

            delayToIdle -= Time.deltaTime;
            if(delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }
    }

    void Jump(){
        animator.SetTrigger("Jump");
        isGround = false;
        animator.SetBool("Grounded", isGround);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        m_groundSensor.Disable(0.2f);
    }
    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }

    
}
