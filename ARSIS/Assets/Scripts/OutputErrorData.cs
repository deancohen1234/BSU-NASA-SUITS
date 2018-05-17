using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputErrorData : MonoBehaviour {

    public Text m_ErrorText;

    [Header("Audio")]
    public AudioSource m_Source;

    public AudioClip m_bad;

    public void OutputErrorText(string s)
    {
        m_ErrorText.text += s + "\n";
        m_Source.clip = m_bad;
        m_Source.loop = true;
        m_Source.Play();
    }

    public void ClearText()
    {
        m_ErrorText.text = "";
        m_Source.loop = false; 
        m_Source.Stop(); 
    }
}