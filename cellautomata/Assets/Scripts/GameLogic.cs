using UnityEngine;

public class GameLogic : MonoBehaviour
{
    //public Automaton automaton_prefab;

    public uint automaton_id;
    public Automaton automaton;
    private GameObject play_btn_obj;
    private GameObject stop_btn_obj;
    private PlayButtonBehavior play_btn;
    private StopButtonBehavior stop_btn;
    private StepButtonBehavior step_btn;
    private ResetButtonBehavior reset_btn;

    // use this for initialization
    void Start()
    {
        // initialize automaton
        NewAutomaton();

        // get buttons
        play_btn_obj = GameObject.Find("Play Button");
        play_btn = play_btn_obj.GetComponent<PlayButtonBehavior>();
        stop_btn_obj = GameObject.Find("Stop Button");
        stop_btn = stop_btn_obj.GetComponent<StopButtonBehavior>();
        step_btn = GameObject.Find("Step Button").GetComponent<StepButtonBehavior>();
        reset_btn = GameObject.Find("Reset Button").GetComponent<ResetButtonBehavior>();

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
    }

    public void PreviousAutomaton()
    {
        --automaton_id;
        NewAutomaton();
        ResetButtons();
    }

    public void NextAutomaton()
    {
        ++automaton_id;
        NewAutomaton();
        ResetButtons();
    }

    private void NewAutomaton()
    {
        automaton.Initialize(automaton_id);
        automaton.name = "Automaton " + automaton_id;
    }
}
