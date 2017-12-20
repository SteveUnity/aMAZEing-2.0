using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MazeGenerator : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public GameObject mazeObject;
    public int mazeSize = 5;
    [Range(2, 6.5f)] public float mazeScale = 2f;
    [Range(2.5f, 6.5f)] public float mazeHeight = 2f;

    public int Seed { set { seed = value; randomSeed = false; } get { return seed; } }
    private int seed;
    private bool randomSeed = true;

    [Space(10)]
    public Material blueWall;
    public Material redWall;
    public Material yellowWall;

    List<List<Cell>> maze;
    //Cell[,] maze;
    List<Cell> visitedCellList = new List<Cell>();
    public enum HasNaghbor
    {
        edgeWall,
        hasNaghbor,
        visited
    }

    // Use this for initialization
    void Awake()
    {
        

        //maze = new Cell[mazeSize, mazeSize];
        seed = Random.Range(int.MinValue + 5, int.MaxValue - 5);

    }

    // Update is called once per frame
    void Update()
    {
        if (mazeObject != null)
        {
            if (mazeObject.transform.localScale.x != mazeScale || mazeObject.transform.localScale.y != mazeHeight)
                mazeObject.transform.localScale = new Vector3(mazeScale, mazeHeight, mazeScale);
        }
    }

    public void DestroyMaze()
    {
        if (mazeObject != null)
        {
            Destroy(mazeObject);
        }

    }

    public void BackWall(GameObject wallFront)
    {
        GameObject wallBack = Instantiate(wall, wallFront.transform);
        wallBack.transform.localPosition = Vector3.zero;
        wallBack.transform.localRotation = Quaternion.Euler(0, 180, 0);
        wallBack.GetComponent<Renderer>().material = yellowWall;
    }

    public void GenerateMaze()
    {
        DestroyMaze();
        mazeObject = new GameObject("Maze");
        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);
        maze = new List<List<Cell>>(mazeSize);
        for (int x = 0; x < mazeSize; x++)
        {
            maze.Add(new List<Cell>(mazeSize));
           
            for (int z = 0; z < mazeSize; z++)
            {
                //print(x + " " + z);
                
                    Cell tempCell = new Cell(mazeObject.transform, floor, wall, x, z);
                    
                //print("maze["+x+"] size= "+maze[x].Count);

                tempCell.floor.transform.position = new Vector3(x, 0, z);
                if (x == 0)
                {
                    tempCell.hasNaboghrCell[0] = HasNaghbor.edgeWall;
                    BackWall(tempCell.wall[0]);
                    tempCell.wall[0].GetComponent<Renderer>().material = redWall;

                    tempCell.hasNaboghrCell[2] = HasNaghbor.hasNaghbor;
                    tempCell.wall[2].GetComponent<Renderer>().material = blueWall;
                }
                else if (x == mazeSize - 1)
                {
                    tempCell.hasNaboghrCell[0] = HasNaghbor.hasNaghbor;
                   tempCell.wall[0].GetComponent<Renderer>().material = blueWall;

                    tempCell.hasNaboghrCell[2] = HasNaghbor.edgeWall;
                    BackWall(tempCell.wall[2]);
                    tempCell.wall[2].GetComponent<Renderer>().material = redWall;
                }
                else
                {
                    tempCell.hasNaboghrCell[0] = HasNaghbor.hasNaghbor;
                   tempCell.wall[0].GetComponent<Renderer>().material = blueWall;

                    tempCell.hasNaboghrCell[2] = HasNaghbor.hasNaghbor;
                   tempCell.wall[2].GetComponent<Renderer>().material = blueWall;
                }

                if (z == 0)
                {
                    tempCell.hasNaboghrCell[3] = HasNaghbor.edgeWall;
                    BackWall(tempCell.wall[3]);
                    tempCell.wall[3].GetComponent<Renderer>().material = redWall;

                    tempCell.hasNaboghrCell[1] = HasNaghbor.hasNaghbor;
                   tempCell.wall[1].GetComponent<Renderer>().material = blueWall;
                }
                else if (z == mazeSize - 1)
                {
                    tempCell.hasNaboghrCell[3] = HasNaghbor.hasNaghbor;
                   tempCell.wall[3].GetComponent<Renderer>().material = blueWall;

                    tempCell.hasNaboghrCell[1] = HasNaghbor.edgeWall;
                    BackWall(tempCell.wall[1]);
                    tempCell.wall[1].GetComponent<Renderer>().material = redWall;
                }
                else
                {
                    tempCell.hasNaboghrCell[3] = HasNaghbor.hasNaghbor;
                    tempCell.wall[3].GetComponent<Renderer>().material = blueWall;

                    tempCell.hasNaboghrCell[1] = HasNaghbor.hasNaghbor;
                    tempCell.wall[1].GetComponent<Renderer>().material = blueWall;
                }
                maze[x].Add(tempCell);
            }
        }
        OpenDoor();
        print("after OpenDoor");
        mazeObject.transform.localScale = new Vector3(mazeScale, mazeHeight, mazeScale);
    }



    void OpenDoor()
    {
        int rx = Random.Range(0, mazeSize - 1);
        int rz = Random.Range(0, mazeSize - 1);
        Cell cell = maze[rx][rz];
        cell.floor.GetComponent<Renderer>().material = blueWall;
        Cell na;

        cell.visited = true;
        visitedCellList.Add(cell);
        while (visitedCellList.Count > 0)
        {
            nextCell:
            int dir = Random.Range(0, 3);

            for (int i = 0; i < 4; i++)
            {
                //print("Cell: " + rx + "," + rz + " | wall: " + dir + " | i= " + i);
                if (cell.hasNaboghrCell[dir] == HasNaghbor.hasNaghbor)
                {
                    //print("has naghbor");
                    switch (dir)
                    {
                        case 0:
                            na = maze[rx - 1][rz];
                            if (na.visited)
                            {
                                cell.hasNaboghrCell[0] = HasNaghbor.visited;
                                dir += 1;
                                if (dir == 4)
                                    dir = 0;
                                continue;
                            }
                            DestroyWall(na, 2);
                            na.hasNaboghrCell[2] = HasNaghbor.visited;
                            break;
                        case 1:
                            na = maze[rx][rz + 1];
                            if (na.visited)
                            {
                                cell.hasNaboghrCell[dir] = HasNaghbor.visited;
                                dir += 1;
                                if (dir == 4)
                                    dir = 0;
                                continue;
                            }
                            DestroyWall(na, 3);
                            na.hasNaboghrCell[3] = HasNaghbor.visited;
                            break;
                        case 2:
                            na = maze[rx + 1][rz];
                            if (na.visited)
                            {
                                cell.hasNaboghrCell[dir] = HasNaghbor.visited;
                                dir += 1;
                                if (dir == 4)
                                    dir = 0;
                                continue;
                            }
                            DestroyWall(na, 0);
                            na.hasNaboghrCell[0] = HasNaghbor.visited;
                            break;
                        case 3:
                            na = maze[rx][rz - 1];
                            if (na.visited)
                            {
                                cell.hasNaboghrCell[dir] = HasNaghbor.visited;
                                dir += 1;
                                if (dir == 4)
                                    dir = 0;
                                continue;
                            }
                            DestroyWall(na, 1);
                            na.hasNaboghrCell[1] = HasNaghbor.visited;
                            break;
                        default:
                            na = maze[rx + 1][rz];
                            break;
                    }
                    DestroyWall(cell, dir);

                    cell = na;
                    cell.floor.GetComponent<Renderer>().material = redWall;
                    cell.visited = true;
                    visitedCellList.Add(cell);
                    rx = (int)cell.coor.x;
                    rz = (int)cell.coor.y;
                    goto nextCell;
                }
                else
                {

                    if (cell.hasNaboghrCell[dir] != HasNaghbor.visited)
                    {
                        cell.wall[dir].GetComponent<Renderer>().material = yellowWall;
                        //print("edge Wall");
                    }
                    else
                        ;//print("Visited");
                }
                dir += 1;
                if (dir == 4)
                    dir = 0;
            }

            visitedCellList.Remove(cell);
            if (visitedCellList.Count > 0)
            {
                cell = visitedCellList[visitedCellList.Count - 1];
                rx = (int)cell.coor.x;
                rz = (int)cell.coor.y;
                //print("cell Complete, removing from visited");
            }
        }

    }

    void DestroyWall(Cell cell, int dir)
    {
        GameObject go = cell.wall[dir];
        cell.wall[dir] = new GameObject(go.name);
        cell.wall[dir].transform.parent = cell.floor.transform;
        Destroy(go);
    }

    public void HideAllCells()
    {

        for (int x = 0; x < mazeSize; x++)
        {
            for (int z = 0; z < mazeSize; z++)
            {
                HideCell(x, z);
            }
        }
    }

    public Cell HideCell(int x, int z)
    {
        if (maze[x][z].floor.activeSelf)
        {
            maze[x][z].floor.SetActive(false);
            return maze[x][z];
        }
        return null;
    }
    public void HideCell(Cell cell)
    {
        HideCell(cell.coor.x, cell.coor.y);
    }

    public Cell ShowCell(int x, int z)
    {
        if (maze[x][z].floor.activeSelf)
        {
            return null;
        }
        maze[x][z].floor.SetActive(true);
        return maze[x][z];
    }

    public void SetSeed(string input)
    {
        seed = int.Parse(input);
    }

    public class Cell
    {
        public GameObject floor;
        public GameObject[] wall = new GameObject[4];
        public HasNaghbor[] hasNaboghrCell = new HasNaghbor[4];
        public bool visited = false;
        public Vector2Int coor;
        public Cell(Transform parent, GameObject floor, GameObject wall, int x, int z)
        {
            coor = new Vector2Int(x, z);
            this.floor = Instantiate(floor, parent);
            this.floor.name = "Cell " + x + " " + z;

            this.wall[0] = Instantiate(wall, new Vector3(-0.5f, 0f, 0f), Quaternion.Euler(0, 180, 0), this.floor.transform);
            this.wall[0].name = "wall " + 0;

            this.wall[1] = Instantiate(wall, new Vector3(0f, 0f, 0.5f), Quaternion.Euler(0, 270, 0), this.floor.transform);
            this.wall[1].name = "wall " + 1;

            this.wall[2] = Instantiate(wall, new Vector3(0.5f, 0f, 0f), Quaternion.Euler(0, 0, 0), this.floor.transform);
            this.wall[2].name = "wall " + 2;

            this.wall[3] = Instantiate(wall, new Vector3(0f, 0f, -0.5f), Quaternion.Euler(0, 90, 0), this.floor.transform);
            this.wall[3].name = "wall " + 3;
        }
    }
}

