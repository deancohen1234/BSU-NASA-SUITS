using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject m_OGMenu;
    public GameObject m_NextMenu;

	public void ChangeMenu(GameObject oldMenu, GameObject newMenu)
    {
        newMenu.transform.position = oldMenu.transform.position;
        newMenu.transform.rotation = Camera.main.transform.rotation;

        oldMenu.SetActive(false);
        newMenu.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeMenu(m_OGMenu, m_NextMenu);
        }
    }
}
