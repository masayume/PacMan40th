using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PacmanMove : MonoBehaviour
{
    public float speed = 20f;
    public Transform movePoint;
    Vector2 dest = Vector2.zero;
    Vector2 dir;
    int scale = 5;
    Animator anim;
    float lastHorizontalValue;
    float lastVerticalValue;
    float nextHorizontalValue;
    float nextVerticalValue;
    int EventCount = 0;

    public LayerMask MoveBlockLayerMask;

    void Start()
    {
        movePoint.SetParent(null);
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        // each frame pacman moves towards its GameObject movePoint, a guide controlled by the player
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        // REFRESH AND STORE pacman movement direction (for animator and movement)    
        // PROBLEM: pacman stops on orthogonal input against a wall, but shouldn't stop at all agaist this type of wall
        // SOLUTION: don't get input if movePoint collides    


        // get Input
        float inputHor = Input.GetAxisRaw("Horizontal");
        float inputVer = Input.GetAxisRaw("Vertical");

        // calculate potential movement and collision
        Vector3 inputX = new Vector3(nextHorizontalValue, 0, 0);
        Vector3 inputY = new Vector3(0, nextVerticalValue, 0);

        bool blockedMoveX = Physics2D.OverlapCircle(movePoint.position + scale*inputX, 1f, MoveBlockLayerMask);
        bool blockedMoveY = Physics2D.OverlapCircle(movePoint.position + scale*inputY, 1f, MoveBlockLayerMask);

        // if collision then no movement applyed
        if ( inputHor != 0 && !blockedMoveX )  
        {
                nextHorizontalValue = inputHor;
                nextVerticalValue = 0;
        }
        if ( inputVer != 0  && !blockedMoveY)
        {
                nextVerticalValue = inputVer;
                nextHorizontalValue = 0;
        }

        if (blockedMoveX) 
            nextVerticalValue = lastVerticalValue;
        if (blockedMoveY) 
            nextHorizontalValue = lastHorizontalValue;

        // MOVE LOOP - if pacman is not arrived at destination...
        if( Vector3.Distance(transform.position, movePoint.position) < 0.1f )
        {
            if( System.Math.Abs(nextHorizontalValue) == 1f ) 
            {
                // checking for horizontal colliders that block movement
                // if ( !blockedMoveX )
                if ( true )
                {
                    movePoint.position += scale*inputX;
                    anim.SetFloat("DirX", nextHorizontalValue);
                    anim.SetFloat("DirY", 0);
                    lastHorizontalValue = nextHorizontalValue;
                } else {
                    EventCount++;
                    // keep on moving vert
                }

            } 
            if( System.Math.Abs(nextVerticalValue) == 1f ) 
            {
                // checking for vertical colliders that block movement
                // if ( !blockedMoveY )
                if ( true )
                {
                    movePoint.position += scale*inputY;
                    anim.SetFloat("DirY", nextVerticalValue);
                    anim.SetFloat("DirX", 0);
                    lastVerticalValue = nextVerticalValue;
                } else {
                    EventCount++;
                    // keep on moving horiz
                }

            }   
        } else {

        }

        // mvpX, mvpY coords are the next cell by input, and blockedMove value is correct
        /*
        Debug.Log(  "mvpX: " + (movePoint.position + scale*inputX) + " mvpY:" + (movePoint.position + scale*inputY) + 
                    "LHorizV:" + lastHorizontalValue + " - LVertV: " + lastVerticalValue + 
                    " blockedMoveX: " + blockedMoveX + " blockedMoveY " + blockedMoveY
                );
        */
    }

    bool valid(Vector2 dir) {
    // Cast Line from 'next to Pac-Man' to 'Pac-Man'

        // Vector2 pos = transform.position;
        // RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        // return (hit.collider == GetComponent<Collider2D>());

        return true;
    }

    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.name == "wall")  // or if(gameObject.CompareTag("YourWallTag"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
