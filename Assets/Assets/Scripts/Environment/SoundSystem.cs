using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour {

	public AudioMixerSnapshot MenuMode;
	public AudioMixerSnapshot GameMode;
	public AudioSource MenuMusic;
	public float bpm = 120;

	private float m_TransitionIn;
	private float m_TransitionOut;
	private float m_QuarterNote;

	// Use this for initialization
	void Start () {
		m_QuarterNote = 60 / bpm;
		m_TransitionIn = m_QuarterNote;
		m_TransitionOut = m_QuarterNote;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GameStartMusic()
	{
		GameMode.TransitionTo (m_TransitionIn);
	}

	public void GameEndMusic()
	{
		MenuMusic.Play ();
		MenuMode.TransitionTo (m_TransitionOut);
	}
}
