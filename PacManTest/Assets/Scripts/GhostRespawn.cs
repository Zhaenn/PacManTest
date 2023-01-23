using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRespawn : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost") && GameManager.instance.levelStarted)
        {
            other.GetComponent<Ghost>().currentGhostBehavior = other.GetComponent<Ghost>().mainBehavior;
            other.GetComponent<Ghost>().ghostDirection = Vector2.up;
            other.GetComponent<Ghost>().previousDirection = Vector2.down;
        }
       

    }
}
