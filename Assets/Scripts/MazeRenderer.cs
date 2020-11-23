using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{

    public SpriteRenderer wall;
    public SpriteRenderer[] walls;
    public SpriteRenderer floor;
    public SpriteRenderer dots;

    // folder gameobjects for prefabs
    public Transform maze;
    public Transform mazedots; 

    // int size = 10;
    public void FillMazeWithDots(int[,] data)
    {
        Debug.Log("Filling the maze");
        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);
        int step = 5;

        for (int i = 0; i <= rMax; i++) // rows 
        {
            for (int j = 0; j <= cMax; j++) // columns
            {
                if (data[i, j] == 0)
                {
                    SpriteRenderer sp = Instantiate<SpriteRenderer>(dots, mazedots);
                    int x = j * step;    
                    int y = i * step;   
                    sp.transform.position = new Vector3(x, y, 0);
                }
            }
        }
    }

    public void RenderFromData(int[,] data)
    {
        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);
        int step = 5;
        // 
        for (int i = 0; i <= rMax; i++) // rows 
        {
            for (int j = 0; j <= cMax; j++) // columns
            {
                if (data[i, j] == 0)
                {
                    // creiamo un nuovo spriterenderer da posizionare nello spazio
                    SpriteRenderer sp = Instantiate<SpriteRenderer>(floor, maze);
                    int x = j * step;    
                    int y = i * step;   
                    sp.transform.position = new Vector3(x, y, 0);

                } else {
                    SpriteRenderer sp = Instantiate<SpriteRenderer>(walls[data[i, j]], maze);
                    int x = j * step;    
                    int y = i * step;   
                    sp.transform.position = new Vector3(x, y, 0);
                    sp.name = "wall";

                }
            }
        }
        
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
