using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Automaton automaton_prefab;

    private uint automaton_id;
    private Automaton current_automaton;

    // use this for initialization
    void Start()
    {
        // setup initial automaton
        current_automaton = null;
        automaton_id = 1;
        LoadAutomaton();
    }

    // called once per frame
    void Update()
    {

    }

    public void PreviousAutomaton()
    {
        --automaton_id;
        LoadAutomaton();
    }

    public void NextAutomaton()
    {
        ++automaton_id;
        LoadAutomaton();
    }

    private void LoadAutomaton()
    {
        // total kludge
        if (current_automaton != null) {
            GameObject obj = GameObject.Find("Automaton " + (automaton_id - 1));
            if (obj != null) {
                Destroy(obj);
            }
            obj = GameObject.Find("Automaton " + (automaton_id + 1));
            if (obj != null) {
                Destroy(obj);
            }
        }

        // instantiate new automaton
        Vector3 pos = new Vector3(0, 0, 0);
        current_automaton = Instantiate(automaton_prefab, pos, Quaternion.identity);
        current_automaton.Initialize(automaton_id);
        current_automaton.name = "Automaton " + automaton_id;

        // associate buttons with the new automaton
        PlayButtonBehavior play_btn = GameObject.Find("Play Button").GetComponent<PlayButtonBehavior>();
        StopButtonBehavior stop_btn = GameObject.Find("Stop Button").GetComponent<StopButtonBehavior>();
        ResetButtonBehavior reset_btn = GameObject.Find("Reset Button").GetComponent<ResetButtonBehavior>();
        StepButtonBehavior step_btn = GameObject.Find("Step Button").GetComponent<StepButtonBehavior>();
        play_btn.automaton = current_automaton;
        stop_btn.automaton = current_automaton;
        reset_btn.automaton = current_automaton;
        step_btn.automaton = current_automaton;
    }
}
