using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System; 

public class TaskManager : MonoBehaviour {

    public List<List<Task>> allTasks = new List<List<Task>>(); 
    public List<Task> disabAlarm = new List<Task>(); 
    public List<Task> reroutPower = new List<Task>();  

    public Texture2D testImage;

    public static TaskManager S; 

	void Start () {
        S = this; 

        populateTasks(); 

        allTasks.Add(disabAlarm);
        allTasks.Add(reroutPower);
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

    private void populateTasks()
    {
        disabAlarm.Add(new Task("1. On the RIGHT side of the EVA kit, locate and use the PANEL ACCESS KEY to unlock the PANEL ACCESS DOOR LOCKS.", null, "CAUTION: The keys are on the tension-spring cable."));
        disabAlarm.Add(new Task("2. Carefully return keys to the side of the EVA kit.", null, null));
        disabAlarm.Add(new Task("3. Insert your fingers in the CENTER OPENING and secure the PANEL ACCESS DOOR in an OPEN position.", null, "WARNING: Door can accidentally close."));
        disabAlarm.Add(new Task("4. On your belt, use the BLUE CARABINEER to securely tether to the TEHTER CABLE inside the STORAGE.", null, "CAUTION: Notice the TETHER CABLE is adjustable."));
        disabAlarm.Add(new Task("5. Locate the E-STOP button and gently press down to temporarily disable the alarm.", null, ""));
        disabAlarm.Add(new Task("6. Locate the FUSIBLE DISCONNECT box and tether the BLUE CARABINEER to the TETHER CABLE.", null, ""));
        disabAlarm.Add(new Task("7. Remove the BLUE CARABINEER from the FUSIBLE DISCONNECT box and transfer it to STORAGE.", null, ""));
        disabAlarm.Add(new Task("8. Open the FUSIBLE DISCONNECT box and secure the lid in the open position.", null, "CAUTION: Pull the locking tab toward STORAGE with the index finger while lifting the cover with the thumb."));
        disabAlarm.Add(new Task("9.Locate the BLACK DISCONNECT and tether if to the TEHTER CABLE.", null, ""));
        disabAlarm.Add(new Task("10. Remove the DISCONNECT and place it in STORAGE.", null, "CAUTION: Pull up with the index and middle fingers while pushing down on the FUSE ACCESS PANEL with the thumb."));
        disabAlarm.Add(new Task("11. Tether the FUSE ACCESS OANEL to the TEHTER CABLE.", null, ""));
        disabAlarm.Add(new Task("12. Remove the FUSE ACCESS PANEL by pulling straight up.", null, ""));
        disabAlarm.Add(new Task("13. Place the FUSE ACCESS PANEL into STORAGE.", null, ""));
        disabAlarm.Add(new Task("14. Tether the ALARM FUSE to the TETHER CABLE.", null, ""));
        disabAlarm.Add(new Task("15. In STORAGE, locate the BLUE FUSE PULLER.", null, ""));
        disabAlarm.Add(new Task("16. Use the BLUE FUSE PULLER to remove ONLY the ALARM FUSE.", null, "CAUTION: Rock the ALARM FUSE with the FUSE PULLER when pulling up."));
        disabAlarm.Add(new Task("17. Return the ALARM FUSE and the FUSE PULLER to STORAGE.", null, ""));
        disabAlarm.Add(new Task("18. In STORAGE, locate the FUSE ACCESS PANEL and reinstall it into the FUSIBLE DISCONNECT box.", null, ""));
        disabAlarm.Add(new Task("19. Remove the FUSE ACCESS PANEL tether from the TETHER CABLE and stow inside.", null, "WARNING: All tethers are under spring tension and can retract quickly."));
        disabAlarm.Add(new Task("20. In STORAGE, locate the DISCONNECT and reinstall it into the FUSIBLE DISCONNECT box.", null, "CAUTION: The DISCONNECT must read 'ON' in the upper right corner to restore conductivity."));
        disabAlarm.Add(new Task("21. Remove the DISCONNECT tether form the TETHER CABLE.", null, "WARNING: All tethers are under spring tension and can retract quickly."));
        disabAlarm.Add(new Task("22. Close the FUSIBLE DISCONNECT box cover.", null, ""));
        disabAlarm.Add(new Task("23. In STORAGE, use the BLUE CARABINEER to clip and lock the FUSIBLE DISCONNECT box cover.", null, ""));
        disabAlarm.Add(new Task("24. Remove the BLUE CARABINEER's tether from the TETHER CABLE.", null, "WARNING: All tethers are under spring tension and can retract quickly."));

        reroutPower.Add(new Task("1. Locate the AUX. POWER INPUT.", null, ""));
        reroutPower.Add(new Task("2. Locate BATTER PACK and tether to TETHER CABLE.", null, ""));
        reroutPower.Add(new Task("3. Undo the BATTERY PACK LEADS from the AUX. POWER INPUT.", null, "CAUTION: Depress the red and black plastic hammers on the side of the AUX. POWER INPUT and pull the leads straight up."));
        reroutPower.Add(new Task("4. Remove BATTERY PACK from AUX.POWER INPUT.", null, ""));
        reroutPower.Add(new Task("5. Locate the ON/OFF switch on the back of the BATTERY PACK and switch it to the OFF position", null, ""));
        reroutPower.Add(new Task("6. Place the BATTERY PACK into STORAGE.", null, ""));
        reroutPower.Add(new Task("7. In STORAGE, find the replacement BATTER PACK.", null, ""));
        reroutPower.Add(new Task("8. Locate the ON/OFF switch on the back of the BATTERY PACK and switch it to the ON position.", null, ""));
        reroutPower.Add(new Task("9. Attach the replacement BATTER PACK onto the AUX. POWER INPUT by the Velcro.", null, ""));
        reroutPower.Add(new Task("10. Insert the BATTERY PACK leads back into the same colored ports.", null, "CAUTION: Depress the red and black plastic hammers on the side fo the AUX. POWER INPUT and push leads straight into their ports."));
        reroutPower.Add(new Task("11. Conduct a GENTLE PUSH TEST on the wires.", null, ""));
        reroutPower.Add(new Task("12. Remove the BATTERY PACK tether from the TETHER CABLE.", null, "WARNING: All tethers are under spring tension and can retract quickly."));
        reroutPower.Add(new Task("13. In STORAGE, locate the GRAY 220 VOLT PLUG.", null, ""));
        reroutPower.Add(new Task("14. Install it into the POWER OUT.", null, "CAUTION: Outlet and plug mate are stiff, ensure the full engagement of the plug into the outlet."));
        reroutPower.Add(new Task("15. Loacte the metal BUSS BAR and verify there are BLACK, GREEN, & WHITE BUSSES, each with 2 openings.", null, ""));
        reroutPower.Add(new Task("16. Insert the WHITE 220 VOLT LEAD into the LEFT WHITE BUSS opening and GENTLY TIGHTEN the thumbscrew.", null, "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("17. Insert the GREEN 220 VOLT LEAD into the LEFT GREEN BUSS opening.", null, "CAUTION: DO NOT overtighten the thumbscrew."));
        reroutPower.Add(new Task("18. Insert the BLACK 220 VOLT LEAD into the LEFT BLACK BUSS opening.", null, "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("19. Make sure the METAL LEADS are not sticking out the BACK of the BUSS BAR.", null, ""));
        reroutPower.Add(new Task("20. Cunduct a GENTLE PULL TEST on each cable.", null, ""));
        reroutPower.Add(new Task("21. In STORAGE, locate the 110 VOLT PLUG and install it in POWER IN.", null, "CAUTION: Lift cover with one hand while installing PLUG into the outlet with the other. The lid is spring-loaded."));
        reroutPower.Add(new Task("22. Insert the WHITE 110 VOLT PLUG LEAD into the RIGHT WHITE BUSS opening.", null, "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("23. Insert the GREEN 110 VOLT PLUG LEAD into the RIGHT GREEN BUS opening.", null, "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("24. Insert the BLACK 110 VOLT PLUB LEAD into the RIGHT BLACK BUSS opening.", null, "CAUTION: DO NOT over tighten the thumbscrew."));
        reroutPower.Add(new Task("25. Conduct a GENTLE PULL TEST on each cable.", null, ""));
        reroutPower.Add(new Task("26. In STORAGE, locate the E-STOP KEY.", null, ""));
        reroutPower.Add(new Task("27. Insert the KEY into the E-STOP and TURN to the RIGHT and button will pop up.", null, ""));
        reroutPower.Add(new Task("28. Remove the KEY and place it in STORAGE.", null, ""));
        reroutPower.Add(new Task("29. Locate the AUX. POWER SWITCH on the POWER IN box and switch it to the 'ON' position.", null, ""));
        reroutPower.Add(new Task("30. Can you please confirm YSE or NO that the SYSTEM GO indicator light is GREEN?", null, ""));
    }
}
