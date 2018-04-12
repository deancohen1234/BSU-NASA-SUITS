using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech; 

public class VoiceManager : MonoBehaviour {
    private KeywordRecognizer _keywordRecognizer = null; 
    private readonly Dictionary<string, System.Action> _keywords = new Dictionary<string, System.Action>();
    private bool _visible = false;

    public GameObject curMenu; 

    public GameObject mainMenu;
    public GameObject menuController;
    private MenuController mc; 

	// Use this for initialization
	void Start () { 
        _keywords.Add("Menu", Menu);
        _keywords.Add("Next", Next); 

        _keywordRecognizer = new KeywordRecognizer(_keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();

        curMenu = null;
        mc = menuController.GetComponent<MenuController>();
    }

    private void Menu()
    {
        ToggleVisibility(mainMenu); 
    }

    private void Next()
    {
        if (curMenu.Equals(mainMenu))
        {
            mc.ChangeMenu(curMenu, mc.m_NextMenu); 
        }
    }

    private void ToggleVisibility(GameObject holoMenu)
    {
        //var holoMenu = GameObject.Find("SuitsMenuTwo");
        if (holoMenu == null) return;

        _visible = !_visible;
        //var rend = holoMenu.GetComponent<Renderer>();
        //if (rend != null) rend.enabled = _visible;

        holoMenu.SetActive(true); 

        holoMenu.transform.localScale = Vector3.one * 0.3f;
        holoMenu.transform.position =
            Camera.main.transform.position + Camera.main.transform.forward;

        Vector3 cameraPos = Camera.main.transform.position;

        holoMenu.transform.LookAt(Camera.main.transform);
        holoMenu.transform.eulerAngles += new Vector3(-90, 0, 0); 

        curMenu = holoMenu; 
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
