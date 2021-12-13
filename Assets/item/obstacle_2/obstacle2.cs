using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle2 : MonoBehaviour
{
    public float waitTime;
    public float speed;
    public float low, high;

    Rigidbody2D rb;
    float time;
    bool up;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        up = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time -= Time.deltaTime;
        if(time > 0){
            rb.velocity = new Vector3(0, 0, 0);
            return;
        }
        if(up && transform.position.y < high){
            rb.velocity = new Vector3(0, speed, 0);
        }else if(up && transform.position.y >= high){
            rb.velocity = new Vector3(0, -speed, 0);
            up = false;
            time = waitTime;
        }else if(!up && transform.position.y > low ){
            rb.velocity = new Vector3(0, -speed, 0);
        }else if(!up && transform.position.y <= low){
            rb.velocity = new Vector3(0, speed, 0);
            up = true;
            time = waitTime;
        }
    }
}
