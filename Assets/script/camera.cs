using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform player;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 2, -50f);
    }
}
