using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hint : MonoBehaviour
{
    public GameObject hintPanel;
    public GameObject text;
    public string inputText;
    // Start is called before the first frame update
    void Start()
    {
        hintPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "player"){
            text.GetComponent<Text>().text = inputText;
            hintPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "player"){
            hintPanel.SetActive(false);
        }
    }
}
