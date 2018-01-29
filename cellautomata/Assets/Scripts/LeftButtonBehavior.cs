﻿using UnityEngine;

public class LeftButtonBehavior : MonoBehaviour
{
    public GameLogic game_logic;
    public AudioManagerBehavior audio_manager;

    public Sprite enabled_sprite;
    public Sprite disabled_sprite;

    // use this for initialization
    void Start()
    {

    }

    // called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (game_logic != null) {
            game_logic.PreviousAutomaton();
            audio_manager.PlayClickSound();
        }
    }

    public void SetEnabledSprite(bool enabled)
    {
        if (enabled) {
            GetComponent<SpriteRenderer>().sprite = enabled_sprite;
        } else {
            GetComponent<SpriteRenderer>().sprite = disabled_sprite;
        }
    }
}
