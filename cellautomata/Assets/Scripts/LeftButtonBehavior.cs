using UnityEngine;

public class LeftButtonBehavior : MonoBehaviour
{
    public GameLogic game_logic;

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
        }
    }
}
