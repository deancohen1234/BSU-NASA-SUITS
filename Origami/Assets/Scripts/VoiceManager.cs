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
    public AudioClip m_NextButton;
    public AudioClip m_BackButton;
    
    void Start () {
        #region keywords
        // Menus 
        _keywords.Add("Main", MainMenu);
        _keywords.Add("Settings", Settings);
        _keywords.Add("Brightness", Brightness);
        _keywords.Add("Volume", Volume);
        _keywords.Add("Biometrics", Biometrics);
        _keywords.Add("Houston", Houston);
        _keywords.Add("Help", Help);
      //  _keywords.Add("Task", Task);

        // Navigation
        _keywords.Add("Menu", Menu);
        _keywords.Add("Move", Menu); 
       // _keywords.Add("Next", Next);
        _keywords.Add("Go", Next); 
      //  _keywords.Add("Back", Back);
        _keywords.Add("Reset", ResetScene);
        _keywords.Add("Clear", ResetScene); 

        // Special Functions
        _keywords.Add("Increase", Increase);
        _keywords.Add("Decrease", Decrease);
        _keywords.Add("Capture", TakePhoto);
        _keywords.Add("Toggle", Toggle);

        // Task List 
        _keywords.Add("Task", generateTaskMenu);
        _keywords.Add("Next", Proceed);
        _keywords.Add("Back", Regress);
        _keywords.Add("ZoomOut", zoomOut);
        _keywords.Add("ZoomIn", zoomIn);

        // Tasks 
        _keywords.Add("DisableAlarm", disableAlarm);
        _keywords.Add("ReroutePower", reroutePower); 

        #endregion

        // Keyword recognition 
        _keywordRecognizer = new KeywordRecognizer(_keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();

        // Set up MenuController 
        curMenu = null;

        mc = FindObjectOfType(typeof(MenuController)) as MenuController;
    }

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

    private void zoomOut()
    {
        mc.zoomOut(); 
    }

    private void zoomIn()
    {
        mc.zoomIn();
    }

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
            generateTaskMenu(); 
        }
    }

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

    private void TakePhoto()
    {
       HoloLensSnapshotTest.S.TakePhoto(); 
    }

    #endregion

    private void Menu()
    {
        mc.ChangeMenu(mc.m_CurrentMenu);

        m_Source.clip = m_OpenMenu;
        m_Source.Play();
    }

    private void Task()
    {
        mc.ChangeMenu(mc.m_TaskListArray[0]); 
    }

    private void Next()
    {
        mc.NextTask();

        m_Source.clip = m_NextButton;
        m_Source.Play();

    }

    private void ResetScene()
    {
        m_Source.clip = m_CloseMenu;
        m_Source.Play();

        SceneManager.LoadScene(0);
    }

    private void Back()
    {
        mc.PreviousTask();

        m_Source.clip = m_BackButton;
        m_Source.Play();
    }

    private void Toggle()
    {
        HoloLensSnapshotTest.S.ToggleImage(); 
    }

    private void Increase()
    {
        //if (mc.m_CurrentMenu.Equals(GameObject.Find("ToggleSliderMenu")))
        //{
        Debug.Log("Increasing"); 
            GameObject GOlt = GameObject.Find("Point light");
            Light lt = GOlt.GetComponent < Light > ();
            if (lt.intensity < 1.4)
            {
                lt.intensity += 0.2f;
                SliderMove sm = mc.m_CurrentMenu.GetComponent < SliderMove > ();
                sm.Increase(); 
            }
        //}
    }

    private void Decrease()
    {
        //if (mc.m_CurrentMenu.Equals(GameObject.Find("ToggleSliderMenu")))
        //{
            GameObject GOlt = GameObject.Find("Point light");
            Light lt = GOlt.GetComponent<Light>();
            if (lt.intensity > 0.6)
            {
                lt.intensity -= 0.2f;
                SliderMove sm = mc.m_CurrentMenu.GetComponent<SliderMove>();
                sm.Decrease();
            }
        //}
    }


    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction; 
        if (_keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(); 
        }
    }
	
}
