using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    Transform cam;
    Vector2 startPoint;
    public float moveRate;
    public bool yMove;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        startPoint = new Vector2(transform.position.x, transform.position.y - cam.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(yMove){
            transform.position = new Vector2(startPoint.x + cam.position.x * moveRate, startPoint.y + cam.position.y);
        }else{
            transform.position = new Vector2(startPoint.x + cam.position.x * moveRate, transform.position.y);
        }
        
    }
}
