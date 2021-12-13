using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helper : MonoBehaviour
{
    public float refreshTime;

    float refresh;
    GameObject child;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.Find("helper0").gameObject;
        refresh = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!child.activeInHierarchy && refresh < 0){
            refresh = refreshTime;
        }
        refresh -= Time.deltaTime;
        if(refresh < 0){
            child.SetActive(true);
        }
    }
}
