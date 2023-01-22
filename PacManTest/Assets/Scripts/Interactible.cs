using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    public int points;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            GameManager.instance.AddPoints(points);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("not pacman");
        }


    }

   
}

