using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

    private string m_task;
    private Texture2D m_picture;
    private string m_warning; 

	public Task(string task, Texture2D picture, string warning)
    {
        m_task = task;
        m_picture = picture;
        m_warning = warning; 
    }
    
    public string task
    {
        get
        {
            return m_task; 
        }
    }

    public Texture2D picture
    {
        get
        {
            return m_picture; 
        }
    }

    public string warning
    {
        get
        {
            return m_warning; 
        }
    }
}
