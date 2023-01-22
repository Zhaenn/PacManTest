using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Dot : MonoBehaviour
{
    public int points;
 
    public void OnTriggerEnter2D(Collider2D other)
    {       
      if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
      {
            GameManager.instance.AddPoints(points);
            GameManager.instance.DotEaten(this);
            this.gameObject.SetActive(false);
      }
      else
      {
            Debug.Log("not pacman");
      }
     
            
    }
    
}
