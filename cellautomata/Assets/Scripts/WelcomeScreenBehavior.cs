using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenBehavior : MonoBehaviour {

    public IntroScreenBehavior intro;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown) {
            gameObject.SetActive(false);
            intro.gameObject.SetActive(true);
        }
    }
}
