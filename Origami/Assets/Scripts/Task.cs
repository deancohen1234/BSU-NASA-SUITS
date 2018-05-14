using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {

    private string m_taskString;
    private Texture2D m_taskImage; 
    
    public Task(string taskString, Texture2D taskImage)
    {
        this.m_taskString = taskString;
        this.m_taskImage = taskImage; 
    }

    public string taskString
    {
        get
        {
            return m_taskString; 
        }
    }

    public Texture2D taskImage
    {
        get
        {
            return m_taskImage; 
        }
    }
}
