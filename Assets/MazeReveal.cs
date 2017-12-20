using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeReveal : MonoBehaviour {

    public MazeGenerator mGen;
    public int renderDist = 5;
    public new Transform transform;
    List<MazeGenerator.Cell> loaded = new List<MazeGenerator.Cell>();
    Vector3 lastLoc;
    Vector3 curLoc;
    bool firstUpdate = true;
    bool partialRender = false;

	// Use this for initialization
	void Start () {
        if (mGen == false)
            mGen = GetComponent<MazeGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(renderDist <= 0)
        {
            renderDist = mGen.mazeSize / 2;
        }
        if (mGen.mazeSize <= renderDist * 2 )
            return; // no need to rerender the map (it is smaller than the render distance)

        if (mGen.mazeObject == null)
        {
            firstUpdate = true;
        }
        else if(firstUpdate)
        {
            FirstUpdate();
            firstUpdate = false;
        }
        else
        {
             curLoc = mGen.mazeObject.transform.InverseTransformPoint(transform.position);
            //curLoc = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            if (curLoc != lastLoc)
            {
                
                HideChunk();
                ShowChunk(mGen.mazeSize);
            }
        }

	}

    void FirstUpdate()
    {
        if (mGen.mazeSize <= renderDist * 2)
        {
            partialRender = false;
        }
        else
        {
            partialRender = true;
            mGen.HideAllCells();
            curLoc = mGen.mazeObject.transform.InverseTransformPoint(transform.position);
            ShowChunk(mGen.mazeSize);
        }
    }
    void ShowChunk(int mazeSize)
    {
        //print("Render next chunck");
        //curLoc = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        //print("x: " + (int)curLoc.x + " z: " + (int)curLoc.z);
        //print("Render| x: " + ((int)curLoc.x - renderDist) + "->" + (renderDist + (int)curLoc.x) + " z: " + ((int)curLoc.z - renderDist) + "->" + (renderDist + (int)curLoc.z));

        for (int x = (int)curLoc.x - renderDist; x < renderDist + (int)curLoc.x; x++)
        {
            if (x < 0 || x >= mazeSize)
                continue;
            for (int z = (int)curLoc.z - renderDist; z < renderDist + (int)curLoc.z; z++)
            {
                if (z < 0 || z >= mazeSize)
                    continue;
                

                loaded.Add(mGen.ShowCell(x, z));
            }
        }
        lastLoc = curLoc;
    }
    void HideChunk()
    {
        //print("Hide next chunck");
        for (int i = loaded.Count - 1; i >= 0; --i)
        {
            mGen.HideCell(loaded[i]);
            loaded.RemoveAt(i);
        }
    }
}
