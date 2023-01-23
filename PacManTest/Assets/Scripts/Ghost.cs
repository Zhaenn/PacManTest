using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Vector2 ghostDirection = Vector2.right;
    public Vector2 previousDirection;

    public Sprite ghostSprite;
    public Sprite vulnerableSprite;
    public Sprite eyesSprite;
    public bool vulnerable = false;

    public int points;
    public float timeUntilRelease;
    private float currentTime;
    public bool released = false;
    public int baseSpeed;
    public int speed;
    public enum behavior {chase, flee, wander, ambush, respawn}
    public behavior currentGhostBehavior;
    public behavior mainBehavior;

    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        previousDirection = -ghostDirection;
        currentTime = timeUntilRelease;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.levelStarted)
        {
            if (released)
            {
                Vector2 movement = ghostDirection * speed * Time.fixedDeltaTime;
                Move(movement);
            }

            if (currentTime > 0f)
            {
                currentTime -= Time.fixedDeltaTime;
            }
            else
            {
                released = true;
            }
        }
        
    }

    private void Move(Vector2 movement)
    {
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    public void HitIntersection(IntersectionPoint intersection)
    {
        IntersectionPoint intersectionPoint = intersection;
        switch (currentGhostBehavior)
        {
            case behavior.chase:               

                float shortestDistanceFromPlayer = float.MaxValue;

                foreach(Vector2 d in intersection.possibleDirections)
                {
                    Vector3 newGhostPosition = transform.position + new Vector3(d.x, d.y, 0f);
                    float distanceFromPlayer = (newGhostPosition - GameManager.instance.pacman.transform.position).sqrMagnitude;

                    if(distanceFromPlayer < shortestDistanceFromPlayer)
                    {
                        ghostDirection = d;
                        shortestDistanceFromPlayer = distanceFromPlayer;
                    }
                }

                return;

            case behavior.flee:

                float highestDistanceFromPlayer = float.MinValue;

                foreach (Vector2 d in intersection.possibleDirections)
                {
                    Vector3 newGhostPosition = transform.position + new Vector3(d.x, d.y, 0f);
                    float distanceFromPlayer = (newGhostPosition - GameManager.instance.pacman.transform.position).sqrMagnitude;

                    if (distanceFromPlayer > highestDistanceFromPlayer)
                    {
                        ghostDirection = d;
                        highestDistanceFromPlayer = distanceFromPlayer;
                    }
                }

                return;

            case behavior.wander:
                //Ghost is going Left
                //Previous direction becomes Left
                previousDirection = ghostDirection;

                //Random roll from all available directions
                int directionIndex = Random.Range(0, intersection.possibleDirections.Count);                

                //If opposite of what my previous direction is = what I rolled (right in this case)
                if(-previousDirection == intersection.possibleDirections[directionIndex])
                {
                    //Remove Right direction
                    intersection.possibleDirections.Remove(intersection.possibleDirections[directionIndex]);

                    if(intersection.possibleDirections.Count > 1)
                    {
                        directionIndex = Random.Range(0, intersection.possibleDirections.Count);
                    }
                    else
                    {
                        directionIndex = 0;
                    }
                    
                }
                ghostDirection = intersection.possibleDirections[directionIndex];

                return;

            case behavior.ambush:
                return;

            case behavior.respawn:
                previousDirection = ghostDirection;
                float shortestDistanceFromSpawn = float.MaxValue;

                foreach (Vector2 d in intersection.possibleDirections)
                {
                    Vector3 newGhostPosition = transform.position + new Vector3(d.x, d.y, 0f);
                    float distanceFromSpawn = (newGhostPosition - GameManager.instance.transform.Find("GhostRespawnPoint").position).sqrMagnitude;
                    
                    if (distanceFromSpawn < shortestDistanceFromSpawn && -previousDirection != d)
                    {                       
                        ghostDirection = d;
                        shortestDistanceFromSpawn = distanceFromSpawn;
                    }                    
                }

                RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0f, Vector2.down, 1.5f, LayerMask.NameToLayer("walls"));

                if(hit.transform.gameObject.tag == "Door")
                {
                    Debug.Log("In front of door");
                    ghostDirection = Vector2.down;
                }
                else
                {
                   
                }
                return;
        }
    }

    public void GhostDeath()
    {
        GameManager.instance.AddPoints(points * GameManager.instance.pointMultiplier);
        GetComponent<SpriteRenderer>().sprite = eyesSprite;
        GetComponent<Collider2D>().enabled = true;
        currentGhostBehavior = behavior.respawn;
        speed = baseSpeed + 3;
        vulnerable = false;
        
    }
}
