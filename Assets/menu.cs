using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    bool isShow;
    GameObject menuObject;
    // Start is called before the first frame update
    void Start()
    {
        isShow = false;
        menuObject = transform.Find("menu").gameObject;
        menuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("menu")){
            if(isShow){
                continueGame();
            }else{
                isShow = true;
                Time.timeScale = 0f;
                menuObject.SetActive(true);
            }
        }
    }

    public void continueGame(){
        isShow = false;
        Time.timeScale = 1f;
        menuObject.SetActive(false);
    }

    public void quitGame(){
        Application.Quit();
    }
}
