using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public float speed = 4.0f;
    public bool isRamming = false;
    private GameObject player;
    [SerializeField] private float cooldown = 3.0f;
    private float cooldownTimer;
    public bool aggro = false;
    public float distanceToAttack = 1f;
    private float initialSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialSpeed = speed;
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
            Ram();
            Flip();
        }
    }

    private void Ram()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) <= distanceToAttack)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer > 0) {
                speed = initialSpeed;
            }

            cooldownTimer = cooldown;

            speed = speed * 1.5f;
        }
        else
        {

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
}
