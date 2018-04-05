using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomButton : MonoBehaviour
{
    public MenuController m_MenuController;
    public GameObject m_DestinationMenu;

    public void OnSelected ()
    {
        m_MenuController.ChangeMenu(gameObject.transform.parent.gameObject, m_DestinationMenu);
    }

    public void OnBack()
    {
        m_MenuController.GoBack();
    }
}
