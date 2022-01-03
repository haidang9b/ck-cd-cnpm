using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatorBullet : MonoBehaviour
{
    private float speed = 3f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody2D>().velocity = (player.transform.position - this.transform.position).normalized * speed;
    }
}
