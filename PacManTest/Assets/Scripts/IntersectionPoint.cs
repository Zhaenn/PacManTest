using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Is used in the rule tile for invisible intersection points placed at each possible intersection. 
// There are also some on the edges of the passages and right in front of the ghost box.
public class IntersectionPoint : MonoBehaviour
{
    public List<Vector2> possibleDirections = new List<Vector2>();
    public LayerMask walls;

    private void Start()
    {
        CheckDirection(Vector2.right);
        CheckDirection(Vector2.left);
        CheckDirection(Vector2.up);
        CheckDirection(Vector2.down);
    }

    //When a ghost enters one of these, it sends the info of the intersection to a function in ghost managing all their behaviors.
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            other.GetComponent<Ghost>().HitIntersection(this);
        }      
    }

    //Simple boxcast to check if a wall is in that direction. If not, it adds that direction to a list of possible directions the ghost can take.
    private void CheckDirection(Vector2 directionToCheck)
    {
        if (!Physics2D.BoxCast(transform.position, new Vector2(0.75f, 0.75f), 0f, directionToCheck, 1.5f, walls))
        {
            possibleDirections.Add(directionToCheck);
        };
    }
}
