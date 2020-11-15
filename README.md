# PacMan40th
40th anniversary pacman in unity 2019

![40th anniversary pacman in unity 2019](img/Screenshot_20201114.png)

# PacMan 40th anniversary

## INFO

- local directory: ~/DATA/E/UNITY.PROJECTS/2020-1-PACMAN/PacMan40th/
- PSD directory: ~/DATA/E/UNITY.PROJECTS/2020-1-PACMAN/PacMan40th/img

## TODO

- trello ?
 
- seed for random
- pacman sprite
- test collider con player
- fix del layout del labirinto alla pacman
- fix della topologia del labirinto (no vicoli ciechi e simmetria sull'asse verticale/orizz)
- implement. dei tunnel
- ghosts

## CODE

- building the maze:
```
	StartNewMaze() of GameController.cs
	FromDimensions(int sizeRows, int sizeCols)of MazeDataGenerator.cs
	DisplayMaze() of MazeConstructor.cs
```
- rendering the maze:
```
     RenderFromData(int[,] data) of MazeRenderer.cs
```

## PATHs & URLS

* [PacMan 40th anniversary git](git@github.com:masayume/PacMan40th.git)
* [PacMan 40th anniversary URL](https://github.com/masayume/PacMan40th)
* []()
* []()
* []()
* []()
* []()


## DONE
- gestione della simmetria centrale
