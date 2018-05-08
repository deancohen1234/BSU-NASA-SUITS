using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; 

public class JSONParse : MonoBehaviour {

    public string m_JSONString;
    public string url = "http://localhost/api/recent";
    public const int OBJECTID_LENGTH = 47;

    public Text bioText; 

    [Space(10)]

    [Header("Ranges")]

    [Space(5)]
    public float m_HeartRateLow;
    public float m_HeartRateHigh;

    [Space(5)]
    public float m_P_SuitLow;
    public float m_P_SuitHigh;

    [Space(5)]
    public float m_P_SubLow;
    public float m_P_SubHigh;

    [Space(5)]
    public float m_T_SubLow;
    public float m_T_SubHigh;

    [Space(5)]
    public float m_V_FanLow;
    public float m_V_FanHigh;

    [Space(5)]
    public float m_P_O2Low;
    public float m_P_O2High;

    [Space(5)]
    public float m_Rate_O2Low;
    public float m_Rate_O2High;

    [Space(5)]
    public float m_Cap_BatteryLow;
    public float m_Cap_BatteryHigh;

    [Space(5)]
    public float m_P_H2O_GLow;
    public float m_P_H2O_GHigh;

    [Space(5)]
    public float m_P_H2O_LLow;
    public float m_P_H2O_LHigh;

    [Space(5)]
    public float m_P_SOPLow;
    public float m_P_SOPHigh;

    [Space(5)]
    public float m_Rate_SOPLow;
    public float m_Rate_SOPHigh;


    // Use this for initialization
    void Start ()
    {
        //StartCoroutine(RunWWW()); 
        //NASADataType jsonObject = JsonUtility.FromJson<NASADataType>(m_JSONString);
        InvokeRepeating("UpdateSystemData", 1, 5);
	}

    private void UpdateSystemData()
    {
        StartCoroutine(RunWWW());
    }
    
    IEnumerator RunWWW()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            string json = ""; 
            if (www.isNetworkError)
            {
                bioText.text = "NETWORK ERROR Not connected to server :(\n"; 
                bioText.text += www.error; 
                
            } else if (www.isHttpError)
            {
                bioText.text = "HTTP ERROR Not connected to server :( :( :(";
                bioText.text += www.error; 
            } else 
            {
                bioText.text = "Connected to server!"; 
                json = RemoveBrackets(www.downloadHandler.text);
            }

            Debug.Log(json);
            NASADataType jsonObject = JsonUtility.FromJson<NASADataType>(json);

            Debug.Log(jsonObject.heart_bpm);
            string bioString = "";
            bioString += "Heart Rate: " + jsonObject.heart_bpm.ToString() + " bpm\n";
            bioString += "Suit Pressure: " + jsonObject.p_suit.ToString() + " psid\n";
            bioString += "External Enviornment Pressure: " + jsonObject.p_sub.ToString() + " psia\n";
            bioString += "External Envionrment Temperature: " + jsonObject.t_sub.ToString() + " °F\n";
            bioString += "Fan Rotation Speed: " + jsonObject.v_fan.ToString() + " rpm\n";
            bioString += "Primary Oxygen Tank Pressure: " + jsonObject.p_o2.ToString() + " psia\n";
            bioString += "Battery Capacity: " + jsonObject.cap_battery.ToString() + " A-hr\n";
            bioString += "H20 Gas Pressure: " + jsonObject.p_h2o_g.ToString() + " psia\n";
            bioString += "H20 Liquid Pressure: " + jsonObject.p_h2o_l.ToString() + " psia\n";
            bioString += "Secondary Oxygen Pack Pressure: " + jsonObject.p_sop.ToString() + " psia\n";
            bioString += "Flow Rate of Secondary Oxygen Pack: " + jsonObject.rate_sop.ToString() + " psi\n";
            bioString += "Time Life Battery: " + jsonObject.t_battery + "\n";
            bioString += "Time Life Oxygen: " + jsonObject.t_oxygen + "\n";
            bioString += "Time Life Water: " + jsonObject.t_water + "\n"; 
            bioText.text += bioString; 
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void CheckAllRanges(NASADataType ndt)
    {
        if (CheckValueRange(ndt.heart_bpm, m_HeartRateLow, m_HeartRateLow)) Debug.Log("Heart Rate going hardcore");
        if (CheckValueRange(ndt.p_suit, m_P_SuitLow, m_P_SuitHigh)) Debug.Log("Suit Pressure Bad, fix it");
        if (CheckValueRange(ndt.p_sub, m_P_SubLow, m_P_SubHigh)) Debug.Log("Outside Pressure Crazy, the world is ending");
        if (CheckValueRange(ndt.t_sub, m_T_SubLow, m_T_SubHigh)) Debug.Log("Outside Temperature is Crazy, the universe is probably breaking");
        if (CheckValueRange(ndt.v_fan, m_V_FanLow, m_V_FanHigh)) Debug.Log("Fan Rotation not right, get working on spinning that out");
        if (CheckValueRange(ndt.p_o2, m_P_O2Low, m_P_O2High)) Debug.Log("O2 pressure not right, breathe drastically and heavily");
        if (CheckValueRange(ndt.rate_o2, m_Rate_O2Low, m_Rate_O2High)) Debug.Log("O2 rate is wrong, the flow ain't flowing");
        if (CheckValueRange(ndt.cap_battery, m_Cap_BatteryLow, m_Cap_BatteryHigh)) Debug.Log("Battery is low, lights are about to go out");
        if (CheckValueRange(ndt.p_h2o_g, m_P_H2O_GLow, m_P_H2O_GHigh)) Debug.Log("Gas of H2O pressure is bad, that's probably not good");
        if (CheckValueRange(ndt.p_h2o_l, m_P_H2O_LLow, m_P_H2O_LHigh)) Debug.Log("Liquid of H2O pressure is bad, woops...");
        if (CheckValueRange(ndt.p_sop, m_P_SOPLow, m_P_SOPHigh)) Debug.Log("SOP is bad, thats no good");
        if (CheckValueRange(ndt.rate_sop, m_Rate_SOPLow, m_Rate_SOPHigh)) Debug.Log("Rate of SOP is bad, sorry about that");
    }

    private string CleanUpJSON(string json)
    {
        string newJson = json.Remove(1,OBJECTID_LENGTH);
        Debug.Log(newJson);

        return newJson;
    }

    private string RemoveBrackets(string json)
    {
        string newJson = json;
        newJson = newJson.Remove(newJson.Length - 1);
        newJson = newJson.Remove(0, 1);

        return newJson;
    }

    private bool CheckValueRange(float value, float lowRange, float highRange)
    {
        bool b = false;
        if (value < highRange && value > lowRange)
        {
            b = true;
            Debug.Log("Value is good. You good my dude, or dudette");
        }

        return b;
    }
}

[System.Serializable]
public class NASADataType
{
    public string create_date = "";
    public int heart_bpm = 0;
    public float p_suit = 0;
    public float p_sub = 0;
    public int t_sub = 0;
    public int v_fan = 0;
    public int p_o2 = 0;
    public float rate_o2 = 0.0f;
    public int cap_battery = 0;
    public int p_h2o_g = 0;
    public int p_h2o_l = 0;
    public int p_sop = 0;
    public float rate_sop = 0.0f;
    public string t_battery = "";
    public string t_oxygen = "";
    public string t_water = "";
}

public class TestClass
{
    public string _id;
}
