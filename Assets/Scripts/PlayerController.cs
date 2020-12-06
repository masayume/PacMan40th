using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private bool isMoving;
    private Vector2 origPos, targetPos;
    private float timeToMove = 0.2f;
    private float scale = 5f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        if (Input.GetKey(KeyCode.UpArrow) && !isMoving && valid(Vector2.up)) {
            StartCoroutine(MovePlayer(Vector2.up));
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", 1.0f);
        }
        if (Input.GetKey(KeyCode.DownArrow) && !isMoving && valid(Vector2.down)) {
            StartCoroutine(MovePlayer(Vector2.down));
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", -1.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !isMoving && valid(Vector2.right)) {
            StartCoroutine(MovePlayer(Vector2.right));
            anim.SetFloat("DirX", 1.0f);
            anim.SetFloat("DirY", 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && !isMoving && valid(Vector2.left)) {
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
        RaycastHit2D hit = Physics2D.Linecast(newPos, pos);
        Debug.Log(pos + " -> " + newPos + " " + hit.collider +  " " + (hit.collider == GetComponent<Collider2D>()));
        return (!(hit.collider == GetComponent<Collider2D>()));

        return true;
    }
}
