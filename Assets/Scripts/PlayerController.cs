using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //  áp dụng các lực : tốc độ, nhày, lăn
    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollForce = 6.0f;
    [SerializeField] bool noBlood = false;
    [SerializeField] GameObject slideDust;

    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRange = 0.5f;

    // rate of character: tỉ lệ máu, mana
    public int idLevelCurrent;
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int dame;
    public int defense;

    // end rate character

    public LayerMask interactableLayers;
    public LayerMask enemyLayers;

    private Animator animator;
    private Rigidbody2D rb;
    // các cảm biến khi chạm
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

    private Vector3 startPosition;
    private List<Skill> skills= new List<Skill>();
    //  các skill của player 
    Skill skillJump = new Skill{name="Jump", description="Player can double jump", mana=10};
    Skill skillRolling = new Skill{name="Rolling", description="Player can rolling", mana=15};
    Skill skillBlock = new Skill{name="Block", description="Player can block weapon of enemies", mana=20};
    Skill skillAttack1 = new Skill{name="Attack 1", description="Player can attack 1", mana=20};
    Skill skillAttack2 = new Skill{name="Attack 2", description="Player can attack 2", mana=20};
    Skill skillAttack3 = new Skill{name="Attack 3", description="Player can attack 3", mana=20};
    private bool CanDoubleJump;
    
    // audio source
    public AudioClip attackAudio;
    public AudioClip jumpAudio;
    public AudioClip rollAudio;
    public AudioClip hurtAudio;
    private AudioSource audioSource;
    private bool hasEffectSound;
    
    // load levels
    List<Level> levels;

    // tiền user có được
    private int coins;

    // thời gian bắt đầu tính action tự động hồi máu + mana
    private float nextActionTime = 0.0f;
    // thời gian hồi sau bao nhiêu giây ( ở đây là 1 giây)
    public float period = 1f;


    // Use this for initialization
    void Start ()
    {
        startPosition = transform.position;
        PlayerPrefs.DeleteAll();
        idLevelCurrent = PlayerPrefs.GetInt("idLevel",1);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // các cảm biến về tường, cảm biến về chạm mặt đất
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        levels = new List<Level>();
        InitDataLevel();
        LoadDataCharacter();
        InitSkill();

        LoadAudio();
    }

    private void InitDataLevel(){
        levels.Add(new Level{idLevel =1, dameAttack = 10, defense = 10, maxMana = 1000, maxHealth = 1000, cost =1000});
        levels.Add(new Level{idLevel =2, dameAttack = 20, defense = 20, maxMana = 2000, maxHealth = 2000, cost =2000});
        levels.Add(new Level{idLevel =3, dameAttack = 30, defense = 30, maxMana = 3000, maxHealth = 3000, cost =3000});
        levels.Add(new Level{idLevel =4, dameAttack = 40, defense = 40, maxMana = 4000, maxHealth = 4000, cost =4000});
        levels.Add(new Level{idLevel =5, dameAttack = 50, defense = 50, maxMana = 5000, maxHealth = 5000, cost =5000});
        levels.Add(new Level{idLevel =6, dameAttack = 60, defense = 60, maxMana = 6000, maxHealth = 6000, cost =6000});
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
        // tăng máu + mana mỗi giây
        autoUpHealthAndMana();

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

        // ĐỔi chiều nhân vât theo input đầu vào
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

        // Di chuyển - move
        if (!isRolling ){
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
            
        }
            

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        // isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        // animator.SetBool("WallSlide", isWallSliding);

        // Chết - Death
        if(currentHealth <= 0){
            
            if(hasEffectSound){
                audioSource.clip = hurtAudio;
                audioSource.Play();
            }
            animator.SetBool("noBlood", noBlood);
            animator.SetTrigger("Death");
            currentHealth = (int)0.7f*maxHealth;
            currentMana = (int)0.7f*maxMana;
            PlayerPrefs.SetInt("CurrentHealth", currentHealth);
            PlayerPrefs.SetInt("CurrentMana", currentMana);
            transform.position = startPosition;
        }
        // if (Input.GetKeyDown("e") && !isRolling)
        // {
        //     if(hasEffectSound){
        //         audioSource.clip = hurtAudio;
        //         audioSource.Play();
        //     }
        //     animator.SetBool("noBlood", noBlood);
        //     animator.SetTrigger("Death");
        // }
            
        //Hurt
        // else if (Input.GetKeyDown("q") && !isRolling)
        //     animator.SetTrigger("Hurt");

        // Xử lý tấn công
        else if(Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !isRolling && currentMana > skillAttack1.mana)
        {   
            if(skills.Contains(skillAttack1) && skills.Contains(skillAttack2) && skills.Contains(skillAttack3)){
                Attack();
                currentAttack++;
                if(hasEffectSound){
                    audioSource.clip = attackAudio;
                    audioSource.Play();
                }
                // Kiểm tra xem có phải đang là skill attack cuối hay không, nếu phải thì quay ngược lại
                if (currentAttack > 3)
                    currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                // Gọi 1 trong 3 animations "Attack1", "Attack2", "Attack3"
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
                // Kiểm tra xem có phải đang là skill attack cuối hay không, nếu phải thì quay ngược lại
                if (currentAttack > 2)
                    currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                // Gọi 1 trong 2 animations "Attack1", "Attack2"
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
                // Kiểm tra xem có phải đang là skill attack cuối hay không, nếu phải thì quay ngược lại
                if (currentAttack > 1)
                    currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                // Gọi 1 animations "Attack1"
                animator.SetTrigger("Attack" + currentAttack);
                
                // Reset timer
                timeSinceAttack = 0.0f;
            }
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !isRolling && skills.Contains(skillBlock) && currentMana >= skillBlock.mana)
        {
            reduceMana(skillBlock);
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1) && skills.Contains(skillBlock))
            animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !isRolling && !isWallSliding  && skills.Contains(skillRolling)  && currentMana >= skillRolling.mana )
        {
            reduceMana(skillRolling);
            if(hasEffectSound){
                audioSource.clip = rollAudio;
                audioSource.Play();
            }
            
            isRolling = true;
            animator.SetTrigger("Roll");
            
            rb.velocity = new Vector2(facingDirection * rollForce, rb.velocity.y);
        }
            
        // dame enemy = dame enemy - defense player 
        // Jump -  Xử lý nhảy 1 lần và nhảy 2 lần
        else if (Input.GetKeyDown("space") && !isRolling  && skills.Contains(skillJump) &&  currentMana >= skillJump.mana )
        {
            if(isGround){ // nếu đang là mặt đất thì có thể nhảy        
                Jump();
                CanDoubleJump = true; // gọi nhảy 1 lần , thức là first time thì có thể nhảy lần 2
            }
            else if(CanDoubleJump ){ // nhảy lần 2
                Jump();
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

        // giao tiếp vs NPC
        if(Input.GetKeyDown(KeyCode.Z)){
            InteractNPC();
        }
    }

    // xử lý giao tiếp vs npc
    void InteractNPC(){
        if(facingDirection > 0){
            var collider = Physics2D.OverlapCircle(attackPointRight.position, 0.3f, interactableLayers);
            if(collider != null){
                collider.GetComponent<Interactable>()?.Interact();
            }
        }
        else{
            var collider = Physics2D.OverlapCircle(attackPointLeft.position, 0.3f, interactableLayers);
            if(collider != null){
                collider.GetComponent<Interactable>()?.Interact();
            }
        }
    }

    // xử lý tấn công
    void Attack(){
        reduceMana(skillAttack1); // bởi vì mana như nhau nên trừ bầng một cái
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
    // xử lý nhảy
    void Jump(){
        reduceMana(skillJump);
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

    // load data cho character
    private void LoadDataCharacter(){
        Level thisLevel = getCurrentLevel();
        coins = PlayerPrefs.GetInt("coins", 100000);   
        maxHealth = thisLevel.maxHealth;
        currentHealth = PlayerPrefs.GetInt("CurrentHealth", 1000);
        maxMana = thisLevel.maxMana;
        currentMana = PlayerPrefs.GetInt("CurrentMana", 1000);
        dame = thisLevel.dameAttack;
        defense = thisLevel.defense;
    }



    //  xử lý các chỉ số cơ bản
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
        return maxHealth;
    }
    
    public List<Skill> GetSkillsPlayer(){
        return this.skills;
    }
    // tự động tăng máu mỗi giây
    private void autoUpHealthAndMana(){
        if(Time.time > nextActionTime){ // check thời gian sau mỗi giây
            nextActionTime = Time.time + period;

            int rateRecovered = 3; // số lượng máu + mana hồi mỗi giây theo cấp độ
            addMana(rateRecovered*idLevelCurrent);
            addHealth(rateRecovered*idLevelCurrent);
        }
        
    }

    // Giảm mana khi dùng chiêu
    private void reduceMana(Skill skill){
        if(currentMana >= skill.mana){
            currentMana = currentMana - skill.mana;
        }
    }
    public void addMana(int mana){
        currentMana += mana;
        if(currentMana > maxMana){
            currentMana = maxMana;
        }
    }

    public void addHealth(int hp){
        currentHealth = currentHealth + hp;
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
    }
    public Level getNextLevel(){
        try{
            int idFinder = idLevelCurrent + 1;
            if(idFinder == 7){
                return null;
            }
            return levels.Find(level => level.idLevel == idFinder);
        }
        catch(Exception e){
            return null;
        }
    }

    public Level getCurrentLevel(){
        return levels.Find(level => level.idLevel == idLevelCurrent);
    }

    public bool reduceCoin(int money){
        if(coins < money){
            return false;
        }
        coins = coins - money;
        PlayerPrefs.SetInt("coins", coins);
        return true;
    }

    public int GetCoins(){
        return coins;
    }

    // Xử lý up level cho nhân vật
    public void UpgradeLevel(){
        idLevelCurrent++;
        PlayerPrefs.SetInt("idLevel", idLevelCurrent);
        Debug.Log("UpgradeLevel " + idLevelCurrent);
        LoadDataCharacter();
        currentMana += 1000;
        currentHealth += 1000;
        PlayerPrefs.SetInt("CurrentHealth", currentHealth);
        PlayerPrefs.SetInt("CurrentMana", currentMana);

        // cần save data ở đây
    }

    public void reduceHealth(int dame){
        animator.SetTrigger("Hurt");
        int dameNeedReduce = dame - GetDefense();
        currentHealth = currentHealth - dameNeedReduce;
        PlayerPrefs.SetInt("CurrentHealth", currentHealth);
    }
}
