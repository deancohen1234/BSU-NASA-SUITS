using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource m_Source;
    public AudioClip m_AdeleSong;

    public static MusicManager m_Instance;
	// Use this for initialization
	void Start () {
        m_Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayAdele()
    {
        if (!m_Source.isPlaying)
        {
            m_Source.clip = m_AdeleSong;
            m_Source.Play();
        } 
    }

    public void StopMusic()
    {
        m_Source.Stop();
    }
}
