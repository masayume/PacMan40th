using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PacmanMove : MonoBehaviour
{
    public float speed = 20f;
    public Transform movePoint;
    Vector2 dest = Vector2.zero;
    Vector2 dir;
    Animator anim;

    public LayerMask MoveBlockLayerMask;

    void Start()
    {
        movePoint.parent = null;
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
        
        Vector3 inputX = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        Vector3 inputY = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);

        // move around checking for colliders that block movement
        if( Vector3.Distance(transform.position, movePoint.position) < 5f )
        {
            if( System.Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f ) 
            {
                // if there is no object in front
                if (!Physics2D.OverlapCircle(movePoint.position + inputX, 1f, MoveBlockLayerMask) )
                {
                    movePoint.position += inputX;
                    anim.SetFloat("DirX", Input.GetAxisRaw("Horizontal"));
                }
            } else if( System.Math.Abs(Input.GetAxisRaw("Vertical")) == 1f ) 
            {
                if (!Physics2D.OverlapCircle(movePoint.position + inputY, 1f, MoveBlockLayerMask) )
                {
                    movePoint.position += inputY;
                    anim.SetFloat("DirY", Input.GetAxisRaw("Vertical"));
                }
            }   
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
