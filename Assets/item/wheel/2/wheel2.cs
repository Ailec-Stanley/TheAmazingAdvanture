using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel2 : MonoBehaviour
{
    public float speed;

    GameObject m;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        m = transform.Find("main").gameObject;
        rb = m.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
    }

    private void Update() {
        m.transform.RotateAround(m.transform.position, new Vector3(0, 0, -1), Time.deltaTime * 800);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(m.transform.position.x > transform.Find("right").transform.position.x && rb.velocity.x > 0){
            rb.velocity = new Vector2(-speed, 0);
        }
        if(m.transform.position.x < transform.Find("left").transform.position.x && rb.velocity.x < 0){
            rb.velocity = new Vector2(speed, 0);
        }
    }
}
