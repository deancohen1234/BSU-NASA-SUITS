using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System;
using UnityEngine.Networking;

/// <summary>
/// Manages all tasks. Tasks are currently hard-coded in; however, we are working on a solution to pull them from a server. 
/// </summary>
public class TaskManager : MonoBehaviour {

    // Singleton 
    public static TaskManager S;

    // List of all steps organized by procedure 
    public List<List<Task>> allTasks = new List<List<Task>>(); 

    // Lists of individual steps 
    public List<Task> disabAlarm = new List<Task>(); 
    public List<Task> reroutPower = new List<Task>();
    public List<Task> lightSwitch = new List<Task>(); 

    // Static Array of images for each task 
    public Texture2D[] images; 

    // Web Connection 
    public string url = "";
    private OutputErrorData m_OutputErrorData;

    // For testing 
    //public GameObject cube; 

    void Start () {
        S = this; 

        populateTasks(); 

        allTasks.Add(disabAlarm);
        allTasks.Add(reroutPower);
        allTasks.Add(lightSwitch); 

        m_OutputErrorData = FindObjectOfType<OutputErrorData>();
        InvokeRepeating("UpdateSystemData", 1, 5);
    }

    public string getStep(int task, int step)
    {
        string retval; 
        try
        {
            retval = allTasks[task - 1][step - 1].task; 
        } catch (ArgumentOutOfRangeException)
        {
            retval = ""; 
        }
        return retval; 
    }

    public Texture2D getPic(int task, int step)
    {
        Texture2D retval;
        try
        {
            retval = allTasks[task - 1][step - 1].picture;
        }
        catch (ArgumentOutOfRangeException)
        {
            retval = null;
        }
        return retval;
    }

    public string getWarning(int task, int step)
    {
        string retval; 
        try
        {
            retval = allTasks[task - 1][step - 1].warning; 
        } catch (ArgumentOutOfRangeException)
        {
            retval = ""; 
        }
        return retval; 
    }

    // Server Connection Stuffs 
    private void UpdateSystemData()
    {
        StartCoroutine(RunWWW());
    }

    IEnumerator RunWWW()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + "task.txt"))
        {
            yield return www.SendWebRequest();

            string fromServer = "";
            if (www.isNetworkError)
            {
                Debug.Log("NETWORK ERROR Not connected to tasklist server :(");
            }
            else if (www.isHttpError)
            {
                Debug.Log("HTTP ERROR Not connected to tasklist server :(");
            }
            else
            {
                //Debug.Log("Connected to tasklist server"); 
                fromServer = www.downloadHandler.text;
                //Debug.Log(fromServer);
                string[] splitString = fromServer.Split('\n');

                Step testStep = new Step();
                testStep.text = "Hi";
                testStep.image = "ImagePath";
                testStep.warning = "You have been warned"; 
                string json = JsonUtility.ToJson(testStep);
                //Debug.Log("JSON: " + json);

                

         
                foreach (string str in splitString)
                {
                    Step newStep = JsonUtility.FromJson<Step>(str);
                    if (newStep == null){ continue; }
                    //Debug.Log(newStep.text);
                    string imagePath = newStep.image;

                    StartCoroutine(getImage(imagePath)); 

                }
            }
        }
    }

    IEnumerator getImage(string path)
    {
         
        using (UnityWebRequest www = UnityWebRequest.Get(url + path))
        {
            
            yield return www.SendWebRequest();

           // Debug.Log("URL: " + url + path);
            byte[] rawImage = www.downloadHandler.data;
           // Debug.Log(rawImage);
            Texture2D tx = new Texture2D(1, 1);
            tx.LoadImage(rawImage);

            //cube.GetComponent<Renderer>().material.mainTexture = tx; 
        }
    }

            /*   private Texture2D getImage(string path)
               {
                   Debug.Log("In getImage"); 
                   using (UnityWebRequest www = UnityWebRequest.Get(url + path))
                   {
                       Debug.Log("URL: " + url + path);
                       //byte[] rawImage = www.downloadHandler.data;
                       string rawImage = www.downloadHandler.text;
                       Debug.Log(rawImage); 
                       //Debug.Log("Raw Image: " + rawImage.Length); 
                       //Texture2D tx = new Texture2D(1, 1);
                       //tx.LoadImage(rawImage);

                       //cube.GetComponent<Renderer>().material.mainTexture = tx; 

                       return null; 

                   }
               }  */


     // Staticly creates all tasks 
    private void populateTasks()
    {
        disabAlarm.Add(new Task("1. On the RIGHT side of the EVA kit, locate and use the PANEL ACCESS KEY to unlock the PANEL ACCESS DOOR LOCKS.", images[0], "CAUTION: The keys are on the tension-spring cable."));
        disabAlarm.Add(new Task("2. Carefully return keys to the side of the EVA kit.", null, null));
        disabAlarm.Add(new Task("3. Insert your fingers in the CENTER OPENING and secure the PANEL ACCESS DOOR in an OPEN position.", images[1], "WARNING: Door can accidentally close."));
        disabAlarm.Add(new Task("4. On your belt, use the BLUE CARABINEER to securely tether to the TEHTER CABLE inside the STORAGE.", images[2], "CAUTION: Notice the TETHER CABLE is adjustable."));
        disabAlarm.Add(new Task("5. Locate the E-STOP button and gently press down to temporarily disable the alarm.", images[3], ""));
        disabAlarm.Add(new Task("6. Locate the FUSIBLE DISCONNECT box and tether the BLUE CARABINEER to the TETHER CABLE.", null, ""));
        disabAlarm.Add(new Task("7. Remove the BLUE CARABINEER from the FUSIBLE DISCONNECT box and transfer it to STORAGE.", images[4], ""));
        disabAlarm.Add(new Task("8. Open the FUSIBLE DISCONNECT box and secure the lid in the open position.", images[5], "CAUTION: Pull the locking tab toward STORAGE with the index finger while lifting the cover with the thumb."));
        disabAlarm.Add(new Task("9.Locate the BLACK DISCONNECT and tether if to the TEHTER CABLE.", null, ""));
        disabAlarm.Add(new Task("10. Remove the DISCONNECT and place it in STORAGE.", images[6], "CAUTION: Pull up with the index and middle fingers while pushing down on the FUSE ACCESS PANEL with the thumb."));
        disabAlarm.Add(new Task("11. Tether the FUSE ACCESS OANEL to the TEHTER CABLE.", null, ""));
        disabAlarm.Add(new Task("12. Remove the FUSE ACCESS PANEL by pulling straight up.", images[7], ""));
        disabAlarm.Add(new Task("13. Place the FUSE ACCESS PANEL into STORAGE.", null, ""));
        disabAlarm.Add(new Task("14. Tether the ALARM FUSE to the TETHER CABLE.", null, ""));
        disabAlarm.Add(new Task("15. In STORAGE, locate the BLUE FUSE PULLER.", null, ""));
        disabAlarm.Add(new Task("16. Use the BLUE FUSE PULLER to remove ONLY the ALARM FUSE.", images[8], "CAUTION: Rock the ALARM FUSE with the FUSE PULLER when pulling up."));
        disabAlarm.Add(new Task("17. Return the ALARM FUSE and the FUSE PULLER to STORAGE.", null, ""));
        disabAlarm.Add(new Task("18. In STORAGE, locate the FUSE ACCESS PANEL and reinstall it into the FUSIBLE DISCONNECT box.", images[9], ""));
        disabAlarm.Add(new Task("19. Remove the FUSE ACCESS PANEL tether from the TETHER CABLE and stow inside.", null, "WARNING: All tethers are under spring tension and can retract quickly."));
        disabAlarm.Add(new Task("20. In STORAGE, locate the DISCONNECT and reinstall it into the FUSIBLE DISCONNECT box.", images[10], "CAUTION: The DISCONNECT must read 'ON' in the upper right corner to restore conductivity."));
        disabAlarm.Add(new Task("21. Remove the DISCONNECT tether form the TETHER CABLE.", null, "WARNING: All tethers are under spring tension and can retract quickly."));
        disabAlarm.Add(new Task("22. Close the FUSIBLE DISCONNECT box cover.", null, ""));
        disabAlarm.Add(new Task("23. In STORAGE, use the BLUE CARABINEER to clip and lock the FUSIBLE DISCONNECT box cover.", images[11], ""));
        disabAlarm.Add(new Task("24. Remove the BLUE CARABINEER's tether from the TETHER CABLE.", null, "WARNING: All tethers are under spring tension and can retract quickly."));

        reroutPower.Add(new Task("1. Locate the AUX. POWER INPUT.", null, ""));
        reroutPower.Add(new Task("2. Locate BATTER PACK and tether to TETHER CABLE.", null, ""));
        reroutPower.Add(new Task("3. Undo the BATTERY PACK LEADS from the AUX. POWER INPUT.", images[12], "CAUTION: Depress the red and black plastic hammers on the side of the AUX. POWER INPUT and pull the leads straight up."));
        reroutPower.Add(new Task("4. Remove BATTERY PACK from AUX.POWER INPUT.", null, ""));
        reroutPower.Add(new Task("5. Locate the ON/OFF switch on the back of the BATTERY PACK and switch it to the OFF position", images[13], ""));
        reroutPower.Add(new Task("6. Place the BATTERY PACK into STORAGE.", null, ""));
        reroutPower.Add(new Task("7. In STORAGE, find the replacement BATTER PACK.", null, ""));
        reroutPower.Add(new Task("8. Locate the ON/OFF switch on the back of the BATTERY PACK and switch it to the ON position.", null, ""));
        reroutPower.Add(new Task("9. Attach the replacement BATTER PACK onto the AUX. POWER INPUT by the Velcro.", null, ""));
        reroutPower.Add(new Task("10. Insert the BATTERY PACK leads back into the same colored ports.", null, "CAUTION: Depress the red and black plastic hammers on the side fo the AUX. POWER INPUT and push leads straight into their ports."));
        reroutPower.Add(new Task("11. Conduct a GENTLE PUSH TEST on the wires.", null, ""));
        reroutPower.Add(new Task("12. Remove the BATTERY PACK tether from the TETHER CABLE.", images[14], "WARNING: All tethers are under spring tension and can retract quickly."));
        reroutPower.Add(new Task("13. In STORAGE, locate the GRAY 220 VOLT PLUG.", null, ""));
        reroutPower.Add(new Task("14. Install it into the POWER OUT.", null, "CAUTION: Outlet and plug mate are stiff, ensure the full engagement of the plug into the outlet."));
        reroutPower.Add(new Task("15. Loacte the metal BUSS BAR and verify there are BLACK, GREEN, & WHITE BUSSES, each with 2 openings.", null, ""));
        reroutPower.Add(new Task("16. Insert the WHITE 220 VOLT LEAD into the LEFT WHITE BUSS opening and GENTLY TIGHTEN the thumbscrew.", images[15], "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("17. Insert the GREEN 220 VOLT LEAD into the LEFT GREEN BUSS opening.", images[16], "CAUTION: DO NOT overtighten the thumbscrew."));
        reroutPower.Add(new Task("18. Insert the BLACK 220 VOLT LEAD into the LEFT BLACK BUSS opening.", images[17], "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("19. Make sure the METAL LEADS are not sticking out the BACK of the BUSS BAR.", null, ""));
        reroutPower.Add(new Task("20. Cunduct a GENTLE PULL TEST on each cable.", null, ""));
        reroutPower.Add(new Task("21. In STORAGE, locate the 110 VOLT PLUG and install it in POWER IN.", images[18], "CAUTION: Lift cover with one hand while installing PLUG into the outlet with the other. The lid is spring-loaded."));
        reroutPower.Add(new Task("22. Insert the WHITE 110 VOLT PLUG LEAD into the RIGHT WHITE BUSS opening.", images[19], "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("23. Insert the GREEN 110 VOLT PLUG LEAD into the RIGHT GREEN BUS opening.", images[20], "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("24. Insert the BLACK 110 VOLT PLUB LEAD into the RIGHT BLACK BUSS opening.", images[21], "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("25. Conduct a GENTLE PULL TEST on each cable.", images[22], ""));
        reroutPower.Add(new Task("26. In STORAGE, locate the E-STOP KEY.", images[23], ""));
        reroutPower.Add(new Task("27. Insert the KEY into the E-STOP and TURN to the RIGHT and button will pop up.", null, ""));
        reroutPower.Add(new Task("28. Remove the KEY and place it in STORAGE.", images[24], ""));
        reroutPower.Add(new Task("29. Locate the AUX. POWER SWITCH on the POWER IN box and switch it to the 'ON' position.", null, ""));
        reroutPower.Add(new Task("30. Can you please confirm YSE or NO that the SYSTEM GO indicator light is GREEN?", null, ""));

        lightSwitch.Add(new Task("1. Locate the Aux. Power Switch", images[25], ""));
        lightSwitch.Add(new Task("2. Flip Aux. power switch to OFF position", images[26], ""));
        lightSwitch.Add(new Task("3. Locate Power Outlet Lock Key tethered to Tether Ubolt", images[27], ""));
        lightSwitch.Add(new Task("4. Locate Power Outlet Lock", images[28], ""));
        lightSwitch.Add(new Task("5. Unlock Power Outlet Lock with Power Outlet Lock Key", images[29], ""));
        lightSwitch.Add(new Task("6. Retether Power Outlet Lock Key and Power Outlet Lock to Tether Ubolt", images[30], ""));
        lightSwitch.Add(new Task("7. Above Power Outlets, locate the Power Plug", images[31], ""));
        lightSwitch.Add(new Task("8. Locate the  Power Outlets inside the weather container.", images[32], ""));
        lightSwitch.Add(new Task("9. Install Power Plug into Power Outlet", images[33], ""));
        lightSwitch.Add(new Task("10. Close weather container", images[34], ""));
        lightSwitch.Add(new Task("11. Insert volt lead in the right silver buss opening and gently tighten the thumb screw", images[35], "CAUTION: Ensure lead is completely inside the buss before tightening"));
        lightSwitch.Add(new Task("12. Locate Wrench Tethered to Tether Ubolt", images[36], ""));
        lightSwitch.Add(new Task("13. Insert the volt lead in the right terminal block and tighten the nut using the wrench", images[37], "CAUTION: Ensure lead is completely inside the buss before tightening"));
        lightSwitch.Add(new Task("14. Insert the lead in the left silver buss opening and gently tighten the thumb screw", images[38], "CAUTION: Ensure lead is completely inside the buss before tightening"));
        lightSwitch.Add(new Task("15. Insert the lead in the Left Terminal Block and tighten the nut using the wrench", images[39], "CAUTION: Ensure lead is completely inside the buss before tightening"));
        lightSwitch.Add(new Task("16. Conduct a gentle pull test on each", images[40], ""));  
        lightSwitch.Add(new Task("17. Once secure retether Wrench to Tether Ubolt.", images[41], "CAUTION: Ensure lead is completely inside the buss before tightening"));
        lightSwitch.Add(new Task("18. Locate the AUX power switch and switch it to the ON position", images[42], "CAUTION: Ensure lead is completely inside the buss before tightening"));
        lightSwitch.Add(new Task("19. Conform that LED indicator light is GREEN", images[43], ""));
        lightSwitch.Add(new Task("Congratulations! You have completed the task.", images[44], ""));
    }
}

// Used for parsing JSON from server 
[System.Serializable]
public class Step
{
    public string text;
    public string image;
    public string warning; 
}
