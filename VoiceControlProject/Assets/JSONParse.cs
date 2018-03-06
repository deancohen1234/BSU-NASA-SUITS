using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONParse : MonoBehaviour {

    public string m_JSONString;
	// Use this for initialization
	void Start ()
    {
        //NASADataType jsonObject = JsonUtility.FromJson<NASADataType>(m_JSONString);
        NASADataType jsonObject = JsonUtility.FromJson<NASADataType>(m_JSONString);
        //Debug.Log(jsonObject);
        Debug.Log(jsonObject.p_sop);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

[System.Serializable]
public class NASADataType
{
    public int p_sub = 0;
    public int t_sub = 0;
    public int v_fan = 0;
    public string t_eva = "";
    public int p_o2 = 0;
    public float rate_o2 = 0.0f;
    public int cap_battery = 0;
    public int p_h2o_g = 0;
    public int p_h2o_l = 0;
    public int p_sop = 0;
    public float rate_sop = 0.0f;
}

public class TestClass
{
    public string _id;
}
