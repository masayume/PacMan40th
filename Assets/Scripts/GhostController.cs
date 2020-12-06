using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // public Transform[] waypoints;
    int cur = 0;
    public float speed = 5f;
    Animator anim;
    public Transform scatterTarget;
    public Transform chaseTarget;
    public Transform nextTile;
    public int ghostState = 1; // start in chase mode = Ghost State: [0=frightened, 1=chase, 2=scatter, 3=eaten]

    // Pac-Man Ghost AI Explained https://www.youtube.com/watch?v=ataGotQ7ir8

    // when state change: turnaround otherwise NO turnaround allowed
    // foreach TileEntered calculate each move (no turnaround) with DistanceToTarget and choose the one nearer
    //   same distance direction priority order: UP, LEFT, DOWN, RIGHT
    // Blinky Target: pac-man tile
    // Pinky Target: 4 in front of pac-man tile
    // Inky Target: f(Pac-Man, Blinky): rotate(vector(Blinky - 2 front pacman), 180°)
    // Clyde Target: if distance(pacman - clyde) < 8 then pacman else scatter target (clyde corner) 
    // during Scatter each Ghost targets a different corner
    // during Frightened turnaround & random at each tile

    // todo pathfinding: place pacman, blinky randomly & draw the path to pacman (blinky target)

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // use the FixedUpdate function to go closer to the current waypoint, 
    // or select the next one as soon as we reached it
    void FixedUpdate()
    {
        // check ghost state to implement motion
        if (ghostState == 1) { // chase
            // move to the next tile towards chaseTarget 
            // list available movements
            // for each available movement calculate distance to chaseTarget
            // set next motion
            // if arrived recalculate motion
        } else if (ghostState == 2) // scatter
        {
            // move to the next tile towards scatterTarget 

        }

        if (transform.position != nextTile.position) {
            /*
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
            */
            transform.position = Vector3.MoveTowards(transform.position, nextTile.position, speed * Time.deltaTime);

        }
        // Waypoint reached, select next one
        else {
            // cur = (cur + 1) % waypoints.Length;
        }

        // Animation
        Vector2 dir = nextTile.position - transform.position;
        anim.SetFloat("DirX", dir.x);
        anim.SetFloat("DirY", dir.y);
    }


    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "pacman") {
            Destroy(gameObject);    // pacman dies
        }


    }

}
