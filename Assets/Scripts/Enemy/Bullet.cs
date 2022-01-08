using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20;
    // Start is called before the first frame update
    private GameObject player;
    private int dame = 50;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().reduceHealth(this.dame);
            Destroy(this.gameObject);
        }
    }

    public void SetDame(int dame){
        this.dame = dame;
    }
}
