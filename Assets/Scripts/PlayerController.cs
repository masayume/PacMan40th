using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private bool isMoving;
    private Vector2 origPos, targetPos;
    private float timeToMove = 0.2f;
    private float scale = 5f;
    public Animator anim;
    public LayerMask moveBlockLayerMask;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        if (!isMoving && (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))) {
            StartCoroutine(MovePlayer(Vector2.up));
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", 1.0f);
        }
        if (!isMoving && (Input.GetKey(KeyCode.DownArrow) && valid(Vector2.down))) {
            StartCoroutine(MovePlayer(Vector2.down));
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", -1.0f);
        }
        if (!isMoving && (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))) {
            StartCoroutine(MovePlayer(Vector2.right));
            anim.SetFloat("DirX", 1.0f);
            anim.SetFloat("DirY", 0);
        }
        if (!isMoving && (Input.GetKey(KeyCode.LeftArrow) && valid(Vector2.left))) {
            StartCoroutine(MovePlayer(Vector2.left));
            anim.SetFloat("DirX", -1.0f);
            anim.SetFloat("DirY", 0);
        }


    }

    private IEnumerator MovePlayer(Vector2 direction)
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


    bool valid(Vector2 dir) {
    // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPos =  pos + dir*scale;
        RaycastHit2D hit = Physics2D.Linecast(pos, newPos, moveBlockLayerMask);
        
//        Debug.Log("from: " + pos + " to: " + newPos + " hit: " + hit.collider +  " is " + (hit.collider == true));
//        Debug.DrawRay(pos, newPos, Color.magenta);

        return (!(hit.collider == true));

        return true;
    }
}
