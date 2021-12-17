using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargingEvent : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void relocate(){
        player.transform.position = player.GetComponent<player>().savedPoint;
    }

    void resurrect(){
        player.GetComponent<player>().isDead = false;
        gameObject.SetActive(false);
    }
}
