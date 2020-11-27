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

    public LayerMask MoveBlockLayerMask;

    void Start()
    {
        movePoint.SetParent(null);
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
        
        if (Input.GetAxisRaw("Horizontal") != 0) {
            lastHorizontalValue = Input.GetAxisRaw("Horizontal");
            lastVerticalValue = 0;
        }
        if (Input.GetAxisRaw("Vertical") != 0) {
            lastVerticalValue = Input.GetAxisRaw("Vertical");
            lastHorizontalValue = 0;
        }

        Vector3 inputX = new Vector3(lastHorizontalValue, 0, 0);
        Vector3 inputY = new Vector3(0, lastVerticalValue, 0);

        // move around checking for colliders that block movement
        if( Vector3.Distance(transform.position, movePoint.position) < 0.1f )
        {
            if( System.Math.Abs(lastHorizontalValue) == 1f ) 
            {
                // if there is no object in front
                if (!Physics2D.OverlapCircle(movePoint.position + scale*inputX, 1f, MoveBlockLayerMask) )
                {
                    movePoint.position += scale*inputX;
                    anim.SetFloat("DirX", lastHorizontalValue);
                    anim.SetFloat("DirY", 0);
                }
            } 
            else if( System.Math.Abs(lastVerticalValue) == 1f ) 
            {
                if (!Physics2D.OverlapCircle(movePoint.position + scale*inputY, 1f, MoveBlockLayerMask) )
                {
                    movePoint.position += scale*inputY;
                    anim.SetFloat("DirY", lastVerticalValue);
                    anim.SetFloat("DirX", 0);
                }
            }   
        } else {

        }

    }

    // Update is called once per frame
    /*
    void FixedUpdate()
    {

        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        // Check for Input if not moving
        if ((Vector2)transform.position == dest) {
            if (Input.GetKey(KeyCode.UpArrow) ) {
            // if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up)) {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if (Input.GetKey(KeyCode.RightArrow) ) {
            // if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right)) {
                dest = (Vector2)transform.position + Vector2.right;
            }   
            if (Input.GetKey(KeyCode.DownArrow) ) {
            // if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up)) {
                dest = (Vector2)transform.position - Vector2.up;
            }
            if (Input.GetKey(KeyCode.LeftArrow) ) {
            // if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right)) {
                dest = (Vector2)transform.position - Vector2.right;
            }
        }                

        // Animation Parameters
        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

    }
    */


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
