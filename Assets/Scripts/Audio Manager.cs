using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

    bool second = false;

    float sound, music;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
        // sound = PlayerPrefs.GetFloat("sound_volume");
        // music = PlayerPrefs.GetFloat("music_volume");
      

        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            // if (s.music)
            //     s.volume = music;
            // else
            //     s.volume = sound ;
            s.source.outputAudioMixerGroup = mixerGroup;
		}
       // sounds[0].source.volume = music.value/100;

	}
     void Start()
    {
        Play("11");
    }
    public void update(float S,float M)
    {

        // foreach (Sound s in sounds)
        // {
        //     if (s.music)
        //         s.source.volume = M;
        //     else
        //         s.source.volume = S;
        // }
    }

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);

        if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume ;
		s.source.pitch = s.pitch;
		s.source.Play();
	}
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }


        s.source.Stop();
        
    }
    

}
