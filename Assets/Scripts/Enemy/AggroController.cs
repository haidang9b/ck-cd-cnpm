using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroController : MonoBehaviour
{
    private GameObject child;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector2.Distance(transform.position, player.transform.position));
        //Debug.Log(child.GetComponent<BoxCollider2D>().size.x);
        if (Vector2.Distance(transform.position, player.transform.position) <= child.GetComponent<BoxCollider2D>().size.x / 10)
        {
            if (this.GetComponent<FlyingEnemy>() != null)
            {
                this.GetComponent<FlyingEnemy>().aggro = true;
            }
        }
        if (this.GetComponent<GroundEnemy>() != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= child.GetComponent<BoxCollider2D>().size.x)
            {
                this.GetComponent<GroundEnemy>().aggro = true;
            }
        }
        if (this.GetComponent<FlyingAtilleryEnemy>() != null)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= child.GetComponent<BoxCollider2D>().size.x)
            {
                this.GetComponent<FlyingAtilleryEnemy>().aggro = true;
            }
        }
        //else
        //{
        //    if (this.GetComponent<FlyingEnemy>() != null)
        //    {
        //        this.GetComponent<FlyingEnemy>().aggro = false;
        //    }
        //    if (this.GetComponent<GroundEnemy>() != null)
        //    {
        //        this.GetComponent<GroundEnemy>().aggro = false;
        //    }
        //    if (this.GetComponent<FlyingAtilleryEnemy>() != null)
        //    {
        //        this.GetComponent<FlyingAtilleryEnemy>().aggro = false;
        //    }
        //}
    }
}
