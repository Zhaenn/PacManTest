using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : Dot
{
    //The Energizer is a dot variant which also triggers the vulnerable state on all ghosts during a given time before returning them to default state. 
    //This can be changed at anytime (increasing or decreasing according to the current level, for example).
    public float vulnerableTime;
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman") && GameManager.instance.levelStarted)
        {
            GameManager.instance.AddPoints(points);
            GameManager.instance.DotEaten(this);
            
           foreach(Ghost g in GameManager.instance.allGhosts)
            {
                g.currentGhostBehavior = Ghost.behavior.flee;
                g.GetComponent<SpriteRenderer>().sprite = g.vulnerableSprite;
                g.vulnerable = true;
            }

            GameManager.instance.Invoke("ReturnGhostToNormal", vulnerableTime);

            this.gameObject.SetActive(false);
        }
    }
}
