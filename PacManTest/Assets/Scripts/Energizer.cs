using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : Dot
{
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
        else
        {
            Debug.Log("not pacman");
        }


    }
}
