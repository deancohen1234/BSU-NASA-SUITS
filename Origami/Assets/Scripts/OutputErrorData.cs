using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputErrorData : MonoBehaviour {

    public Text m_ErrorText;

    public void OutputErrorText(string s)
    {
        m_ErrorText.text += s + "\n";
    }

    public void ClearText()
    {
        m_ErrorText.text = "";
    }
}
