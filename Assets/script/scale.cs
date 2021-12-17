using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scale : MonoBehaviour
{
    float s;
    Vector2 canvasPosition;
    // Start is called before the first frame update
    void Start()
    {
        canvasPosition = new Vector2(UnityEngine.Screen.width/2, UnityEngine.Screen.height/2);
        s = ((float)UnityEngine.Screen.width)/826f;
        transform.localScale = new Vector3(s, s, 1f);
        transform.position = new Vector3(canvasPosition.x + (transform.position.x - canvasPosition.x) * s, canvasPosition.y + (transform.position.y - canvasPosition.y) * s, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
