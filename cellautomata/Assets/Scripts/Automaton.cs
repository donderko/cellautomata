using System;
using UnityEngine;

public class Automaton : MonoBehaviour
{
    public Cell cell_prefab;
    public bool did_play_action;
    public bool did_stop_action;
    public bool state_saved;

    // audio
    public AudioManagerBehavior audio_manager;

    // puzzle-specific
    private uint x_size = 0;
    private uint y_size = 0;
    private Action InitAutomaton;
    private Func<bool> VictoryCondition;
    private LevelButtonBehavior level_button;

    // general
    private bool running;
    private bool victory;
    private bool first_run;
    private Cell[,] cells;
    private bool[,] next_states;
    private bool[,] saved_states;
    private uint[] row_alive_counts;
    private uint[] column_alive_counts;

    void Start()
    {

    }

    // use this for initialization
    public void Initialize(uint automaton_id, LevelButtonBehavior level_button)
    {
        CancelInvoke();
        DestroyCurrentCells();

        did_play_action = false;
        did_stop_action = false;
        state_saved = false;

        running = false;
        victory = false;
        first_run = true;

        this.level_button = level_button;

        // select automaton
        switch (automaton_id) {
            case 1:
                x_size = x_size_1;
                y_size = y_size_1;
                InitAutomaton = AutomatonInit1;
                VictoryCondition = VictoryCondition1;
                break;
            case 2:
                x_size = x_size_2;
                y_size = y_size_2;
                InitAutomaton = AutomatonInit2;
                VictoryCondition = VictoryCondition2;
                break;
            case 3:
                x_size = x_size_3;
                y_size = y_size_3;
                InitAutomaton = AutomatonInit3;
                VictoryCondition = VictoryCondition3;
                break;
            case 4:
                x_size = x_size_4;
                y_size = y_size_4;
                InitAutomaton = AutomatonInit4;
                VictoryCondition = VictoryCondition4;
                break;
            case 5:
                x_size = x_size_5;
                y_size = y_size_5;
                InitAutomaton = AutomatonInit5;
                VictoryCondition = VictoryCondition5;
                break;
            case 6:
                x_size = x_size_6;
                y_size = y_size_6;
                InitAutomaton = AutomatonInit6;
                VictoryCondition = VictoryCondition6;
                break;
            case 7:
                x_size = x_size_7;
                y_size = y_size_7;
                InitAutomaton = AutomatonInit7;
                VictoryCondition = VictoryCondition7;
                break;
            case 8:
                x_size = x_size_8;
                y_size = y_size_8;
                InitAutomaton = AutomatonInit8;
                VictoryCondition = VictoryCondition8;
                break;
            case 9:
                x_size = x_size_9;
                y_size = y_size_9;
                InitAutomaton = AutomatonInit9;
                VictoryCondition = VictoryCondition9;
                break;
            case 10:
                x_size = x_size_10;
                y_size = y_size_10;
                InitAutomaton = AutomatonInit10;
                VictoryCondition = VictoryCondition10;
                break;
            case 11:
                x_size = x_size_11;
                y_size = y_size_11;
                InitAutomaton = AutomatonInit11;
                VictoryCondition = VictoryCondition11;
                break;
            default:
                x_size = x_size_default;
                y_size = y_size_default;
                InitAutomaton = AutomatonInitDefault;
                VictoryCondition = VictoryConditionDefault;
                break;
        }

        // init automaton
        InstantiateCells();
        ResetAutomaton();
    }

    private void DestroyCurrentCells()
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                Destroy(cells[x, y].transform.gameObject);
            }
        }
    }

    // called once per frame
    // do all of the complicated, state-dependent logic here and in the ButtonAction functions (keep it out of the helper functions)
    void Update()
    {
        // pause or resume automata
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (running) {
                StopButtonAction();
            } else { // paused
                PlayButtonAction();
            }
        }

        // reset automata
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetButtonAction();
        }
    }

    // do this when the play button is pressed
    public bool PlayButtonAction()
    {
        if (!running && !victory) {
            running = true;
            if (first_run) {
                first_run = false;
                SaveStates();
            }
            RunAutomaton();
            return true;
        }
        return false;
    }

    // do this when the stop button is pressed
    public bool StopButtonAction()
    {
        if (running) {
            running = false;
            PauseAutomaton();
            return true;
        }
        return false;
    }

    // do this when the reset button is pressed
    public void ResetButtonAction()
    {
        audio_manager.PlayResetSound();

        victory = false;
        state_saved = false;

        if (running) {
            running = false;
            first_run = true;
            PauseAutomaton();
            ResetAutomaton();
            LoadStates();
        } else { // paused
            ResetAutomaton();
            if (!first_run) {
                LoadStates();
            }
            first_run = true;
        }
    }

    // do this when the step button is pressed
    public void StepButtonAction()
    {
        if (!running && !victory) {
            if (first_run) {
                first_run = false;
                SaveStates();
            }
            TickCells();
        }
    }

    // re-initialize the automaton
    private void ResetAutomaton()
    {
        // init all cells to be dead, editable, clickable, and non-targets
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetAlive(false);
                cells[x, y].SetEditable(true);
                cells[x, y].SetClickable(true);
                cells[x, y].SetTarget(false);
            }
        }

        // init automaton for this specific puzzle
        InitAutomaton();
    }

    // pause the automaton
    private void PauseAutomaton()
    {
        did_stop_action = true;
        running = false;
        audio_manager.PlayStopSound();
        CancelInvoke();
    }

    // run the automaton
    private void RunAutomaton()
    {
        did_play_action = true;
        audio_manager.PlayPlaySound();
        SetAllClickable(false);
        InvokeRepeating("TickCells", 0, 0.2f);
    }

    // stores the alive/dead states of all cells
    private void SaveStates()
    {
        state_saved = true;
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                saved_states[x, y] = cells[x, y].IsAlive();
            }
        }
    }

    // restore the automaton to a saved state
    private void LoadStates()
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetAlive(saved_states[x, y]);
            }
        }
    }

    // set all cells to be clickable or not
    private void SetAllClickable(bool clickable)
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetClickable(clickable);
            }
        }
    }

    // set all cells to be editable or not
    private void SetAllEditable(bool editable)
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetEditable(editable);
            }
        }
    }

    // instantiate the cells in a grid
    private void InstantiateCells()
    {
        // init arrays
        cells = new Cell[x_size, y_size];
        next_states = new bool[x_size, y_size];
        saved_states = new bool[x_size, y_size];
        row_alive_counts = new uint[y_size];
        column_alive_counts = new uint[x_size];

        // compute offsets so that the grid is centered on the screen
        float x_offset = (x_size - 1) / 2 + (x_size % 2 == 0 ? 0.5f : 0.0f); // magic
        float y_offset = (y_size - 1) / 2 + (y_size % 2 == 0 ? 0.5f : 0.0f); // magic

        // for each cell
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                // instantiate the cell
                Vector3 pos = new Vector3(x - x_offset, y - y_offset, 0) * 1;
                cells[x, y] = Instantiate(cell_prefab, pos, Quaternion.identity) as Cell;
                cells[x, y].transform.SetParent(this.transform);
                cells[x, y].audio_manager = audio_manager;
                cells[x, y].name = "Cell (" + x + "," + y + ")";
            }
        }
    }

    // update all cells one time tick
    private void TickCells()
    {
        SetAllEditable(false);

        // compute next states
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                if (cells[x, y].IsEmpty()) {
                    continue;
                }
                uint alive_neighbor_count = AliveNeighborCount(x, y);
                if (cells[x, y].IsAlive()) {
                    if (alive_neighbor_count < 2 || alive_neighbor_count > 3) {
                        next_states[x, y] = false;
                    } else {
                        next_states[x, y] = true;
                    }
                } else { // the cell is dead
                    if (alive_neighbor_count == 3) {
                        next_states[x, y] = true;
                    } else {
                        next_states[x, y] = false;
                    }
                }
            }
        }

        // apply next states
        for (uint x = 0; x < x_size; ++x) {
            column_alive_counts[x] = 0;
        }
        for (uint y = 0; y < y_size; ++y) {
            row_alive_counts[y] = 0;
        }
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                if (next_states[x, y]) {
                    cells[x, y].SetAlive(true);
                    ++row_alive_counts[y];
                    ++column_alive_counts[x];
                } else {
                    cells[x, y].SetAlive(false);
                }
            }
        }

        // play sound
        audio_manager.PlayAutomatonSound(row_alive_counts, column_alive_counts);

        if (VictoryCondition()) {
            DoVictory();
            audio_manager.PlayVictorySound();
        }
    }

    // returns the number of neighbors of cell (x, y) that are alive
    private uint AliveNeighborCount(uint x, uint y)
    {
        uint count = 0;

        // manhattan
        if (x > 0 && cells[x - 1, y].IsAlive()) { ++count; }
        if (y > 0 && cells[x, y - 1].IsAlive()) { ++count; }
        if (x < (x_size - 1) && cells[x + 1, y].IsAlive()) { ++count; }
        if (y < (y_size - 1) && cells[x, y + 1].IsAlive()) { ++count; }

        // diagonal
        if (x > 0 && y > 0 && cells[x - 1, y - 1].IsAlive()) { ++count; }
        if (x < (x_size - 1) && y < (y_size - 1) && cells[x + 1, y + 1].IsAlive()) { ++count; }
        if (x > 0 && y < (y_size - 1) && cells[x - 1, y + 1].IsAlive()) { ++count; }
        if (x < (x_size - 1) && y > 0 && cells[x + 1, y - 1].IsAlive()) { ++count; }

        return count;
    }

    private void DoVictory()
    {
        victory = true;
        level_button.SetSolved();
        PauseAutomaton();
    }

    /////////////////////////////////////////////////
    /// PUZZLES
    /// TODO: move these into their own class/file
    /////////////////////////////////////////////////

    uint x_size_1 = 3;
    uint y_size_1 = 3;

    private void AutomatonInit1()
    {
        //cells[3, 0].SetEditable(false);
        cells[1, 0].SetTarget(true);
        cells[1, 2].SetTarget(true);
    }

    private bool VictoryCondition1()
    {
        return cells[1, 2].IsAlive() || cells[1, 0].IsAlive();
    }

    uint x_size_2 = 5;
    uint y_size_2 = 3;

    private void AutomatonInit2()
    {
        for (uint x = 3; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetAlive(false);
                cells[x, y].SetEditable(false);
            }
        }
        
        cells[4, 0].SetTarget(true);
        cells[4, 1].SetTarget(true);
        cells[4, 2].SetTarget(true);
    }

    private bool VictoryCondition2()
    {
        if (cells[4, 0].IsAlive() || cells[4, 1].IsAlive() || cells[4, 2].IsAlive()) {
            return true;
        }
        return false;
    }

    uint x_size_3 = 9;
    uint y_size_3 = 3;

    private void AutomatonInit3()
    {
        cells[3, 0].SetEditable(false);
        cells[3, 1].SetEditable(false);
        cells[3, 2].SetEmpty(true);
        cells[4, 0].SetEditable(false);
        cells[4, 1].SetEditable(false);
        cells[4, 2].SetTarget(true);
        cells[5, 0].SetEditable(false);
        cells[5, 1].SetEditable(false);
        cells[5, 2].SetEmpty(true);
    }

    private bool VictoryCondition3()
    {
        return cells[4, 2].IsAlive();
    }

    uint x_size_4 = 9;
    uint y_size_4 = 9;

    private void AutomatonInit4()
    {

        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetAlive(false);
                cells[x, y].SetEditable(false);
            }
        }

        for (uint x = 3; x < 6; ++x) {
            for (uint y = 3; y < 6; ++y) {
                cells[x, y].SetEditable(true);
            }
        }

        cells[8, 4].SetTarget(true);
        cells[4, 8].SetTarget(true);
        cells[4, 0].SetTarget(true);
        cells[0, 4].SetTarget(true);
    }

    private bool VictoryCondition4()
    {
        bool alive1 = cells[8, 4].IsAlive();
        bool alive2 = cells[4, 8].IsAlive();
        bool alive3 = cells[4, 0].IsAlive();
        bool alive4 = cells[0, 4].IsAlive();

        return (alive1 || alive2 || alive3 || alive4);
    }

    uint x_size_5 = 10;
    uint y_size_5 = 4;

    private void AutomatonInit5()
    {
        for (uint x = 6; x < x_size - 1; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetAlive(false);
                cells[x, y].SetEditable(false);
            }
        }
        for (uint y = 0; y < y_size; ++y) {
            cells[x_size - 1, y].SetTarget(true);
        }
    }

    private bool VictoryCondition5()
    {
        for (uint y = 0; y < y_size; ++y) {
            if (cells[x_size - 1, y].IsAlive()) {
                return true;
            }
        }
        return false;
    }

    uint x_size_6 = 8;
    uint y_size_6 = 8;

    private void AutomatonInit6()
    {
        for (uint x = 0; x < 4; ++x) {
            for (uint y = 4; y < 8; ++y) {
                cells[x, y].SetEmpty(true);
            }
        }
        for (uint x = 4; x < 8; ++x) {
            for (uint y = 0; y < 8; ++y) {
                cells[x, y].SetEditable(false);
            }
        }
        cells[4, 7].SetTarget(true);
    }

    private bool VictoryCondition6()
    {
        return cells[4, 7].IsAlive();
    }

    uint x_size_7 = 8;
    uint y_size_7 = 7;

    private void AutomatonInit7()
    {
        for (uint x = 0; x < 4; ++x) {
            for (uint y = 4; y < 7; ++y) {
                cells[x, y].SetEmpty(true);
            }
        }
        for (uint x = 4; x < 8; ++x) {
            for (uint y = 0; y < 7; ++y) {
                cells[x, y].SetEditable(false);
            }
        }
        cells[7, 6].SetTarget(true);
    }

    private bool VictoryCondition7()
    {
        return cells[7, 6].IsAlive();
    }

    uint x_size_8 = 11;
    uint y_size_8 = 10;

    private void AutomatonInit8()
    {
        for (uint y = 0; y < 7; ++y) {
            for (uint x = 3; x < 11; ++x) {
                cells[x, y].SetEditable(false);
            }
        }
        cells[2, 6].SetEditable(false);
        cells[3, 7].SetEditable(false);
        cells[3, 8].SetEditable(false);
        cells[4, 7].SetEditable(false);
        for (uint x = 3; x < 11; ++x) {
            cells[x, 9].SetEmpty(true);
        }
        for (uint x = 4; x < 11; ++x) {
            cells[x, 8].SetEmpty(true);
        }
        for (uint x = 5; x < 11; ++x) {
            cells[x, 7].SetEmpty(true);
        }
        for (uint x = 6; x < 11; ++x) {
            cells[x, 6].SetEmpty(true);
        }
        for (uint x = 7; x < 11; ++x) {
            cells[x, 5].SetEmpty(true);
        }
        for (uint x = 8; x < 11; ++x) {
            cells[x, 4].SetEmpty(true);
        }
        for (uint x = 9; x < 11; ++x) {
            cells[x, 3].SetEmpty(true);
        }
        for (uint x = 10; x < 11; ++x) {
            cells[x, 2].SetEmpty(true);
        }
        for (uint x = 0; x < 1; ++x) {
            cells[x, 7].SetEmpty(true);
        }
        for (uint x = 0; x < 2; ++x) {
            cells[x, 6].SetEmpty(true);
        }
        for (uint x = 0; x < 3; ++x) {
            cells[x, 5].SetEmpty(true);
        }
        for (uint x = 0; x < 4; ++x) {
            cells[x, 4].SetEmpty(true);
        }
        for (uint x = 0; x < 5; ++x) {
            cells[x, 3].SetEmpty(true);
        }
        for (uint x = 0; x < 6; ++x) {
            cells[x, 2].SetEmpty(true);
        }
        for (uint x = 0; x < 7; ++x) {
            cells[x, 1].SetEmpty(true);
        }
        for (uint x = 0; x < 8; ++x) {
            cells[x, 0].SetEmpty(true);
        }
        cells[10, 0].SetTarget(true);
    }

    private bool VictoryCondition8()
    {
        return cells[10, 0].IsAlive();
    }

    // sandbox
    uint x_size_9 = 14;
    uint y_size_9 = 10;

    private void AutomatonInit9()
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].sandbox_cell1 = true;
            }
        }
    }

    private bool VictoryCondition9()
    {
        return false;
    }

    // sandbox
    uint x_size_10 = 14;
    uint y_size_10 = 10;

    private void AutomatonInit10()
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].sandbox_cell2 = true;
            }
        }
    }

    private bool VictoryCondition10()
    {
        return false;
    }

    // sandbox
    uint x_size_11 = 14;
    uint y_size_11 = 10;

    private void AutomatonInit11()
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].sandbox_cell3 = true;
            }
        }
    }

    private bool VictoryCondition11()
    {
        return false;
    }

    uint x_size_default = 14;
    uint y_size_default = 10;

    private void AutomatonInitDefault()
    {
        // for each cell
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].Initialize(
                    false, // alive
                    true, // clickable
                    true, // editable
                    false, // target
                    false // empty
                );
            }
        }
    }

    private bool VictoryConditionDefault()
    {
        return false;
    }
}
