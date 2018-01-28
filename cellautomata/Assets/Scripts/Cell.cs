using UnityEngine;

public class Cell : MonoBehaviour
{
    public Sprite alive_editable_img;
    public Sprite alive_uneditable_img;
    public Sprite dead_editable_img;
    public Sprite dead_uneditable_img;
    public Sprite alive_target_img;
    public Sprite dead_target_img;

    private bool alive;
    private bool clickable;
    private bool editable;
    private bool target;

    void Start()
    {

    }

    // use this for initialization
    public void Initialize(bool alive, bool clickable, bool editable, bool target)
    {
        SetAlive(alive);
        SetClickable(clickable);
        SetEditable(editable);
        SetTarget(target);
    }

    // called once per frame
    void Update()
    {

    }

    // toggle dead/alive on mouse down
    private void OnMouseDown()
    {
        if (clickable) {
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
            // target cells are always unclickable and uneditable
            clickable = false;
            editable = false;
        }
        UpdateSprite();
    }

    // update appearance
    private void UpdateSprite()
    {
        if (alive) {
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
