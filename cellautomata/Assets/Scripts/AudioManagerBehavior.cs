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

    public AudioClip victory_sound;
    public AudioClip victory_sound2;
    public AudioClip victory_sound3;
    public AudioClip victory_sound4;
    public float victory_volume = 0.5f;

    public AudioClip dud_sound;
    public float dud_volume = 0.5f;

    public AudioClip click_sound;
    public float click_volume = 0.4f;

    public AudioClip reset_sound;
    public float reset_volume = 0.5f;

    public AudioClip sandbox_toggle_sound1;
    public float sandbox_toggle_volume1 = 0.5f;

    public AudioClip sandbox_toggle_sound2;
    public float sandbox_toggle_volume2 = 0.5f;

    public AudioClip sandbox_toggle_sound3;
    public float sandbox_toggle_volume3 = 0.5f;

    private AudioClip current_toggle_sound;
    public float current_toggle_volume = 0.5f;

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

    private void PlayCurrentToggleSound()
    {
        source.pitch = Random.Range(toggle_low_pitch_range, toggle_high_pitch_range);
        source.PlayOneShot(current_toggle_sound, current_toggle_volume);
    }

    public void PlayToggleSound()
    {
        current_toggle_sound = toggle_sound;
        current_toggle_volume = toggle_volume;
        PlayCurrentToggleSound();
    }

    public void PlaySandboxToggleSound1()
    {
        current_toggle_sound = sandbox_toggle_sound1;
        current_toggle_volume = sandbox_toggle_volume1;
        PlayCurrentToggleSound();
    }

    public void PlaySandboxToggleSound2()
    {
        current_toggle_sound = sandbox_toggle_sound2;
        current_toggle_volume = sandbox_toggle_volume2;
        PlayCurrentToggleSound();
    }

    public void PlaySandboxToggleSound3()
    {
        current_toggle_sound = sandbox_toggle_sound3;
        current_toggle_volume = sandbox_toggle_volume3;
        PlayCurrentToggleSound();
    }

    public void PlayAutomatonSound(uint[] row_counts, uint[] column_counts)
    {
        int row_counts_length = row_counts.GetLength(0);
        uint cap = 0;
        for (int row = 0; row < row_counts_length; ++row) {
            float row_normalized = (float)row_counts[row] / (float)row_counts_length;
            if (row_normalized > 0 && cap < 3) {
                ++cap;
                source.pitch = row_normalized + 0.2f;
                source.PlayOneShot(current_toggle_sound, 0.3f);
            }
        }

        /*
        int row_counts_length = row_counts.GetLength(0);
        float normalized_sum = 0;
        for (int row = 0; row < row_counts_length; ++row) {
            float row_normalized = (float)row_counts[row] / (float)row_counts_length;
            normalized_sum += row_normalized;
            //source.pitch = row_normalized + 0.2f;
            //source.PlayOneShot(toggle_sound, 0.3f);
        }
        source.pitch = (normalized_sum / (float)row_counts_length) + 0.2f;
        source.PlayOneShot(toggle_sound, 0.3f);
        */
    }

    public void PlayResetSound()
    {
        source.PlayOneShot(reset_sound, reset_volume);
    }
    public void PlayDudSound()
    {
        // miss clicked or clicked on thing that cant be changed
        source.PlayOneShot(dud_sound, dud_volume);
    }
    public void PlayClickSound()
    {
        // sounds for click on buttons or interactables
        // that are not special
        source.PlayOneShot(click_sound, click_volume);
    }
    public void PlayVictorySound()
    {
        StartCoroutine(CoPlayDelayedClip(victory_sound4, 0.0f, 1.0f));
    }

    IEnumerator CoPlayDelayedClip(AudioClip clip, float delay, float pitch)
    {
        yield return new WaitForSeconds(delay);
        source.pitch = pitch;
        source.PlayOneShot(clip, victory_volume);
    }
}
 