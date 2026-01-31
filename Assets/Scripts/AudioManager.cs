using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

	public static AudioManager Instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public Sound Play(string sound)
	{
		Sound s = Find(sound);

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
		return s;
	}

	private Sound Find(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
		return s;

	}

	public void Stop(string sound)
	{

		Find(sound).source.Stop();
    }

	public void StopAll()
	{
        foreach (Sound sound in sounds)
        {
			Stop(sound.name);
        }
    }

	public void changeMask(string sound)
	{
		int scratchNumber = UnityEngine.Random.Range(0, 5);
		Sound scratch = Find("scratch" + scratchNumber.ToString());
		Sound loop = Find(sound);
		Debug.Log(loop);
		Sound putMask = Find("putMask");
		double nowtime = AudioSettings.dspTime;
		putMask.source.Play();
		scratch.source.PlayScheduled(nowtime + putMask.source.clip.length);
		loop.source.PlayScheduled(nowtime + putMask.source.clip.length + scratch.source.clip.length);


    }

}
