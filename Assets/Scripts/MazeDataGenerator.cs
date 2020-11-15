/*
 * written by Joseph Hocking 2017
 * released under MIT license
 * text of license https://opensource.org/licenses/MIT
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{    
    // generator params
    public float placementThreshold;    // chance of empty space

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];

        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        // Debug.Log("rMax, cMax: " + rMax + ", " + cMax);

        int c05 = (int)(cMax/2);

        for (int i = 0; i <= rMax; i++) // rows
        {
            for (int j = 0; j <= cMax; j++) // cols
            {
                // outside wall
                if (i == 0 || j == 0 || i == rMax || j == cMax) { maze[i, j] = 1; }

                // every other inside space
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        maze[i, j] = 1;

                        // in addition to this spot, randomly place adjacent
                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i+a, j+b] = 1;
                    }
                }
            }
        }

        // re-edit right half of the maze to be symmetric
        for (int i = 1; i < rMax; i++) // rows
        {
            for (int j = c05+1; j < cMax; j++) // cols
            {

                Debug.Log(" i,j: " + i + "," + j + " = " + i + "," + (j-(c05-j)));
                maze[i, j] = maze[i, cMax-j];
            }
        }
        return maze;
    }
}
