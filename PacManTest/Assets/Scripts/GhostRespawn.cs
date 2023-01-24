using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//There are two objects with this script attached. One is on top of the ghost box and one is inside. The one inside has the tag spawn
//This is used to respawn the ghosts when they are in the respawn behavior or make them exit the box when they first are released. They then cannot enter the box anymore
public class GhostRespawn : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost") && GameManager.instance.levelStarted)
        {
            
            if(other.GetComponent<Ghost>().currentGhostBehavior == Ghost.behavior.respawn) //This one is for the object OUTSIDE of the ghost box
            {
                other.GetComponent<Ghost>().speed = other.GetComponent<Ghost>().baseSpeed;
                other.GetComponent<SpriteRenderer>().sprite = other.GetComponent<Ghost>().ghostSprite;
                Physics2D.IgnoreCollision(GameManager.instance.pacman.GetComponent<Collider2D>(), other.GetComponent<Collider2D>(), false);
                other.GetComponent<Ghost>().currentGhostBehavior = other.GetComponent<Ghost>().mainBehavior;
            }
            else if(gameObject.tag == "Spawn") //This one is for the object IN the box
            {                
                other.GetComponent<Ghost>().ghostDirection = Vector2.up;
                other.GetComponent<Ghost>().previousDirection = Vector2.down;
                other.GetComponent<Ghost>().currentGhostBehavior = other.GetComponent<Ghost>().mainBehavior;
            }
        }
    }
}
