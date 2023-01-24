using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The basic class from everything interactible. The dot and energizer both inherit from this and the Fruit prefab also has it.
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
    }
}

