using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlyingEnemy : Enemy
{
    public float speed = 4.0f;
    public bool isRamming = false;
    private GameObject player;
    [SerializeField] private float cooldown = 1.0f;
    private float cooldownTimer;
    public bool aggro = false;
    public float distanceToAttack = 10f;
    // private float enemyBulletSpeed = 10.0f;

    public GameObject bullet;

    private EnemyState currentState;
    private Animator animation;

    private Vector2 startPosition;
    private PlayerController playerController;
    private Transform playerTransform;

    public Healthbar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animation = GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        healthbar.SetHealth(currentHealth, maxHealth);
        bullet.GetComponent<Bullet>().SetDame(dameAttack);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayObject(); // kiểm tra xem nó có nằm trong danh sách enemy đã bị tiêu diệt hay không? nếu có thì ẩn nó đi
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
            case EnemyState.idle:
                return;
        }
    }

    public void follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) <= distanceToAttack)
        {
            ShootAtPlayer();
        }
        else
        {

        }
    }

    private void checkDistance()
    {
        //Debug.Log(Vector2.Distance(playerTransform.position, transform.position));
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
            transform.rotation = Quaternion.Euler(0, 0, 0);
            currentState = EnemyState.idle;
        }
        //Debug.Log(currentState);
    }

    public void back()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void Ram() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) <= distanceToAttack) {
            ShootAtPlayer();
        } else {
            
        }
    }

    private void Flip() {
        if (transform.position.x > player.transform.position.x) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ShootAtPlayer()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer > 0) return;

        cooldownTimer = cooldown;
        
        GameObject tempBullet = Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject; //shoots from enemies eyes
        
        Destroy(tempBullet, 3f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            playerController.reduceHealth(50);
            healthbar.SetHealth(currentHealth - 10, maxHealth);
            currentHealth -= 10;
        }
    }
}
