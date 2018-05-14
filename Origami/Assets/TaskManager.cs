using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System; 

public class TaskManager : MonoBehaviour {

    public List<string> taskArray;
    public List<Texture2D> textureArray;
    public List<string> warningArray; 

    public Texture2D testImage;

    public static TaskManager S; 

	void Start () {
        S = this; 

        for (int i = 0; i < 10; i++)
        {
            string text = "Step " + i;
            taskArray.Add(text);

            textureArray.Add(testImage); 
        }
        

        //Debug.Log(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)); 
	}

    public string getText(int step)
    {
        return taskArray[step-1]; 
    }

    public Texture2D getImage(int step)
    {
        return textureArray[step-1]; 
    }
}
