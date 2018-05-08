using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject m_OGMenu;
    //public GameObject m_NextMenu;

    public GameObject m_CurrentMenu;
    private GameObject m_PreviousMenu;

    public GameObject[] m_TaskListArray;
    public GameObject m_mainMenu;
    public GameObject m_settingsMenu;
    public GameObject m_brightnessMenu;
    public GameObject m_volumeMenu; 
    public GameObject m_sosMenu;
    public GameObject m_helpMenu;
    public GameObject m_biometricsMenu; 

    private int m_CurrentTaskIndex = 0;

    public void Start()
    {
        m_CurrentMenu = m_OGMenu;
    }
    //hide old menu, and switch to new menu
    public void ChangeMenu(GameObject newMenu)
    {
        GameObject oldMenu = m_CurrentMenu; 
        m_CurrentMenu = newMenu;
        if (oldMenu != null)
        {
            m_PreviousMenu = oldMenu;
            oldMenu.SetActive(false); 
        }
       
        newMenu.transform.position = oldMenu.transform.position;
        newMenu.transform.rotation = oldMenu.transform.rotation;
        //newMenu.transform.rotation = Quaternion.Euler(new Vector3(newMenu.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, newMenu.transform.eulerAngles.z));
        
        ToggleVisibility(newMenu); 
        //newMenu.SetActive(true);
    }

    private void ToggleVisibility(GameObject holoMenu)
    {
        if (holoMenu == null) return;

        if (m_CurrentMenu != null)
        {
            m_CurrentMenu.SetActive(false);
        }

        holoMenu.SetActive(true);

        holoMenu.transform.position =
            Camera.main.transform.position + (Camera.main.transform.forward);

        Vector3 cameraPos = Camera.main.transform.position;

        holoMenu.transform.rotation = Quaternion.Euler(new Vector3(holoMenu.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, holoMenu.transform.eulerAngles.z));

        holoMenu.transform.position += new Vector3(0, .125f, 0);

        m_CurrentMenu = holoMenu;
    }

    //go back to previous menu
    public void GoBack()
    {
        m_PreviousMenu.transform.position = m_CurrentMenu.transform.position;
        m_PreviousMenu.transform.rotation = Quaternion.Euler(new Vector3(m_CurrentMenu.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, m_PreviousMenu.transform.eulerAngles.z));

        m_CurrentMenu.SetActive(false);
        m_PreviousMenu.SetActive(true);
    }

    public void NextTask()
    {
        ChangeMenu(m_TaskListArray[m_CurrentTaskIndex + 1]);

        m_CurrentTaskIndex++;

    }

    public void PreviousTask()
    {
        ChangeMenu(m_TaskListArray[m_CurrentTaskIndex - 1]);

        m_CurrentTaskIndex--;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ChangeMenu(m_OGMenu, m_NextMenu);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoBack();
        }
    }
}
