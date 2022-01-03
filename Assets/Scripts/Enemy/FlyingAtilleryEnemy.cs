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
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (aggro)
        {


            if (player == null)
            {
                return;
            }
            if (bullet == null)
            {
                return;
            }
            Flip();
            ShootAtPlayer();
            // Kite();
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
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer > 0) return;

        cooldownTimer = cooldown;

        GameObject tempBullet = Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject; //shoots from enemies eyes

        Destroy(tempBullet, 2.5f);
    }
}
