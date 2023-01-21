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
    void Update()
    {
        
        if(Input.GetAxis("Horizontal") > 0f)
        {
            //Going Right
            playerDirection = new Vector2(1f, 0f);            
        }        

        if (Input.GetAxis("Horizontal") < 0f)
        {
            Vector2 direction = new Vector2(-1f, 0f);
            //Going Left
            if (!IsThereAWall(direction))
            {
                playerDirection = direction;
            }
            
        }

        if (Input.GetAxis("Vertical") > 0f)
        {
            //Going Up
            playerDirection = new Vector2(0f, 1f);
        }

        if (Input.GetAxis("Vertical") < 0f)
        {
            //Going Down
            playerDirection = new Vector2(0f, -1f);
        }

        Vector2 movement = playerDirection * speed * Time.deltaTime;
        Move(movement);

    }

    public bool IsThereAWall(Vector2 direction)
    {
        Debug.Log(direction);
       


        
        if (Physics2D.Raycast(transform.position, direction, 1.5f, walls))
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

  /*  public IntersectionPoint FindNextIntersectionPoint()
    {

    }*/
}
