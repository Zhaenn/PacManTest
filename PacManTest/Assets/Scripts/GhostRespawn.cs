using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRespawn : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost") && GameManager.instance.levelStarted)
        {
            if(other.GetComponent<Ghost>().currentGhostBehavior == Ghost.behavior.respawn)
            {
                other.GetComponent<Ghost>().speed = other.GetComponent<Ghost>().baseSpeed;
                other.GetComponent<SpriteRenderer>().sprite = other.GetComponent<Ghost>().ghostSprite;
            }
            else
            {                
                other.GetComponent<Ghost>().ghostDirection = Vector2.up;
                other.GetComponent<Ghost>().previousDirection = Vector2.down;
            }
            other.GetComponent<Ghost>().currentGhostBehavior = other.GetComponent<Ghost>().mainBehavior;

        }
       

    }
}
