using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float deleteTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        deleteTime -= Time.deltaTime;
        if(deleteTime < 0){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "platform") {
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
