using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAtilleryEnemy : MonoBehaviour
{

    public float speed = 4.0f;
    public bool isRamming = false;
    private GameObject player;
    [SerializeField] private float cooldown = 1.0f;
    private float cooldownTimer;
    public bool aggro = false;
    public float distanceToAttack = 10f;
    private float kiteCooldownTimer;
    private float kiteCooldown = 2.0f;
    private bool isKiting = false;
    // private float enemyBulletSpeed = 10.0f;

    public GameObject bullet;

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
        cooldownTimer = 0; 
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

                ShootAtPlayer();

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
        if (Vector2.Distance(startPosition, transform.position) > 7f)
        {
            currentState = EnemyState.back;
        }
        else if (Vector2.Distance(playerTransform.position, transform.position) <= 5f)
        {
            currentState = EnemyState.attack;
        }
        else if (Vector2.Distance(startPosition, transform.position) == 0)
        {
            currentState = EnemyState.idle;
        }
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ShootAtPlayer()
    {
        GameObject tempBullet = Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject; //shoots from enemies eyes

        Destroy(tempBullet, 2.5f);
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
