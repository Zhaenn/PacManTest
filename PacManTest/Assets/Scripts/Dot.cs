using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Dot : Interactible
{   
    // The Dot class is present on the prefab used in the Rule Tile to make all the dots appear on screen and have interactibility.
         
    public override void OnTriggerEnter2D(Collider2D other)
    { 
      //The same as the basic interactible collision class, but adds the DotEaten call in the Game Manager
      if (other.gameObject.layer == LayerMask.NameToLayer("Pacman") && GameManager.instance.levelStarted)
      {
            GameManager.instance.AddPoints(points);
            GameManager.instance.DotEaten(this);
            this.gameObject.SetActive(false);
      } 
    }
    
}
