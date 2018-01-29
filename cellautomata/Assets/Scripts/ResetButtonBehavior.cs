using UnityEngine;

public class ResetButtonBehavior : MonoBehaviour
{
    public Sprite reinitialize_img;
    public Sprite clear_img;
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
            automaton.ResetButtonAction();
        }
    }
}
