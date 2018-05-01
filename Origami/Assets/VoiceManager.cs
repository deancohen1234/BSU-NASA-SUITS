using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

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

    // this should go away later
    public GameObject brightMenu; 

    // Use this for initialization
    void Start () { 
        _keywords.Add("Menu", Menu);
        _keywords.Add("Move", Menu); 
        _keywords.Add("Next", Next);
        _keywords.Add("Go", Next); 
        _keywords.Add("Back", Back);
        _keywords.Add("Reset", ResetScene);
        _keywords.Add("Clear", ResetScene); 
        _keywords.Add("Increase", Increase);
        _keywords.Add("Decrease", Decrease);
        _keywords.Add("Brightness", Brightness); 

        // _keywords.Add("current task", currentTask); 
        // _keywords.Add("task list", taskList); 
        // _keywords.Add("biometrics", biometrics); 
        // _keywords.Add("help", help); 
        // _keywords.Add("settings", settings); 
        // _keywords.Add("sos", sos); 



        _keywordRecognizer = new KeywordRecognizer(_keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();

        curMenu = null;

        mc = FindObjectOfType(typeof(MenuController)) as MenuController;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Next();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Menu();
        }

        

    }

    private void Menu()
    {
        ToggleVisibility(mc.m_CurrentMenu);

        m_Source.clip = m_OpenMenu;
        m_Source.Play();
    }

    private void Next()
    {

        //mc.ChangeMenu(mc.m_CurrentMenu, mc.m_NextMenu); 
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

        //mc.ChangeMenu(mc.m_CurrentMenu, mc.m_NextMenu); 
        mc.PreviousTask();

        m_Source.clip = m_BackButton;
        m_Source.Play();
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

    private void Brightness()
    {
        ToggleVisibility(brightMenu);
    }

    // Helper functions // 
    private void ToggleVisibility(GameObject holoMenu)
    {
        //var holoMenu = GameObject.Find("SuitsMenuTwo");
        if (holoMenu == null) return;

        if (mc.m_CurrentMenu != null)
        {
            mc.m_CurrentMenu.SetActive(false);
        }

        _visible = !_visible;
        //var rend = holoMenu.GetComponent<Renderer>();
        //if (rend != null) rend.enabled = _visible;

        holoMenu.SetActive(true);

        holoMenu.transform.position =
            Camera.main.transform.position + (Camera.main.transform.forward);

        Vector3 cameraPos = Camera.main.transform.position;

        holoMenu.transform.rotation = Quaternion.Euler(new Vector3(holoMenu.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, holoMenu.transform.eulerAngles.z));

        holoMenu.transform.position += new Vector3(0, .125f, 0);

        mc.m_CurrentMenu = holoMenu; 
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
