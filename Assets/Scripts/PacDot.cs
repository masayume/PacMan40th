using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour
{

    // private Text score;

    // Start is called before the first frame update
    void Start()
    {
        // score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "pacman") {
            Destroy(gameObject);    // dot eaten
            Score.UpdateScore(10);  // score updated
        }


    }

}
