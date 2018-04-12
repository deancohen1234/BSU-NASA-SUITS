using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject m_OGMenu;
    public GameObject m_NextMenu;

    public GameObject m_CurrentMenu;
    private GameObject m_PreviousMenu;

    public GameObject[] m_TaskListArray;

    private int m_CurrentTaskIndex = 0;

    public void Start()
    {
        m_CurrentMenu = m_OGMenu;
    }
    //hide old menu, and switch to new menu
    public void ChangeMenu(GameObject oldMenu, GameObject newMenu)
    {
        m_CurrentMenu = newMenu;
        m_PreviousMenu = oldMenu;

        newMenu.transform.position = oldMenu.transform.position;
        newMenu.transform.rotation = Quaternion.Euler(new Vector3(newMenu.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, newMenu.transform.eulerAngles.z));

        oldMenu.SetActive(false);
        newMenu.SetActive(true);
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
        ChangeMenu(m_TaskListArray[m_CurrentTaskIndex], m_TaskListArray[m_CurrentTaskIndex + 1]);

        m_CurrentTaskIndex++;

    }

    public void PreviousTask()
    {
        ChangeMenu(m_TaskListArray[m_CurrentTaskIndex], m_TaskListArray[m_CurrentTaskIndex - 1]);

        m_CurrentTaskIndex--;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeMenu(m_OGMenu, m_NextMenu);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoBack();
        }
    }
}
