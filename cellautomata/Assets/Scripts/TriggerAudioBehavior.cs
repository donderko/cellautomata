using UnityEngine;

public class TriggerAudioBehavior : MonoBehaviour
{
    public AudioClip static_sound;

    private AudioSource source;
    private float volume = 0.5f;

	// use this for initialization
	void Start()
    {
		
	}

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // called once per frame
    void Update()
    {
		
	}

    public void TriggerAudio()
    {
        source.PlayOneShot(static_sound, volume);
    }
}
