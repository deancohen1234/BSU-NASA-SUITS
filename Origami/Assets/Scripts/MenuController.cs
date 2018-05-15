using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MenuController : MonoBehaviour
{
    public GameObject m_OGMenu;
    //public GameObject m_NextMenu;

    public GameObject m_CurrentMenu;
    private GameObject m_PreviousMenu;

    public int currentStep;
    public int currentTask; 

    public GameObject[] m_TaskListArray;
    public Text[] m_StepArray;
    public GameObject m_mainMenu;
    public GameObject m_settingsMenu;
    public GameObject m_brightnessMenu;
    public GameObject m_volumeMenu; 
    public GameObject m_sosMenu;
    public GameObject m_helpMenu;
    public GameObject m_biometricsMenu;
    public GameObject m_taskList; 

    public GameObject m_blankTaskMenu;
    public Text m_stepText;
    public RawImage m_stepImage;
    public Text m_stepPrevText;
    public Text m_stepCurText;
    public Text m_stepNextText;
    public Text m_warningText; 

    private int m_CurrentTaskIndex = 0;

    public bool taskZoomedIn = false;

    [Header("Audio")]
    public AudioSource m_Source;

    public AudioClip m_changeMenuSound; 

    public void Start()
    {
        m_CurrentMenu = m_OGMenu;
        currentStep = 1;
        currentTask = 1; 
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

        m_Source.clip = m_changeMenuSound;
        m_Source.Play();
    }

    public void ChangeMenuNonBlender(GameObject newMenu)
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

        ToggleVisibilityNonBlender(newMenu);
        //newMenu.SetActive(true);
    }

    public void zoomOut()
    {
        m_stepImage.gameObject.SetActive(false);
        m_stepText.gameObject.SetActive(false);
        m_warningText.gameObject.SetActive(false); 

        m_stepPrevText.gameObject.SetActive(true);
        m_stepNextText.gameObject.SetActive(true);
        m_stepCurText.gameObject.SetActive(true); 
    }

    public void zoomIn()
    {
        m_stepImage.gameObject.SetActive(true);
        m_stepText.gameObject.SetActive(true);
        m_warningText.gameObject.SetActive(true);

        m_stepPrevText.gameObject.SetActive(false);
        m_stepNextText.gameObject.SetActive(false);
        m_stepCurText.gameObject.SetActive(false);
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

    private void ToggleVisibilityNonBlender(GameObject holoMenu)
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

        holoMenu.transform.rotation = Quaternion.identity;

        holoMenu.transform.position += new Vector3(0, .125f, 0);

        holoMenu.transform.rotation = Quaternion.Euler(new Vector3(holoMenu.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, holoMenu.transform.eulerAngles.z));


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
