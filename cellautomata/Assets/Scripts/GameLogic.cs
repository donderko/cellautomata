﻿using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Automaton automaton;
    private GameObject play_btn_obj;
    private GameObject stop_btn_obj;
    private GameObject reset_btn_obj;
    private PlayButtonBehavior play_btn;
    private StopButtonBehavior stop_btn;
    private ResetButtonBehavior reset_btn;
    private StepButtonBehavior step_btn;

    public LevelButtonBehavior[] level_buttons;
    public uint current_level_index;

    // use this for initialization
    void Start()
    {
        // initialize automaton
        NewAutomaton();

        level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(true);

        // get buttons
        play_btn_obj = GameObject.Find("Play Button");
        play_btn = play_btn_obj.GetComponent<PlayButtonBehavior>();
        stop_btn_obj = GameObject.Find("Stop Button");
        stop_btn = stop_btn_obj.GetComponent<StopButtonBehavior>();
        reset_btn_obj = GameObject.Find("Reset Button");
        reset_btn = reset_btn_obj.GetComponent<ResetButtonBehavior>();
        step_btn = GameObject.Find("Step Button").GetComponent<StepButtonBehavior>();

        // associate buttons with the automaton
        play_btn.automaton = automaton;
        stop_btn.automaton = automaton;
        reset_btn.automaton = automaton;
        step_btn.automaton = automaton;

        ResetButtons();
    }

    private void ResetButtons()
    {
        stop_btn_obj.SetActive(false);
        play_btn_obj.SetActive(true);
        reset_btn_obj.GetComponent<SpriteRenderer>().sprite = reset_btn.clear_img;
    }

    // called once per frame
    void Update()
    {
        if (automaton.did_stop_action) {
            automaton.did_stop_action = false;
            stop_btn_obj.SetActive(false);
            play_btn_obj.SetActive(true);
        } else if (automaton.did_play_action) {
            automaton.did_play_action = false;
            play_btn_obj.SetActive(false);
            stop_btn_obj.SetActive(true);
        }

        if (automaton.state_saved) {
            reset_btn_obj.GetComponent<SpriteRenderer>().sprite = reset_btn.reinitialize_img;
        } else {
            reset_btn_obj.GetComponent<SpriteRenderer>().sprite = reset_btn.clear_img;
        }
    }

    public void PreviousAutomaton()
    {
        if (current_level_index > 0) {
            level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(false);
            --current_level_index;
            level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(true);
            NewAutomaton();
            ResetButtons();
        }
    }

    public void NextAutomaton()
    {
        if (current_level_index < level_buttons.GetLength(0) - 1) {
            level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(false);
            ++current_level_index;
            level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(true);
            NewAutomaton();
            ResetButtons();
        }
    }

    private void NewAutomaton()
    {
        automaton.Initialize(level_buttons[current_level_index].automaton_id, level_buttons[current_level_index]);
        automaton.name = "Automaton " + level_buttons[current_level_index].automaton_id;
    }

    public void LoadAutomaton(uint automaton_id)
    {
        level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(false);
        for (uint i = 0; i < level_buttons.GetLength(0); ++i) {
            if (level_buttons[i].automaton_id == automaton_id) {
                current_level_index = i;
                break;
            }
        }
        level_buttons[current_level_index].GetComponent<Pulse>().SetEnabled(true);
        NewAutomaton();
        ResetButtons();
    }
}
