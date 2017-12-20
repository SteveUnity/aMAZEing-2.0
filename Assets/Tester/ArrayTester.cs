using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTester : MonoBehaviour {


    // Use this for initialization
    void Start () {
        List<List<int>> lists = new List<List<int>>(5);

        for (int i = 0; i < lists.Count; i++)
        {
            lists[i] = new List<int>(5);
        }


        lists[2][3] = 8;
        print(lists[2][3]);
        print(lists[2][2]);



    }
	
	// Update is called once per frame
	void Update () {
        


    }

    
}
