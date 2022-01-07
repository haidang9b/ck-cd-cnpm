using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public GameObject pickUpEffect;
    public string thisIdSkill;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerController>().HasSkillByID(thisIdSkill)){
            gameObject.SetActive(false);
        }
        else{
            gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            GameObject clone = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            Destroy(clone, 1.0f);
            player.GetComponent<PlayerController>().AddSkillByID(thisIdSkill);
        }
    }
}
