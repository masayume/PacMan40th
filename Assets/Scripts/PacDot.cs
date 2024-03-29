﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour
{

    Camera m_MainCamera;

    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        // m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "Pacman") {

            // AudioSource.PlayClipAtPoint(clip, new Vector3(57f, 50f, -251f));
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            
            Destroy(gameObject);    // dot eaten

            Score.UpdateScore(10);  // score updated
        }


    }

}
