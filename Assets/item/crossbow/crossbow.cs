using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossbow : MonoBehaviour
{
    public GameObject arrow;

    Animator anim;
    public float cdTime;
    public float speed;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = cdTime;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        time -= Time.deltaTime;
        if(time > 0){
            anim.SetBool("shooting", false);
        }else{
            anim.SetBool("shooting", true);
            createArrow();
            time = cdTime;
        }
    }

    void createArrow(){
        GameObject arr = Instantiate(arrow, new Vector3(transform.position.x, transform.position.y + 0.3f * transform.localScale.y, 0), Quaternion.identity);
        arr.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        arr.GetComponent<Rigidbody2D>().velocity = new Vector3(-transform.localScale.x * speed, 0, 0);
    }
}
