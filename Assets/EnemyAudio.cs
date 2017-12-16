using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour {

	public static EnemyAudio Instance;

	public AudioClip[] hit;
	public AudioClip[] hitWhileInvul;
	public AudioClip[] die;
	public AudioClip bossDie;
	private AudioSource source;
	
	void Start () {
		Instance = this;
		source = GetComponent<AudioSource>();
	}

	public static void Play(AudioClip c, float volume)
	{
		Instance.source.PlayOneShot(c, volume);
	}

	public static void Play(AudioClip c)
	{
		Play(c, Instance.source.volume);
	}

	public static void Play(AudioClip[] c)
	{
		Play(Choose(c), Instance.source.volume);
	}

	public static void Play(AudioClip[] c, float volume)
	{
		Play(Choose(c), volume);
	}
	
	void Update () {
		
	}

	public static AudioClip Choose(AudioClip[] clips)
	{
		return clips[Random.Range(0, clips.Length)];
	}
}
