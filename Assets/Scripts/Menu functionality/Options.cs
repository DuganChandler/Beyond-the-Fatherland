using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Options : MonoBehaviour
{   
    public GameObject optionsMenuUI;
    public GameObject StartingButton;
    public bool optionOpen = true;
    public Mainmenu Mainmenu;
    public PauseMenu PauseMenu;

    public void openOpps()
    {
        
        optionsMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(StartingButton);
        Debug.Log("The options Menue is open");
        optionOpen = true;

    }

    public void stopOpps()
    {
        optionsMenuUI.SetActive(false); ;
        Debug.Log("The options Menue is closed");
        optionOpen = false;
        if (PauseMenu.IsGamePaused == true)
        {
            EventSystem.current.SetSelectedGameObject(PauseMenu.firseButton);
        }
        else { EventSystem.current.SetSelectedGameObject(Mainmenu.StartinngButton); }
        
    }

    public void volume ()
    {
        Debug.Log("Volume is volumeing");
    }



}
