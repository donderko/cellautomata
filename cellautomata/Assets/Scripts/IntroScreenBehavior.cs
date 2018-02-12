using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreenBehavior : MonoBehaviour {

    public IntroScreenBehavior next_screen;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown) {
            gameObject.SetActive(false);
            if (next_screen) {
                next_screen.gameObject.SetActive(true);
            }
        }
	}
}
