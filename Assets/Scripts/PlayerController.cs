using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SKILL{
    JUMP,
    BLOCK,
    ROLL,
    ATTACK1,
    ATTACK2,
    ATTACK3
}

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollForce = 6.0f;
    [SerializeField] bool noBlood = false;
    [SerializeField] GameObject slideDust;

    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRange = 0.5f;

    // rate of character
    public int maxHeath;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int dame;
    public int defense;

    // end rate character
    public LayerMask enemyLayers;

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

    private List<Skill> skills= new List<Skill>();
    Skill skillJump = new Skill{name="Jump", description="Player can double jump"};
    Skill skillRolling = new Skill{name="Rolling", description="Player can rolling"};
    Skill skillBlock = new Skill{name="Block", description="Player can block weapon of enemies"};
    Skill skillAttack1 = new Skill{name="Attack 1", description="Player can attack 1"};
    Skill skillAttack2 = new Skill{name="Attack 2", description="Player can attack 2"};
    Skill skillAttack3 = new Skill{name="Attack 3", description="Player can attack 3"};
    private bool CanDoubleJump;
    
    // audio source
    public AudioClip attackAudio;
    public AudioClip jumpAudio;
    public AudioClip rollAudio;
    public AudioClip hurtAudio;
    private AudioSource audioSource;
    private bool hasEffectSound;
    
    
    
    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // các cảm biến về tường, cảm biến về chạm mặt đất
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        
        LoadDataCharacter();
        InitSkill();

        LoadAudio();
    }

    private void LoadAudio(){
        bool hasMusic = PlayerPrefs.GetInt("HasMusic", 0) == 0 ? false : true;
        hasEffectSound = PlayerPrefs.GetInt("HasEffectSound", 0) == 0 ? false : true;
        if(hasMusic){
            if(audioSource.isPlaying == false){
                audioSource.Play();
            }
        }
        else{
            if(audioSource.isPlaying){
                audioSource.Pause();
            }
        }
    }
    private void InitSkill(){
        skills.Add(skillJump);
        skills.Add(skillBlock);
        skills.Add(skillRolling);
        skills.Add(skillAttack1);
        skills.Add(skillAttack2);
        skills.Add(skillAttack3);
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
        // isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        // animator.SetBool("WallSlide", isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !isRolling)
        {
            if(hasEffectSound){
                audioSource.clip = hurtAudio;
                audioSource.Play();
            }
            animator.SetBool("noBlood", noBlood);
            animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !isRolling)
            animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !isRolling)
        {   
            if(skills.Contains(skillAttack1) && skills.Contains(skillAttack2) && skills.Contains(skillAttack3)){
                Attack();
                currentAttack++;
                if(hasEffectSound){
                    audioSource.clip = attackAudio;
                    audioSource.Play();
                }
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
            else if(skills.Contains(skillAttack1) && skills.Contains(skillAttack2)){
                Attack();
                currentAttack++;
                if(hasEffectSound){
                    audioSource.clip = attackAudio;
                    audioSource.Play();
                }
                // Loop back to one after third attack
                if (currentAttack > 2)
                    currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                // Call one of two attack animations "Attack1", "Attack2"
                animator.SetTrigger("Attack" + currentAttack);
                
                // Reset timer
                timeSinceAttack = 0.0f;
            }
            else if(skills.Contains(skillAttack1)){
                Attack();
                currentAttack++;
                if(hasEffectSound){
                    audioSource.clip = attackAudio;
                    audioSource.Play();
                }
                // Loop back to one after third attack
                if (currentAttack > 1)
                    currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                // Call one of three attack animations "Attack1"
                animator.SetTrigger("Attack" + currentAttack);
                
                // Reset timer
                timeSinceAttack = 0.0f;
            }
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !isRolling && skills.Contains(skillBlock))
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1) && skills.Contains(skillBlock))
            animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !isRolling && !isWallSliding  && skills.Contains(skillRolling))
        {
            if(hasEffectSound){
                audioSource.clip = rollAudio;
                audioSource.Play();
            }
            
            isRolling = true;
            animator.SetTrigger("Roll");
            
            rb.velocity = new Vector2(facingDirection * rollForce, rb.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && !isRolling  && skills.Contains(skillJump))
        {
            
            if(isGround){         
                Jump();
                CanDoubleJump = true;
            }
            else if(CanDoubleJump ){
                Debug.Log("Nhảy lần 2");
                // jumpForce = jumpForce / 1.5f;
                Jump();
                // jumpForce = jumpForce * 1.5f;
                CanDoubleJump = false;
            }
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.03f;
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

    void Attack(){
        Collider2D[] hitEnemies;
        if(facingDirection > 0){
            hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, enemyLayers);
        }else{
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
        }
        
        foreach(Collider2D e in hitEnemies){
            e.gameObject.GetComponent<EnemyController>().ReduceHealth(GetDame());
            Debug.Log("we hit "+ e.name + " dame " +GetDame());
        }
    }

    void Jump(){
        if(hasEffectSound){
            audioSource.clip = jumpAudio;
            audioSource.Play();     
        }
        
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

    private void LoadDataCharacter(){
        maxHeath = PlayerPrefs.GetInt("MaxHealth", 1000);
        currentHealth = PlayerPrefs.GetInt("CurrentHealth", 1000);
        maxMana = PlayerPrefs.GetInt("MaxMana", 1000);
        currentMana = PlayerPrefs.GetInt("CurrentMana", 1000);
        dame = PlayerPrefs.GetInt("Dame", 10);
        defense = PlayerPrefs.GetInt("Defense",10);
    }

    public int GetDame(){
        int dameCurrentWeapon = GameController.instance.GetCurrentWeapon() == null ? 0 : GameController.instance.GetCurrentWeapon().dame;
        return dame + dameCurrentWeapon;
    }
    public int GetDefense(){
        int currentDefense = GameController.instance.GetCurrentArmor() == null ? 0 : GameController.instance.GetCurrentArmor().defense;
        return defense + currentDefense;
    }

    public int GetCurrentMana(){
        return currentMana;
    }

    public int GetMaxMana(){
        return maxMana;
    }

    public int GetCurrentHealth(){
        return currentHealth;
    }

    public int GetMaxHealth(){
        int currentHealthArmor = GameController.instance.GetCurrentArmor() == null ? 0 : GameController.instance.GetCurrentArmor().HP;
        return maxHeath + currentHealthArmor;
    }
    
    public List<Skill> GetSkillsPlayer(){
        return this.skills;
    }
}
