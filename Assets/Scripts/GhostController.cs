using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform[] waypoints;
    int cur = 0;
    public float speed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Animator>().SetFloat("DirX", 0);
        // GetComponent<Animator>().SetFloat("DirY", 0);
    }

    // use the FixedUpdate function to go closer to the current waypoint, 
    // or select the next one as soon as we reached it
    void FixedUpdate()
    {
        if (transform.position != waypoints[cur].position) {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else {
            cur = (cur + 1) % waypoints.Length;
        }

        // Animation
        Vector2 dir = waypoints[cur].position - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }


    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "pacman") {
            Destroy(gameObject);    // pacman dies
        }


    }

}
