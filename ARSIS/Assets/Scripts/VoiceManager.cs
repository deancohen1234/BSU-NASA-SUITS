using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class VoiceManager : MonoBehaviour {
    private KeywordRecognizer _keywordRecognizer = null; 
    private readonly Dictionary<string, System.Action> _keywords = new Dictionary<string, System.Action>();
    private bool _visible = false;

    public GameObject curMenu; 

    public GameObject mainMenu;
    public GameObject menuController;
    private MenuController mc;

    [Header("Audio")]
    public AudioSource m_Source;

    public AudioClip m_OpenMenu;
    public AudioClip m_CloseMenu;
    public AudioClip m_ChangeMenu; 
    public AudioClip m_NextButton;
    public AudioClip m_BackButton;
    public AudioClip m_ZoomIn;
    public AudioClip m_ZoomOut;
    public AudioClip m_SliderSound; 

    public GameObject m_brightnessMenu;
    public GameObject m_volumeMenu; 
    
    void Start () {
        #region keywords
        // Menus 
        _keywords.Add("Adele Main", MainMenu);
        _keywords.Add("Adele Settings", Settings);
        _keywords.Add("Adele Brightness", Brightness);
        _keywords.Add("Adele Volume", Volume);
        _keywords.Add("Adele Biometrics", Biometrics);
        _keywords.Add("Adele Houston", Houston);
        _keywords.Add("Adele Help", Help);
        _keywords.Add("Help", Help); 
        _keywords.Add("Adele TaskList", TaskList);

        // Navigation
        _keywords.Add("Adele Menu", Menu);
        _keywords.Add("Adele Move", Menu); 
        _keywords.Add("Adele Reset", ResetScene);
        _keywords.Add("Adele Clear", ResetScene); 

        // Special Functions
        _keywords.Add("Increase", Increase);
        _keywords.Add("Decrease", Decrease);
        _keywords.Add("Adele Capture", TakePhoto);
        _keywords.Add("Adele Toggle", Toggle);

        // Task List 
        _keywords.Add("Adele Task", generateTaskMenu);
        _keywords.Add("Next", Proceed);
        _keywords.Add("Back", Regress);
        _keywords.Add("Zoom Out", zoomOut);
        _keywords.Add("Zoom In", zoomIn);

        // Tasks 
        _keywords.Add("Disable Alarm", disableAlarm);
        _keywords.Add("Reroute Power", reroutePower);

        //Music
        _keywords.Add("Adele Hello", PlayAdele);
        _keywords.Add("Stop", StopMusic);

        #endregion

        // Keyword recognition 
        _keywordRecognizer = new KeywordRecognizer(_keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();

        // Set up MenuController 
        curMenu = null;

        mc = FindObjectOfType(typeof(MenuController)) as MenuController;
    }

    // Keyword Functions 
    #region Menu Functions

    private void MainMenu()
    {
        mc.ChangeMenu(mc.m_mainMenu); 
    }

    public void Settings()
    {
        mc.ChangeMenu(mc.m_settingsMenu); 
    }

    public void Houston()
    {
        mc.ChangeMenu(mc.m_sosMenu);
        ServerConnect.S.sos(); 
    }

    public void Help()
    {
        mc.ChangeMenu(mc.m_helpMenu); 
    }

    public void Biometrics()
    {
        mc.ChangeMenu(mc.m_biometricsMenu); 
    }

    private void Brightness()
    {
        mc.ChangeMenu(mc.m_brightnessMenu);
    }

    private void Volume()
    {
        mc.ChangeMenu(mc.m_volumeMenu);
    }

    private void TaskList()
    {
        mc.ChangeMenu(mc.m_taskList);  
    }

    #endregion

    #region Navigation Functions 

    private void Menu()
    {
        mc.ChangeMenu(mc.m_CurrentMenu);

        m_Source.clip = m_OpenMenu;
        m_Source.Play();
    }

    private void ResetScene()
    {
        m_Source.clip = m_CloseMenu;
        m_Source.Play();

        SceneManager.LoadScene(0);
    }

    #endregion

    #region Special Functions 

    private void TakePhoto()
    {
        HoloLensSnapshotTest.S.TakePhoto();

        m_Source.clip = m_ZoomOut;
        m_Source.Play();
    }

    private void Toggle()
    {
        HoloLensSnapshotTest.S.ToggleImage();

        m_Source.clip = m_ZoomIn;
        m_Source.Play();
    }

    private void Increase()
    {
        if (mc.m_CurrentMenu.Equals(m_brightnessMenu))
        {
            Debug.Log("Increasing Brightness");
            GameObject GOlt = GameObject.Find("Point light");
            Light lt = GOlt.GetComponent<Light>();
            if (lt.intensity < 1.4)
            {
                lt.intensity += 0.2f;
                SliderMove sm = mc.m_CurrentMenu.GetComponent<SliderMove>();
                sm.Increase();

                m_Source.clip = m_SliderSound;
                m_Source.Play();
            }
        }
        if (mc.m_CurrentMenu.Equals(m_volumeMenu))
        {
            Debug.Log("Increasing Volume");
            if (m_Source.volume < 1)
            {
                m_Source.volume += 0.2f;
                SliderMove sm = mc.m_CurrentMenu.GetComponent<SliderMove>();
                sm.Increase();

                m_Source.clip = m_SliderSound;
                m_Source.Play();
            }
        }
    }

    private void Decrease()
    {
        if (mc.m_CurrentMenu.Equals(GameObject.Find("ToggleSliderMenu")))
        {
            GameObject GOlt = GameObject.Find("Point light");
            Light lt = GOlt.GetComponent<Light>();
            if (lt.intensity > 0.6)
            {
                lt.intensity -= 0.2f;
                SliderMove sm = mc.m_CurrentMenu.GetComponent<SliderMove>();
                sm.Decrease();

                m_Source.clip = m_SliderSound;
                m_Source.Play();
            }
        }
        if (mc.m_CurrentMenu.Equals(m_volumeMenu))
        {
            Debug.Log("Decreasing Volume");
            if (m_Source.volume > 0)
            {
                m_Source.volume -= 0.2f;
                SliderMove sm = mc.m_CurrentMenu.GetComponent<SliderMove>();
                sm.Decrease();

                m_Source.clip = m_SliderSound;
                m_Source.Play();
            }
        }
    }

    #endregion

    #region Task List Functions 

    private void generateTaskMenu()
    {
        mc.ChangeMenu(mc.m_blankTaskMenu);
        displayStep();

        m_Source.clip = m_OpenMenu;
        m_Source.Play();
    }

    private void Proceed()
    {
        mc.currentStep++;
        displayStep();

        m_Source.clip = m_NextButton;
        m_Source.Play();
    }

    private void Regress()
    {
        mc.currentStep--;
        displayStep();

        m_Source.clip = m_BackButton;
        m_Source.Play();
    }

    private void zoomOut()
    {
        mc.zoomOut();

        m_Source.clip = m_ZoomOut;
        m_Source.Play();
    }

    private void zoomIn()
    {
        mc.zoomIn();

        m_Source.clip = m_ZoomIn;
        m_Source.Play();
    }

    private void displayStep()
    {
        int curStep = mc.currentStep;
        int curTask = mc.currentTask;

        string curText = TaskManager.S.getStep(curTask, curStep);
        string prevText = TaskManager.S.getStep(curTask, curStep - 1);
        string nextText = TaskManager.S.getStep(curTask, curStep + 1);

        mc.m_stepText.text = curText;

        mc.m_stepPrevText.text = prevText;
        mc.m_stepCurText.text = curText;
        mc.m_stepNextText.text = nextText;

        Texture2D curImage = TaskManager.S.getPic(curTask, curStep);

        mc.m_stepImage.texture = curImage;

        string warningText = TaskManager.S.getWarning(curTask, curStep);
        mc.m_warningText.text = warningText;
    }

    #endregion

    #region Task Names

    private void disableAlarm()
    {
        mc.currentTask = 1;
        mc.currentStep = 1;
        generateTaskMenu();
    }

    private void reroutePower()
    {
        mc.currentTask = 2;
        mc.currentStep = 1;
        generateTaskMenu();
    }

    #endregion

    #region Music Functions 

    private void PlayAdele()
    {
        MusicManager.m_Instance.PlayAdele();
    }

    private void StopMusic()
    {
        MusicManager.m_Instance.StopMusic();
    }
    #endregion 

    // Keyword Recognition 
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction; 
        if (_keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(); 
        }
    }

    // For testing only 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            MainMenu();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Menu();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Increase();
        }
    }
}
