using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool alive;
    private bool clickable;
    private bool editable;

    public Sprite alive_img;
    public Sprite alive_uneditable_img;
    public Sprite dead_img;
    public Sprite dead_uneditable_img;

    void Start() {}

    // use this for initialization
    public void Initialize(bool alive, bool clickable, bool editable)
    {
        SetAlive(alive);
        SetClickable(clickable);
        SetEditable(editable);
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
            clickable = false;
        }
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (alive) {
            if (editable) {
                GetComponent<SpriteRenderer>().sprite = alive_img;
            } else {
                GetComponent<SpriteRenderer>().sprite = alive_uneditable_img;
            }
        } else { // dead
            if (editable) {
                GetComponent<SpriteRenderer>().sprite = dead_img;
            } else {
                GetComponent<SpriteRenderer>().sprite = dead_uneditable_img;
            }
        }
    }
}
