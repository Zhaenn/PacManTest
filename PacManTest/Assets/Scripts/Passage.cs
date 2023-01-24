using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple script attached to two objects on each side of the screen allowing the player to teleport to the other side.
public class Passage : MonoBehaviour
{
    public Transform otherPassage;
    public Vector3 offset; //This is used to teleport the player slightly before the trigger so the player doesn't infinitely teleport
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            other.transform.position = otherPassage.position + offset;
        }
    }
}
