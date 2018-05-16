using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMove : MonoBehaviour {
    public GameObject slider;

    public float minPosX = 0f;
    public float maxPosX = 0.3f;

    private float interval = 0.04f; 

    public void Increase()
    {
        Debug.Log("Before x: " + slider.transform.position.x); 
        Move(-interval);
        Debug.Log("After x: " + slider.transform.position.x); 
    }

    public void Decrease()
    {
        Move(interval); 
    }

    private void Move(float distance)
    {
        slider.transform.Translate(new Vector3(distance, 0, 0)); 
    }
}
