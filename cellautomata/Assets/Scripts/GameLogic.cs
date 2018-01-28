using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Automaton automaton_prefab;

    private uint automaton_id;
    private Automaton current_automaton;
    private GameObject play_btn_obj;
    private GameObject stop_btn_obj;
    private PlayButtonBehavior play_btn;
    private StopButtonBehavior stop_btn;
    private StepButtonBehavior step_btn;
    private ResetButtonBehavior reset_btn;

    // use this for initialization
    void Start()
    {
        // get buttons
        play_btn_obj = GameObject.Find("Play Button");
        play_btn = play_btn_obj.GetComponent<PlayButtonBehavior>();
        stop_btn_obj = GameObject.Find("Stop Button");
        stop_btn = stop_btn_obj.GetComponent<StopButtonBehavior>();
        step_btn = GameObject.Find("Step Button").GetComponent<StepButtonBehavior>();
        reset_btn = GameObject.Find("Reset Button").GetComponent<ResetButtonBehavior>();

        // setup initial automaton
        current_automaton = null;
        automaton_id = 1;
        LoadAutomaton();

        // button setup
        current_automaton.did_play_action = false;
        current_automaton.did_stop_action = false;
        stop_btn_obj.SetActive(false);
        play_btn_obj.SetActive(true);
    }

    // called once per frame
    void Update()
    {
        if (current_automaton.did_stop_action) {
            current_automaton.did_stop_action = false;
            stop_btn_obj.SetActive(false);
            play_btn_obj.SetActive(true);
        } else if (current_automaton.did_play_action) {
            current_automaton.did_play_action = false;
            play_btn_obj.SetActive(false);
            stop_btn_obj.SetActive(true);
        }
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
        play_btn.automaton = current_automaton;
        stop_btn.automaton = current_automaton;
        reset_btn.automaton = current_automaton;
        step_btn.automaton = current_automaton;
    }
}
