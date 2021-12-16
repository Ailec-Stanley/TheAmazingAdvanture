using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class finalWin : MonoBehaviour
{
    Text t;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
        time = GameObject.Find("Timer").transform.Find("Text").gameObject.GetComponent<timer>().game_time;
        t.text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
