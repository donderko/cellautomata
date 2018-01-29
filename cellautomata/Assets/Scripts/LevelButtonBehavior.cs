using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonBehavior : MonoBehaviour
{

    public Sprite unsolved_img;
    public Sprite solved_img;
    public uint automaton_id;
    public GameLogic game_logic;

	// use this for initialization
	void Start()
    {
		
	}

    // called once per frame
    void Update()
    {
		
	}

    private void OnMouseDown()
    {
        game_logic.LoadAutomaton(automaton_id);
    }

    public void SetSolved()
    {
        GetComponent<SpriteRenderer>().sprite = solved_img;
    }
}
