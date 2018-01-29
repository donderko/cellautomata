using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenBehavior : MonoBehaviour {

    private uint counter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ++counter;
        if (counter > 200) {
            gameObject.SetActive(false);
        }
	}
}
