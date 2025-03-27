using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public GameObject MainMenueTing;
   
    public GameObject StartinngButton;
    public Options Options;
    public void Start()
    {
       
      
    }
 

    public void stargGame()
    {

        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        // replace sample scene for the starting scene
        Debug.Log("Loading game scene and deleting this one");


    }

    public void Begone()
    {
        Application.Quit();
        Debug.Log("Succesfully exited the game");
    }

}
    



   





