using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // public Transform[] waypoints;
    int actionCtr = 0;
    int lastDir = 0;            // direction of the last movement (0: up, 1: right, 2: down, 3: left)

    [Header("ghost speed")]
    public float timeToMove = 0.4f;
    // private float speed = 0.8f;

    Animator anim;

    [Header("targets")]
    public Transform scatterTarget;
    public Transform chaseTarget;
    public Transform nextTile;
    private GameObject pacman, blinky;
    public static int ghostState = 1; // start in chase mode = Ghost State: [0=frightened, 1=chase, 2=scatter, 3=eaten]

    private bool isMoving; 
    private Vector2 origPos, targetPos, destPos; // actor position; next move position; destination position (scatter/chase)
    private static float scale = 5f;
    private float chaseScale = 4f * scale;

    [Header("collider mask")]
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
        pacman = GameObject.Find("Pacman");
        blinky = GameObject.Find("Ghost1"); // Red Ghost

        // coroutines to switch state
        StartCoroutine(ToggleStateCoroutine());
    }

    // ghostState = 1; // start in chase mode = Ghost State: [0=frightened, 1=chase, 2=scatter, 3=eaten]
    IEnumerator ToggleStateCoroutine()
    {
        while (true)
        {
            if (ghostState == 1) {
                ghostState = 2;
                // Debug.Log(this.gameObject.name + " scatters.");
            } else if (ghostState == 2) {
                ghostState = 1;
                // Debug.Log(this.gameObject.name + " chasing.");
            }
            yield return new WaitForSeconds(7f); // wait for 7 seconds and then go on

        }
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
            
            MoveGhostTargets();
            
            actionCtr++;
            float maxDist = 100000f;
            float bestDist = 100000f;
            int moveDir = -1;

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

            // Debug.Log("act: " + actionCtr + " best: " + moveDir + "=== origPos:" + origPos + " Pos:" + transform.position.x + "," + transform.position.y);

            lastDir = moveDir;

            if (moveDir == 0) // 0: UP
            {
                StartCoroutine(MoveGhost(Vector2.up));
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", 1.0f);                
            }
            else if (moveDir == 1) // 1: RIGHT
            {
                StartCoroutine(MoveGhost(Vector2.right));
                anim.SetFloat("DirX", 1.0f);
                anim.SetFloat("DirY", 0);
            }
            else if (moveDir == 2) // 2: DOWN
            {
                StartCoroutine(MoveGhost(Vector2.down));
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", -1.0f);
            }
            else if (moveDir == 3) // 3: LEFT
            {
                StartCoroutine(MoveGhost(Vector2.left));
                anim.SetFloat("DirX", -1.0f);
                anim.SetFloat("DirY", 0);
            }

        }


    }


    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "Pacman") {
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

    // Blinky
    public void MoveRedChaseTarget()
    {
        // find pacman position
        chaseTarget.transform.position = new Vector3(pacman.transform.position.x, pacman.transform.position.y, 0);
    }

    // Pinky
    public void MovePinkChaseTarget()
    {
        // find pacman position and direction: get DirX, DirY values from Pacman animator
        chaseTarget.transform.position = new Vector3(
            pacman.transform.position.x + pacman.GetComponent<Animator>().GetFloat("DirX") * chaseScale, 
            pacman.transform.position.y + pacman.GetComponent<Animator>().GetFloat("DirY") * chaseScale, 
        0);

        // Debug.Log("moving pink target: " + chaseTarget.transform.position);

    }

    // Inky
    public void MoveCyanChaseTarget()
    {
        // find pacman position and direction: get DirX, DirY values from Pacman animator
        // find Blinky (Red) position 
        Vector3 blinkyPos, v2, v1;

        Vector3 p0 = new Vector3(
            pacman.transform.position.x + pacman.GetComponent<Animator>().GetFloat("DirX") * (chaseScale / 2), 
            pacman.transform.position.y + pacman.GetComponent<Animator>().GetFloat("DirY") * (chaseScale / 2), 
        0);

        if (blinky != null) // blinky is not destroyed
        {
            blinkyPos = new Vector3(blinky.transform.position.x, blinky.transform.position.y, 0);
            v1 = blinkyPos - p0;
            v2 = -v1;
        } else {
            v2 = new Vector3(0, 0, 0);
        }

        chaseTarget.transform.position = new Vector3(pacman.transform.position.x + v2.x, pacman.transform.position.y + v2.y, 0);    
    }

// updating chase target position
    private void MoveGhostTargets() {

        // Debug.Log("must move " + this.gameObject.name );

        if (this.gameObject.name == "Ghost1") // Red Ghost target is where pacman is located
        {
            // set new chaseTarget position
            MoveRedChaseTarget();

        } else if (this.gameObject.name == "Ghost2") // Pink Ghost target is 4+ units in front of pacman
        {
            // update chaseTarget (PinkWPChase)
            MovePinkChaseTarget();

        } else if (this.gameObject.name == "Ghost3") // Cyan Ghost target a function of pacman and Red Ghost Position
        {
            // update chaseTarget (CyanWPChase)
            MoveCyanChaseTarget();

        }


    }
}
