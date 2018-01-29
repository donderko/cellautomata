using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerBehavior : MonoBehaviour
{
    public AudioSource source;
    public AudioClip play_sound;
    public float play_volume = 0.5f;
    public AudioClip stop_sound;
    public float stop_volume = 0.5f;
    public AudioClip toggle_sound;
    public float toggle_volume = 0.5f;
    public float toggle_high_pitch_range = 4.5f;
    public float toggle_low_pitch_range = 0.2f;

    void Start ()
    {
		
	}

    // use this for initialization
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // called once per frame
    void Update ()
    {
		
	}

    public void PlayStopSound()
    {
        source.PlayOneShot(stop_sound, stop_volume);
    }

    public void PlayPlaySound()
    {
        source.PlayOneShot(play_sound, play_volume);
    }

    public void PlayToggleSound()
    {
        source.pitch = Random.Range(toggle_low_pitch_range, toggle_high_pitch_range);
        source.PlayOneShot(toggle_sound, toggle_volume);
    }

    public void PlayAutomatonSound(uint[] row_counts, uint[] column_counts)
    {
        int row_counts_length = row_counts.GetLength(0);
        for (int row = 0; row < row_counts_length; ++row) {
            float row_normalized = (float)row_counts[row] / (float)row_counts_length;
            source.pitch = row_normalized + 0.2f;
            source.PlayOneShot(toggle_sound, 0.3f);
        }
    }
}
