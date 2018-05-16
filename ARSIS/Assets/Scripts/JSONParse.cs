using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; 

public class JSONParse : MonoBehaviour {

    public string m_JSONString;
    public string url = "http://localhost/api/recent";
    public string switchUrl = "";
    public const int OBJECTID_LENGTH = 47;

    private OutputErrorData m_OutputErrorData;

    public Text bioText;
    public Text oxygenText;
    public Text batteryText; 

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

    void Start ()
    {
        m_OutputErrorData = FindObjectOfType<OutputErrorData>();
        InvokeRepeating("UpdateSystemData", 1, 5);
	}

    private void UpdateSystemData()
    {
        StartCoroutine(RunWWW());
        StartCoroutine(RunSwitchWWW());
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
                //json = RemoveBrackets(www.downloadHandler.text);
                json = www.downloadHandler.text;
            }

            NASADataType jsonObject = JsonUtility.FromJson<NASADataType>(json);
            
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
            //bioString += "Time Life Battery: " + jsonObject.t_battery + "\n";
            //bioString += "Time Life Oxygen: " + jsonObject.t_oxygen + "\n";
            bioString += "Time Life Water: " + jsonObject.t_water + "\n"; 
            bioText.text = bioString;

            oxygenText.text = "Oxygen: " + jsonObject.t_oxygen.ToString();
            batteryText.text = "Battery: " + jsonObject.t_battery.ToString(); 
        }
    }

    IEnumerator RunSwitchWWW()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(switchUrl))
        {
            yield return www.SendWebRequest();

            string json = "";
            if (www.isNetworkError)
            {
                bioText.text = "NETWORK ERROR Not connected to server :(\n";
                bioText.text += www.error;

            }
            else if (www.isHttpError)
            {
                bioText.text = "HTTP ERROR Not connected to server :( :( :(";
                bioText.text += www.error;
            }
            else
            {
                //bioText.text = "Connected to server!";
                //json = RemoveBrackets(www.downloadHandler.text);
                json = www.downloadHandler.text;

            }

           // Debug.Log(json);
            NASADataTypeSwitch jsonObject = JsonUtility.FromJson<NASADataTypeSwitch>(json);

            CheckSuitSwitches(jsonObject);
        }
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

    private void CheckSuitSwitches(NASADataTypeSwitch ndts)
    {
        m_OutputErrorData.ClearText();
        
        if (ndts.h2o_off == "true") m_OutputErrorData.OutputErrorText("H2O IS OFF");
        if (ndts.sspe == "true") m_OutputErrorData.OutputErrorText("SUIT P EMERG");
        if (ndts.fan_error == "true") m_OutputErrorData.OutputErrorText("FAN SW OFF");
        if (ndts.vent_error == "true") m_OutputErrorData.OutputErrorText("NO VENT FLOW"); // Add vent rpms 
        if (ndts.vehicle_power == "true") m_OutputErrorData.OutputErrorText("VEHICLE POWER AVAIL");
        if (ndts.o2_off == "true") m_OutputErrorData.OutputErrorText("O2 IS OFF");
    }

    private string CleanUpJSON(string json)
    {
        string newJson = json.Remove(1,OBJECTID_LENGTH);

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
           // Debug.Log("Value is good. You good my dude, or dudette");
        }

        return b;
    }
}


//////////////////////// All telemetry variables are defined here /////////////////////////////////
[System.Serializable]
public class NASADataType
{
    public string create_date = "";
    public int heart_bpm = 0;
    public float p_suit = 0;
    public float p_sub = 0;
    public int t_sub = 0;
    public int v_fan = 0;  // fan speed 
    public int p_o2 = 0;
    public float rate_o2 = 0.0f;
    public int cap_battery = 0;
    public int p_h2o_g = 0;
    public int p_h2o_l = 0;    // if the delta between _g and _l is more than 3, then water quantity low 
    public int p_sop = 0;
    public float rate_sop = 0.0f;
    public string t_battery = "";
    public string t_oxygen = "";
    public string t_water = "";
}

public class NASADataTypeSwitch
{
    public string create_date = "";
    public string sop_on = "";   // SOP 02 ON TIME LF XX:XX   - secondary oxygen system on - meaning primary system is depleted 
    public string sspe = "";  // SUIT P EMERG    - out of oxygen or regulator is not working 
    public string fan_error = ""; // FAN SW OFF   - 
    public string vent_error = ""; // NO VENT FLOW  - <v_fan> rpm  
    public string vehicle_power = ""; // VEHICLE POWER AVAIL   - you should switch to save suit power 
    public string h2o_off = ""; // H20 IS OFF  - for cooling inside the suit 
    public string o2_off = "";  // O2 IS OFF 

    // BAT VDC LOW / VAT VDC XX.X - if battery is under 15 V 

    // public bool p_sop = false; // SOP P LOW   SOP P <p_sop>  SOP RATE <rate_sop>  
    // triggered when O2 rate is greater than 10.2 psi/min -  O2 USE HIGH O2 RATE <rate_O2> 

    // public bool low_voltage = false;   // BAT VDC LOW    BAT VDC <t_battery> V 

    // water gas pressure - WATER QUANTITY LOW 
}
