using UnityEngine;

public class CellBoard : MonoBehaviour {

    public Cell cell_prefab;

    private uint x_size;
    private uint y_size;
    private bool running;
    private Cell[,] cells;
    private bool[,] next_state;

    void Start() {}

    // use this for initialization
    public void Initialize(uint x_size, uint y_size)
    {
        // passed in values
        this.x_size = x_size;
        this.y_size = y_size;

        // default vaules
        running = false;

        // array construction
        cells = new Cell[x_size, y_size];
        next_state = new bool[x_size, y_size];

        // compute offsets so that the grid is centered on the screen
        float x_offset = (x_size - 1) / 2 + (x_size % 2 == 0 ? 0.5f : 0.0f); // magic
        float y_offset = (y_size - 1) / 2 + (y_size % 2 == 0 ? 0.5f : 0.0f); // magic
        
        // instantiate a grid of cells
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                // instantiate
                Vector3 pos = new Vector3(x - x_offset, y - y_offset, 0) * 1;
                cells[x, y] = Instantiate(cell_prefab, pos, Quaternion.identity) as Cell;
                cells[x, y].transform.SetParent(this.transform);
                cells[x, y].name = "Cell (" + x + "," + y + ")";

                // init cell
                cells[x, y].Initialize(
                    false, // alive
                    true, // clickable
                    true // editable
                );
            }
        }

        // generate a preset board
        GeneratePreset1();
    }

    public void Initialize()
    {

    }

    // called once per frame
    void Update()
    {
        // pause or resume automata
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (running) {
                PauseAutomata();
            } else {
                ResumeAutomata();
            }
        }

        // reset automata
        if (Input.GetKeyDown(KeyCode.R)) {
            // pause automata
            PauseAutomata();
            // set all cells to be dead
            for (uint x = 0; x < x_size; ++x) {
                for (uint y = 0; y < y_size; ++y) {
                    cells[x, y].SetAlive(false);
                }
            }
        }
    }

    // begin the automata
    private void ResumeAutomata()
    {
        SetClickable(false);
        SetEditable(true);
        running = true;
        InvokeRepeating("TickCells", 0, 0.2f);
    }

    // pause the automata
    private void PauseAutomata()
    {
        SetClickable(true);
        running = false;
        CancelInvoke();
    }

    // set all cells to be clickable or not
    private void SetClickable(bool clickable)
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetClickable(clickable);
            }
        }
    }

    // set all cells to be editable or not
    private void SetEditable(bool editable)
    {
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                cells[x, y].SetEditable(editable);
            }
        }
    }

    // update all cells one time tick
    private void TickCells()
    {
        // compute next cell states
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                uint aliveNeighborCount = AliveNeighborCount(x, y);
                if (cells[x, y].IsAlive()) {
                    if (aliveNeighborCount < 2 || aliveNeighborCount > 3) {
                        // kill the cell
                        next_state[x, y] = false;
                    } else {
                        next_state[x, y] = true;
                    }
                } else { // the cell is dead
                    if (aliveNeighborCount == 3) {
                        // birth the cell
                        next_state[x, y] = true;
                    } else {
                        next_state[x, y] = false;
                    }
                }
            }
        }

        // apply next states
        for (uint x = 0; x < x_size; ++x) {
            for (uint y = 0; y < y_size; ++y) {
                if (next_state[x, y]) {
                    cells[x, y].SetAlive(true);
                } else {
                    cells[x, y].SetAlive(false);
                }
            }
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

    private void GeneratePreset1()
    {
        cells[1, 1].SetAlive(true);
        cells[1, 1].SetEditable(false);

        cells[4, 4].SetAlive(false);
        cells[4, 4].SetEditable(false);

        cells[7, 7].SetAlive(true);
        cells[7, 7].SetEditable(false);
    }
}
