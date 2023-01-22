using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 playerDirection = new Vector2(1f, 0f);
    public int speed;
    private Rigidbody2D rigidBody;
    public LayerMask walls;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(Input.GetAxis("Horizontal") > 0f)
        {
            //Going Right
            Vector2 direction = new Vector2(1f, 0f);

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
            Vector2 direction = new Vector2(-1f, 0f);
            
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
            Vector2 direction = new Vector2(0f, 1f);

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
            Vector2 direction = new Vector2(0f, -1f);

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

    public bool IsThereAWall(Vector2 direction)
    {
        Debug.Log(direction);
       
        if (Physics2D.BoxCast(transform.position, new Vector2(0.75f, 0.75f), 0f, direction, 1.5f, walls))
        {
            Debug.DrawRay(transform.position, direction * 1.5f, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 1.5f, Color.red);
            return false;
        }
  
        
    }

    public void Move(Vector2 movement)
    {
        rigidBody.MovePosition(rigidBody.position + movement);
    }
}
