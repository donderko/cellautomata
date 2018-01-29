using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float step_size = 0.01f;
    private bool pulse_enabled = false;
    private float angle = 0f;

    private void Update()
    {
        if (pulse_enabled) {
            angle += step_size;
            float val = (float)System.Math.Sin((angle % 360));
            gameObject.transform.localScale = new Vector3(val, val, 1);
        }
    }

    public void SetEnabled(bool enabled)
    {
        pulse_enabled = enabled;
        if (!enabled) {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}