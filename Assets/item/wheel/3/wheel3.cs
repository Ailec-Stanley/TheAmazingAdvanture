using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel3 : MonoBehaviour
{
    public float speed;

    GameObject m;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        m = transform.Find("main").gameObject;
        rb = m.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);
    }

    private void Update() {
        m.transform.RotateAround(m.transform.position, new Vector3(0, 0, -1), Time.deltaTime * 800);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if(m.transform.position.y > transform.Find("up").transform.position.y && rb.velocity.y > 0){
            rb.velocity = new Vector2(0, -speed);
        }
        if(m.transform.position.y < transform.Find("down").transform.position.y && rb.velocity.y < 0){
            rb.velocity = new Vector2(0, speed);
        }
    }
}
