using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    public CellBoard cell_board_prefab;

    // use this for initialization
    void Start()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        CellBoard cell_board = Instantiate(cell_board_prefab, pos, Quaternion.identity);
        cell_board.Initialize(15, 10);
        cell_board.name = "Cell Board";
    }

    // called once per frame
    void Update ()
    {

	}
}
