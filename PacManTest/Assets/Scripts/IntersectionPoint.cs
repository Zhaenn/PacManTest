using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionPoint : MonoBehaviour
{
    public List<Vector2> possibleDirections = new List<Vector2>();
    public LayerMask walls;

    private void Start()
    {
        //Right    
        CheckDirection(Vector2.right);

        //Left
        CheckDirection(Vector2.left);

        //Up
        CheckDirection(Vector2.up);

        //Down
        CheckDirection(Vector2.down);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            other.GetComponent<Ghost>().HitIntersection(this);
        }      
    }


    private void CheckDirection(Vector2 directionToCheck)
    {
        if (!Physics2D.BoxCast(transform.position, new Vector2(0.75f, 0.75f), 0f, directionToCheck, 1.5f, walls))
        {
            possibleDirections.Add(directionToCheck);
        };
    }
}
