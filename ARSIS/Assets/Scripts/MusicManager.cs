using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource m_Source;
    public AudioClip m_AdeleSong;
    public AudioClip m_Africa;
    public AudioClip m_Skyfall;
    public AudioClip m_SpaceOddity;
    public AudioClip m_Thunderstruck;
    public AudioClip m_Eclipse;
    public AudioClip m_RocketMan;

    public static MusicManager m_Instance;
	
	void Start () {
        m_Instance = this;
	}

    public void PlaySong(AudioClip clip)
    {
        if (!m_Source.isPlaying || m_Source.clip != clip)
        {
            m_Source.clip = clip;
            m_Source.Play();
        } 
    }

    public void StopMusic()
    {
        m_Source.Stop();
    }
}
