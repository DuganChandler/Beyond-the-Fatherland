using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenuUI;
    public GameObject firseButton;
    public Mainmenu Mainmenu;
    // Start is called before the first frame update
    public bool IsGamePaused = false;
   



    // Update is called once per frame
   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (IsGamePaused )
            {
                Continue();
                
                Debug.Log("The game continues");
            }
            else
            {
                Pause();
                Debug.Log("game is pasued");
                ;
            }
        }
    }
    public void Continue()
    {
        pauseMenuUI.SetActive(false);
        
        IsGamePaused = false;
        EventSystem.current.SetSelectedGameObject(Mainmenu.StartinngButton);
        //replace the Mainmenu.starting button with the starting button on the scene 
        //for example the first character profile

    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        
        IsGamePaused = true;
        EventSystem.current.SetSelectedGameObject(firseButton);


    }
    public void ExitToMenue()
    {
        SceneManager.LoadScene("Mainmenu", LoadSceneMode.Single);
        pauseMenuUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(Mainmenu.StartinngButton);
        
    }
    
}
