using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceManager : MonoBehaviour {

	KeywordRecognizer keywordRecognizer;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
	// Use this for initialization
	void Start () 
	{
	//Create keywords for keyword recognizer
		keywords.Add("computer", () =>
		{
				Debug.Log("I understand and hear you! God you are annoying");
			// action to be performed when this keyword is spoken
		});

		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

		keywordRecognizer.Start ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		Debug.Log ("Key Word Recognized");
		System.Action keywordAction;
		// if the keyword recognized is in our dictionary, call that Action.
		if (keywords.TryGetValue(args.text, out keywordAction))
		{
			keywordAction.Invoke();
		}
	}
}
