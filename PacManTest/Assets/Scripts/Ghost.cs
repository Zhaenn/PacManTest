using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 ghostDirection = Vector2.right;
    private Vector2 startingDirection;
    public Vector2 previousDirection;
    private Rigidbody2D rigidBody;

    [Header("Vulnerable")]
    public Sprite ghostSprite;
    public Sprite vulnerableSprite;
    public Sprite eyesSprite;
    public bool vulnerable = false;

    [Header("Game Data")]
    public int points;

    public int baseSpeed;
    public int speed;

    public float timeUntilRelease;
    private float currentTime;
    public bool released = false;

    private bool defaultReleaseState;
    private Vector3 startingPosition;
  
    [Header("Ghost Behavior")]
    public behavior currentGhostBehavior;
    public behavior mainBehavior; //The behavior the ghost will return to after going into another behavior (respawn or flee for example)
    public enum behavior { chase, flee, wander, respawn } //All the behaviors the ghost can have. Defined in more details in HitIntersection function

    //Initialization of different values used when resetting the game/level
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        previousDirection = -ghostDirection;
        currentTime = timeUntilRelease;
        defaultReleaseState = released;
        startingPosition = transform.position;
        startingDirection = ghostDirection;
    }

    // Simple movement logic. Simply moves according to a direction if the level is started and the ghost is released.
    //The direction changes every time a ghost hits an intersection point.
    void FixedUpdate()
    {
        if (GameManager.instance.levelStarted)
        {
            if (released)
            {
                Vector2 movement = ghostDirection * speed * Time.fixedDeltaTime;
                Move(movement);
            }

            //Used to release the ghost one at a time according to a timer instead of all at once
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

    //Same movement function as the player. Moves according to a movement vector consisting of direction, speed and deltatime
    private void Move(Vector2 movement)
    {
        rigidBody.MovePosition(rigidBody.position + movement);
    }


    //Triggers every time a ghost hits an intersection point. This is the main function determining the ghost behavior, where they move and how they act
    //There are 4 main behaviors as defined in the enum earlier. Each one is fairly simple, but act in slightly different ways.
    public void HitIntersection(IntersectionPoint intersection)
    {
        IntersectionPoint intersectionPoint = intersection; //storing the intersection point in another variable to avoid messing with its properties.

        switch (currentGhostBehavior)
        {
            //The Chase behavior will make the ghost follow the player and take the shortest path towards the players. This is done using a hypothetical
            //movement in all possible directions to check if that would put the ghost closer to the player. It then chooses the shortest path and takes that direction.
            //As is the case for all behaviors, the ghost cannot return in the same direction it just came in to avoid it going back and forth between two points.
            //Unless there is only one direction to go in (only occurs in the passages), then the ghost takes that direction instead.
            case behavior.chase:

                previousDirection = ghostDirection;
                float shortestDistanceFromPlayer = float.MaxValue; //Make this float maximum for the first check to always result in it being shorter

                if(intersectionPoint.possibleDirections.Count > 1)
                {
                    foreach (Vector2 d in intersectionPoint.possibleDirections)
                    {
                        Vector3 newGhostPosition = transform.position + new Vector3(d.x, d.y, 0f); //Hypothetical movement in direction
                        float distanceFromPlayer = (newGhostPosition - GameManager.instance.pacman.transform.position).sqrMagnitude;

                        //opposite of previous direction is not the direction we're currently checking (if previous direction = left, we don't want to go right)
                        if (distanceFromPlayer < shortestDistanceFromPlayer && -previousDirection != d)
                        {
                            ghostDirection = d;
                            shortestDistanceFromPlayer = distanceFromPlayer;
                        }
                    }
                }
                else
                {
                    ghostDirection = intersectionPoint.possibleDirections[0];
                }
               
                return;

                //The flee behavior is the same as the chase behavior, but will try to get away from the player as much as possible. This is mainly used when the ghosts are
                //in their vulnerable state. It is also used by default by Clyde (orange ghost) to emulate how he was in the original game.
            case behavior.flee:

                previousDirection = ghostDirection;
                float highestDistanceFromPlayer = float.MinValue;

                if (intersectionPoint.possibleDirections.Count > 1)
                {
                    foreach (Vector2 d in intersectionPoint.possibleDirections)
                    {
                        Vector3 newGhostPosition = transform.position + new Vector3(d.x, d.y, 0f);
                        float distanceFromPlayer = (newGhostPosition - GameManager.instance.pacman.transform.position).sqrMagnitude;

                        if (distanceFromPlayer > highestDistanceFromPlayer && -previousDirection != d)
                        {
                            ghostDirection = d;
                            highestDistanceFromPlayer = distanceFromPlayer;
                        }
                    }
                }
                else
                {
                    ghostDirection = intersectionPoint.possibleDirections[0];
                }

                return;

                //The wander behavior is the random one of the bunch. When entering an intersection, it will take a random direction between the possible directions
                //If it rolls the opposite of what my direction is, it will instead take another direction.
            case behavior.wander:
               
                previousDirection = ghostDirection;
                int directionIndex = 0;
                if (intersectionPoint.possibleDirections.Count > 1)
                {
                    directionIndex = Random.Range(0, intersectionPoint.possibleDirections.Count); //Since Random Range is exclusive, we use the total amount in the list as we're using the index in an array

                    if (-previousDirection == intersectionPoint.possibleDirections[directionIndex]) //If the chosen direction is the opposite of what my previous direction is
                    {
                        directionIndex++; //This simply makes it so it chooses another direction

                        if (directionIndex + 1 > intersectionPoint.possibleDirections.Count) //We then make sure the index isn't out of range
                        {
                            directionIndex = 0;
                        }
                    }
                }
                else
                {
                    directionIndex = 0;
                }

                ghostDirection = intersectionPoint.possibleDirections[directionIndex];

                return;    

                //The respawn behavior is used when the ghosts have been eaten by the player. They then essentially to the same as the chase behavior, but they chase a respawn point located on top
                //of the ghost box. The GhostRespawnPoint transform is located under the Game Manager empty object.
            case behavior.respawn:
                previousDirection = ghostDirection;
                float shortestDistanceFromSpawn = float.MaxValue;

                foreach (Vector2 d in intersectionPoint.possibleDirections)
                {
                    Vector3 newGhostPosition = transform.position + new Vector3(d.x, d.y, 0f);
                    float distanceFromSpawn = (newGhostPosition - GameManager.instance.transform.Find("GhostRespawnPoint").position).sqrMagnitude;
                    
                    if (distanceFromSpawn < shortestDistanceFromSpawn && -previousDirection != d)
                    {                       
                        ghostDirection = d;
                        shortestDistanceFromSpawn = distanceFromSpawn;
                    }                    
                }
                return;
        }
    }

    //Function triggered when the ghosts are eaten by the player.
    public void GhostDeath()
    {
        GameManager.instance.AddPoints(points * GameManager.instance.pointMultiplier);
        GameManager.instance.pointMultiplier *= 2; //The player gains more points according to a multiplier if ghost are eaten in succession
        GetComponent<SpriteRenderer>().sprite = eyesSprite;    
        currentGhostBehavior = behavior.respawn;
        speed = baseSpeed + 3; //The ghost go slightly faster when they are in this state then they would otherwise.
        vulnerable = false;
    }

    //Basic reset function triggered when the level or game is reset.
    public void ResetState()
    {
        GetComponent<SpriteRenderer>().sprite = ghostSprite;
        vulnerable = false;
        speed = baseSpeed;
        currentGhostBehavior = mainBehavior;
        currentTime = timeUntilRelease;
        released = defaultReleaseState;
        transform.position = startingPosition;
        ghostDirection = startingDirection;
    }
}
