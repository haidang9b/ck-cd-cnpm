using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundEnemy : Enemy
{
    public float speed = 1.0f;
    public bool isRamming = false;
    private GameObject player;
    [SerializeField] private float cooldown = 3.0f;
    private float cooldownTimer;
    public bool aggro = false;
    public float distanceToAttack = 1f;
    private float initialSpeed;

    private EnemyState currentState;
    private Animator animation;

    private Vector2 startPosition;
    private PlayerController playerController;
    private Transform playerTransform;

    public Healthbar healthbar;
    private int currentHealth = 200;
    private int maxHealth = 200;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animation = GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        initialSpeed = speed;
        healthbar.SetHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        checkDistance();
        switch (currentState)
        {
            case EnemyState.attack:
                cooldownTimer -= Time.deltaTime;

                if (cooldownTimer > 0)
                {
                    return;
                }

                cooldownTimer = cooldown;

                return;

            case EnemyState.back:
                back();
                return;
            case EnemyState.follow:
                Flip();
                follow();
                return;
        }
    }

    public void follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    public void back()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
    }

    private void checkDistance()
    {
        // Debug.Log(Vector2.Distance(playerTransform.position, transform.position));
        if (Vector2.Distance(startPosition, transform.position) > 7f)
        {
            currentState = EnemyState.back;
        }
        else if (Vector2.Distance(playerTransform.position, transform.position) <= 5f) 
        {
            currentState = EnemyState.follow;
        }
        else if (Vector2.Distance(startPosition, transform.position) == 0)
        {
            currentState = EnemyState.idle;
        }
        // Debug.Log(currentState);
    }

    //private void Ram()
    //{
    //    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    //    if (Vector2.Distance(transform.position, player.transform.position) <= distanceToAttack)
    //    {
    //        cooldownTimer -= Time.deltaTime;

    //        if (cooldownTimer > 0) {
    //            speed = initialSpeed;
    //        }

    //        cooldownTimer = cooldown;

    //        speed = speed * 1.5f;
    //    }
    //    else
    //    {

    //    }
    //}

    private void Flip()
    {
        if (transform.position.x > playerTransform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerController.reduceHealth(100);
            Debug.Log("OnCollisionEnter2D");
            healthbar.SetHealth(currentHealth - 10, maxHealth);
            currentHealth -= 10;
        }
    }
}
