using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taiji : MonoBehaviour
{
    public float cdTime;
    float remindTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        remindTime -= Time.deltaTime;
        if(remindTime < 0){
            transform.Find("taiji").gameObject.SetActive(true);
        }
        transform.RotateAround(transform.position, new Vector3(0, 0, -1), Time.deltaTime * 360);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "player" && remindTime < 0){
            remindTime = cdTime;
            transform.Find("taiji").gameObject.SetActive(false);
        }
    }
}
