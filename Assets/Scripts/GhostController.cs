using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // public Transform[] waypoints;
    int cur = 0;
    int actionCtr = 0;
    int lastDir = 0;            // direction of the last movement (0: up, 1: right, 2: down, 3: left)
    public float speed = 1f;
    Animator anim;
    public Transform scatterTarget;
    public Transform chaseTarget;
    public Transform nextTile;
    public int ghostState = 1; // start in chase mode = Ghost State: [0=frightened, 1=chase, 2=scatter, 3=eaten]

    private bool isMoving, moveDown, moveUp, moveLeft, moveRight;
    private Vector2 origPos, targetPos, destPos; // actor position; next move position; destination position (scatter/chase)
    private float timeToMove = 1f;
    private float scale = 5f;
    public LayerMask moveBlockLayerMask;

    private bool[] valid;
    private float[] d2t; 
    
    // 0: up, 1: right, 2: down, 3: left
    private Vector2[] dir = { Vector2.up, Vector2.right, Vector2.down, Vector2.left};
    private int[] reverse = { 2, 3, 0, 1};

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
    void Update()
    {

        // evaluate available movements (set initial direction)
        // move to the next tile towards chaseTarget 
        // list available movements
        // for each available movement calculate distance to chaseTarget
        // set next motion
        // if arrived recalculate motion

        if (!isMoving) {
            
            actionCtr++;
            float maxDist = 100000f;
            float bestDist = 100000f;
            int moveDir = -1;
            
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
                // check any direction, if there are no walls and it's not a reverse
                if ( Valid(dir[i]) && !(lastDir == reverse[i]) ) {
                    maxDist = Vector2.Distance(origPos + dir[i], destPos);
                    if (maxDist < bestDist) 
                    {
                        bestDist = maxDist;
                        moveDir = i;
                    }
                } 
            }

            Debug.Log("act: " + actionCtr + " best: " + moveDir + "=== origPos:" + origPos + 
            " Pos:" + transform.position.x + "," + transform.position.y);

            lastDir = moveDir;

            if (moveDir == 0) // 0: UP
            {
                Debug.Log("coroutine dir: " + moveDir + " pos: " + transform.position);
                StartCoroutine(MoveGhost(Vector2.up));
                // StartCoroutine(MoveGhost(transform.position, scale * Vector2.up));
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", 1.0f);                
            }
            else if (moveDir == 1) // 1: RIGHT
            {
                Debug.Log("coroutine dir: " + moveDir + " pos: " + transform.position);
                StartCoroutine(MoveGhost(Vector2.right));
                // StartCoroutine(MoveGhost(transform.position, scale * Vector2.right));
                anim.SetFloat("DirX", 1.0f);
                anim.SetFloat("DirY", 0);
            }
            else if (moveDir == 2) // 2: DOWN
            {
                Debug.Log("coroutine dir: " + moveDir + " pos: " + transform.position);
                StartCoroutine(MoveGhost(Vector2.down));
                // StartCoroutine(MoveGhost(transform.position, scale * Vector2.down));
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", -1.0f);
            }
            else if (moveDir == 3) // 3: LEFT
            {
                Debug.Log("coroutine dir: " + moveDir + " pos: " + transform.position);
                StartCoroutine(MoveGhost(Vector2.left));
                // StartCoroutine(MoveGhost(transform.position, scale * Vector2.left));
                anim.SetFloat("DirX", -1.0f);
                anim.SetFloat("DirY", 0);
            }

        }


    }


    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "pacman") {
            Destroy(gameObject);    // pacman dies
        }


    }

/*
    protected IEnumerator MoveGhostNew2(Vector2 nextPos)
    {
        float i = 0.0f;
        float rate = 1 / moveDuration;
        Vector3 newPos = nextCell.transform.position;
        Vector3 startPos = transform.position;
        while (i < 1)
        {
            i += rate * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, newPos, i);
            yield return null;
        }
        transform.position = newPos;
        currentCell = nextCell;
        Move();
    }
*/
    
    private IEnumerator MoveGhost(Vector2 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        float threshold = 0.005f;
        float step;
        float speed = 0.01f;

        origPos = new Vector2(transform.position.x, transform.position.y);
        targetPos = origPos + scale * direction;

        step =  5f ; // calculate distance to move 

        while (elapsedTime < timeToMove)
        {
            if (Vector2.Distance(transform.position, targetPos) > threshold) {
                transform.position = Vector2.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                // elapsedTime += Time.deltaTime;
                elapsedTime += Time.smoothDeltaTime;
            } else {
                transform.position = targetPos;
            }
            yield return new WaitForFixedUpdate();

        }
      
//        Debug.Log("targetPos final:" + targetPos);
        // transform.position = targetPos;

        isMoving = false;

        yield return null;
    }


    private IEnumerator MoveGhostNew(Vector2 from, Vector2 move) {
            float speed = 1f;
            Vector2 to = new Vector2(from.x + move.x, from.y + move.y);
            float step = (speed / (from - to).magnitude) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f) {
                t += step; // Goes from 0 to 1, incrementing by step each time
                transform.position = Vector2.Lerp(from, to, t); // Move objectToMove closer to b
                yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
            }
            transform.position = to;
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
