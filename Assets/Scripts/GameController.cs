/*
 * written by Joseph Hocking 2017
 * released under MIT license
 * text of license https://opensource.org/licenses/MIT
 */

/*
 * scatter-chase sequences:
 * level 1:     7" 20" 7" 20" 5" 20" 5" chase
 * level 2-4:   7" 20" 7" 20" 5" 13" 1" chase
 * level 5+:    5" 20" 5" 20" 5" 17" 1" chase
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    private DateTime startTime;
    private int timeLimit;
    private int reduceLimitBy;

    private int score;
    // private bool isToggling = false, ToggleOnOff;

    // Use this for initialization
    void Start() {
        generator = GetComponent<MazeConstructor>();
        StartNewGame();
    }

    private void StartNewGame()
    {
        /*
        timeLimit = 80;
        reduceLimitBy = 5;
        startTime = DateTime.Now;
        */
        score = 0;
        // scoreLabel.text = score.ToString();

        StartNewMaze();
    }

    private void StartNewMaze()
    {
        generator.GenerateNewMaze(25, 21, OnStartTrigger, OnGoalTrigger);

        float x = generator.startCol * generator.hallWidth;
        // float y = 1;
        float z = generator.startRow * generator.hallWidth;

        // restart timer
        /*
        timeLimit -= reduceLimitBy;
        startTime = DateTime.Now;
        */
    }

    // Update is called once per frame
    void Update()
    {
        // ghostState = 1; // start in chase mode = Ghost State: [0=frightened, 1=chase, 2=scatter, 3=eaten]
/*
        if (!isToggling){
            Toggle();
        }
*/ 
            // timeLabel.text = "TIME UP";
            // player.enabled = false;

            // Invoke("StartNewGame", 4);
 
    }

    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Goal!");
        // goalReached = true;

        score += 1;
        // scoreLabel.text = score.ToString();

        Destroy(trigger);
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
/*
        if (goalReached)
        {
            Debug.Log("Finish!");
            // player.enabled = false;

            Invoke("StartNewMaze", 4);
        }
*/

    }

}
