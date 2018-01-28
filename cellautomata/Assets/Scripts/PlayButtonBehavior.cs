using UnityEngine;

public class PlayButtonBehavior : MonoBehaviour
{
    public Automaton automaton;

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
        if (automaton != null) {
            automaton.Play();
        }
    }
}
