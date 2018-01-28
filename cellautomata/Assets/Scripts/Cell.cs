using UnityEngine;

public class Cell : MonoBehaviour
{
    public Sprite alive_editable_img;
    public Sprite alive_uneditable_img;
    public Sprite dead_editable_img;
    public Sprite dead_uneditable_img;
    public Sprite alive_target_img;
    public Sprite dead_target_img;
    public Sprite empty_img;

    private bool alive;
    private bool clickable;
    private bool editable;
    private bool target;
    private bool empty;

    void Start()
    {

    }

    // use this for initialization
    public void Initialize(bool alive, bool clickable, bool editable, bool target, bool empty)
    {
        SetAlive(alive);
        SetClickable(clickable);
        SetEditable(editable);
        SetTarget(target);
        SetEmpty(empty);
    }

    // called once per frame
    void Update()
    {

    }

    // toggle dead/alive on mouse down
    private void OnMouseDown()
    {
        if (clickable) {
            Automaton parent = transform.parent.gameObject.GetComponent<Automaton>();
            parent.source.pitch = Random.Range(parent.toggle_low_pitch_range, parent.toggle_high_pitch_range);
            parent.source.PlayOneShot(parent.toggle_sound, parent.toggle_volume);
            SetAlive(!alive);
        }
    }

    /*
    private void OnMouseOver()
    {
        //Debug.Log("OnMouseOver");
            //if (Input.GetMouseButtonDown(0)) { Debug.Log("GetMouseButtonDown"); }
        if (Input.GetMouseButton(0) && clickable && !alive) {
            SetAlive(true);
        }
    }
    */

    // set dead or alive state
    public void SetAlive(bool alive)
    {
        this.alive = alive;
        UpdateSprite();
    }

    // returns true if the cell is alive and false if it's dead
    public bool IsAlive()
    {
        return alive;
    }

    // set clickable state
    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;
    }

    // set editable state
    public void SetEditable(bool editable)
    {
        this.editable = editable;
        if (!editable) {
            // uneditable cells are always unclickable
            clickable = false;
        }
        UpdateSprite();
    }

    // set target state
    public void SetTarget(bool target)
    {
        this.target = target;
        if (target) {
            // target cells are always unclickable, uneditable, and non-empty
            clickable = false;
            editable = false;
            empty = false;
        }
        UpdateSprite();
    }

    // set empty state
    public void SetEmpty(bool empty)
    {
        this.empty = empty;
        if (empty) {
            // empty cells are always unclickable, uneditable, and non-targets
            clickable = false;
            editable = false;
            target = false;
        }
        UpdateSprite();
    }

    // update appearance
    private void UpdateSprite()
    {
        if (empty) {
            GetComponent<SpriteRenderer>().sprite = empty_img;
        } else if (alive) {
            if (target) {
                GetComponent<SpriteRenderer>().sprite = alive_target_img;
            } else if (editable) {
                GetComponent<SpriteRenderer>().sprite = alive_editable_img;
            } else { // uneditable
                GetComponent<SpriteRenderer>().sprite = alive_uneditable_img;
            }
        } else { // dead
            if (target) {
                GetComponent<SpriteRenderer>().sprite = dead_target_img;
            } else if (editable) {
                GetComponent<SpriteRenderer>().sprite = dead_editable_img;
            } else { // uneditable
                GetComponent<SpriteRenderer>().sprite = dead_uneditable_img;
            }
        }
    }
}
