using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour
{

    // private Text score;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "Pacman") {
            // audioSource.Play();
            // AudioSource.PlayOneShot(clip);
            AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0));
            Destroy(gameObject);    // dot eaten

            Score.UpdateScore(10);  // score updated
        }


    }

}
