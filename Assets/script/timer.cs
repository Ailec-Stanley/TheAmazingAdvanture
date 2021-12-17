using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public float game_time;
    Text t;
    // Start is called before the first frame update
    void Start()
    {
        game_time = 0f;
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        t.text = game_time.ToString("#0.0") + " S";

    }

    void FixedUpdate()
    {
        game_time += Time.deltaTime;
    }
}
