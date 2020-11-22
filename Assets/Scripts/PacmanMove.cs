using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    public float speed = 4f;
    Vector2 dest = Vector2.zero;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        dest = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        Debug.Log("started Pacman Controller");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
/*
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);
*/

        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        // Check for Input if not moving
        if ((Vector2)transform.position == dest) {
            if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up)) {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right)) {
                dest = (Vector2)transform.position + Vector2.right;
            }   
            if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up)) {
                dest = (Vector2)transform.position - Vector2.up;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right)) {
                dest = (Vector2)transform.position - Vector2.right;
            }
        }                

        // Animation Parameters
        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

    }

    bool valid(Vector2 dir) {
    // Cast Line from 'next to Pac-Man' to 'Pac-Man'
       Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }


}
