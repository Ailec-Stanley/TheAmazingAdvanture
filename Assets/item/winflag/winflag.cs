using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class winflag : MonoBehaviour
{
    public string nextName; 
    public GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "player"){
            DontDestroyOnLoad(ui);
            SceneManager.LoadScene(nextName);
        }
    }
}
