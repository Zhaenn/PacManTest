using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 playerDirection = Vector2.right;
    public Vector3 startPosition;
    public int speed;
    private Rigidbody2D rigidBody;
    public LayerMask walls; //The layer mask containing the wall tiles in the tilemap. Used to detect if the player can go in that direction or not.

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }
   
    // Fixed Update contains all the movement logic for the player. When using the axis in a particular direction, it checks if there's a wall in that direction.
    // If not, the player moves in that direction using a rigidbody and the MovePosition function. Also rotates the player to face the right way
    void FixedUpdate()
    {
        if (GameManager.instance.levelStarted) //Player can't move if the level hasn't started yet.
        {
            if (Input.GetAxis("Horizontal") > 0f)
            {
                //Going Right
                Vector2 direction = Vector2.right;

                if (!IsThereAWall(direction))
                {
                    playerDirection = direction;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.rotation = rotation;
                }
            }

            if (Input.GetAxis("Horizontal") < 0f)
            {
                //Going Left
                Vector2 direction = Vector2.left;

                if (!IsThereAWall(direction))
                {
                    playerDirection = direction;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 180f);
                    transform.rotation = rotation;
                }

            }

            if (Input.GetAxis("Vertical") > 0f)
            {
                //Going Up
                Vector2 direction = Vector2.up;

                if (!IsThereAWall(direction))
                {
                    playerDirection = direction;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
                    transform.rotation = rotation;
                }
            }

            if (Input.GetAxis("Vertical") < 0f)
            {
                //Going Down
                Vector2 direction = Vector2.down;

                if (!IsThereAWall(direction))
                {
                    playerDirection = direction;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, -90f);
                    transform.rotation = rotation;
                }
            }

            Vector2 movement = playerDirection * speed * Time.fixedDeltaTime;
            Move(movement);

        }

    }

    //Used to check if there's a walls in a given direction. Returns if there's a hit or not
    public bool IsThereAWall(Vector2 direction)
    {
        return Physics2D.BoxCast(transform.position, new Vector2(0.75f, 0.75f), 0f, direction, 1.5f, walls);
    }

    //Move Function called at the end of FixedUpdate. Simply adds the current position to a movement vector consisting of direction, speed and deltatime.
    public void Move(Vector2 movement)
    {
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    //Collision with the Ghosts. Check if they are vulnerable. If so, they die. If not, player dies.
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            if (other.gameObject.GetComponent<Ghost>().vulnerable)
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());                
                
                other.gameObject.GetComponent<Ghost>().Invoke("GhostDeath", 0.1f);
            }
            else if (other.gameObject.GetComponent<Ghost>().currentGhostBehavior != Ghost.behavior.respawn)
            {
                GameManager.instance.PacmanDeath();
            }
        }
    }

}
