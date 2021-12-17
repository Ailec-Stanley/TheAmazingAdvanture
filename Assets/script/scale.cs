using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scale : MonoBehaviour
{
    float s_x;
    float s_y;
    Vector2 canvasPosition;
    // Start is called before the first frame update
    void Start()
    {
        s_x = ((float)UnityEngine.Screen.width)/826f;
        s_y = ((float)UnityEngine.Screen.height)/364f;
        transform.localScale = new Vector3(transform.localScale.x * s_x, transform.localScale.y * s_x, 1f);
        transform.localPosition = new Vector3(transform.localPosition.x * s_x, transform.localPosition.y * s_y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
