using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // public Transform[] waypoints;
    int cur = 0;
    public float speed = 1f;
    Animator anim;
    public Transform scatterTarget;
    public Transform chaseTarget;
    public Transform nextTile;
    public int ghostState = 1; // start in chase mode = Ghost State: [0=frightened, 1=chase, 2=scatter, 3=eaten]

    private bool isMoving, moveDown, moveUp, moveLeft, moveRight;
    private Vector2 origPos, targetPos, destPos;
    private float timeToMove = 0.2f;
    private float scale = 5f;
    public LayerMask moveBlockLayerMask;

    private bool[] valid;
    private float[] d2t; 

    // 0: up, 1: right, 2: down, 3: left
    private Vector2[] dir = { Vector2.up, Vector2.right, Vector2.down, Vector2.left};

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


        moveRight = true;
    }

    // use the FixedUpdate function to go closer to the current waypoint, 
    // or select the next one as soon as we reached it
    void FixedUpdate()
    {

        // evaluate available movements (set initial direction)
        // move to the next tile towards chaseTarget 
        // list available movements
        // for each available movement calculate distance to chaseTarget
        // set next motion
        // if arrived recalculate motion

        if (!isMoving) {

            float maxDist = 100000f;
            float bestDist = 100000f;
            int moveDir = 5;
            
            moveUp = false;
            moveRight = false;
            moveDown = false;
            moveLeft = false;

            // check ghost state to implement motion type and destTarget
            if (ghostState == 1) { // chase
                destPos = new Vector2(chaseTarget.position.x, chaseTarget.position.y) ;
            } else if (ghostState == 2) // scatter
            {
                destPos = new Vector2(scatterTarget.position.x, scatterTarget.position.y) ;
            }

            // Debug.Log("1 - calculate valid moves end best direction through min distance: moveDir");
            for (int i=0; i<4; i++) // 0: up, 1: right, 2: down, 3: left
            {                
                // Debug.Log("2 - calculate d2t[i] for valid moves");
                if (Valid(dir[i])) {
                    maxDist = Vector2.Distance(origPos + dir[i], destPos);
                    if (maxDist < bestDist) 
                    {
                        bestDist = maxDist;
                        moveDir = i;
                    }
                } 
                // Debug.Log(origPos + " - valid dir: " + i + ":" + Valid(dir[i]) );
            }

            // Debug.Log("best dir: " + moveDir );

            if (moveDir == 0) moveUp = true;
            if (moveDir == 1) moveRight = true;
            if (moveDir == 2) moveDown = true;
            if (moveDir == 3) moveLeft = true;

            if (moveDown) {
                StartCoroutine(MoveGhost(Vector2.down));
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", -1.0f);
            }
            if (moveUp) {
                StartCoroutine(MoveGhost(Vector2.up));
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", 1.0f);
            }
            if (moveLeft) {
                StartCoroutine(MoveGhost(Vector2.left));
                anim.SetFloat("DirX", -1.0f);
                anim.SetFloat("DirY", 0);
            }
            if (moveRight) {
                StartCoroutine(MoveGhost(Vector2.right));
                anim.SetFloat("DirX", 1.0f);
                anim.SetFloat("DirY", 0);
            }

        }


    }


    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "pacman") {
            Destroy(gameObject);    // pacman dies
        }


    }

    private IEnumerator MoveGhost(Vector2 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = new Vector2(transform.position.x, transform.position.y);
        targetPos = origPos + scale * direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector2.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPos;

        isMoving = false;
    }

    bool Valid(Vector2 dir) {
        // Cast Line from 'ghost' to 'next tile'
        
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPos =  pos + dir*scale;
        RaycastHit2D hit = Physics2D.Linecast(pos, newPos, moveBlockLayerMask);
        
//        Debug.Log("from: " + pos + " to: " + newPos + " hit: " + hit.collider +  " is " + (hit.collider == true));
//        Debug.DrawRay(pos, newPos, Color.magenta);

        return (!(hit.collider == true));

    }
}
