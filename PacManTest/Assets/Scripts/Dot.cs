using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int points;
 
    public void OnTriggerEnter2D(Collider2D other)
    {       
      if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
      {
            Debug.Log("hit pacman");
            this.gameObject.SetActive(false);
      }
      else
      {
            Debug.Log("not pacman");
      }
     
            
    }
    
}
