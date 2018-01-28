using System;
using UnityEngine;

public class Automaton : MonoBehaviour
{
    public Cell cell_prefab;

    // puzzle-specific
    private uint x_size;
    private uint y_size;
    private Action InitAutomaton;
    private Func<bool> VictoryCondition;

    // general
    private bool running;
    private bool victory;
    private bool first_run;
    private Cell[,] cells;
    private bool[,] nest_states;
    private bool[,] saved_states;

    void Start()
    {

    }

    // use this for initialization
    public void Initialize(uint automaton_id)
    {
        running = false;
        victory = false;
        first_run = true;

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
            case 5:
                x_size = x_size_5;
                y_size = y_size_5;
                InitAutomaton = AutomatonInit5;
                VictoryCondition = VictoryCondition5;
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
    public void PlayButtonAction()
    {
        if (!running && !victory) {
            running = true;
            if (first_run) {
                first_run = false;
                SaveStates();
            }
            RunAutomaton();
        }
    }

    // do this when the stop button is pressed
    public void StopButtonAction()
    {
        if (running) {
            running = false;
            PauseAutomaton();
        }
    }

    // do this when the reset button is pressed
    public void ResetButtonAction()
    {
        victory = false;

        if (running) {
            running = false;
            first_run = true;
            PauseAutomaton();
            ResetAutomaton();
            LoadStates();
        } else { // paused
            running = false;
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
        CancelInvoke();
    }

    // run the automaton
    private void RunAutomaton()
    {
        SetAllClickable(false);
        InvokeRepeating("TickCells", 0, 0.2f);
    }

    // stores the alive/dead states of all cells
    private void SaveStates()
    {
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
        nest_states = new bool[x_size, y_size];
        saved_states = new bool[x_size, y_size];

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
                cells[x, y].name = "Cell (" + x + "," + y + ")";
            }
        }
    }

    // update all cells one time tick
    private void TickCells()
    {
        // compute next states
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                uint alive_neighbor_count = AliveNeighborCount(x, y);
                if (cells[x, y].IsAlive()) {
                    if (alive_neighbor_count < 2 || alive_neighbor_count > 3) {
                        nest_states[x, y] = false;
                    } else {
                        nest_states[x, y] = true;
                    }
                } else { // the cell is dead
                    if (alive_neighbor_count == 3) {
                        nest_states[x, y] = true;
                    } else {
                        nest_states[x, y] = false;
                    }
                }
            }
        }

        // apply next states
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                if (nest_states[x, y]) {
                    cells[x, y].SetAlive(true);
                } else {
                    cells[x, y].SetAlive(false);
                }
            }
        }

        if (VictoryCondition()) {
            DoVictory();
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
        PauseAutomaton();
    }

    /////////////////////////////////////////////////
    /// PUZZLES
    /// TODO: move these into their own class/file
    /////////////////////////////////////////////////

    uint x_size_1 = 5;
    uint y_size_1 = 3;

    private void AutomatonInit1()
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

    private bool VictoryCondition1()
    {
        if (cells[4, 0].IsAlive() || cells[4, 1].IsAlive() || cells[4, 2].IsAlive()) {
            return true;
        }
        return false;
    }

    uint x_size_2 = 9;
    uint y_size_2 = 3;

    private void AutomatonInit2()
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

    private bool VictoryCondition2()
    {
        return cells[4, 2].IsAlive();
    }

    // can be 11
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
