using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Background;
    public GameObject backscreen;

    public GameObject MainMenueTing;
    public GameObject StartinngButton;
    
    public GameObject pauseMenuUI;
    public GameObject firseButton;
 
    public GameObject optionsMenuUI;
    public GameObject StartingButton;

    public GameObject ItemsMenuUI;
    public GameObject FirstItem;

    public GameObject StatsMenuUI;
    public GameObject FirstStats;
    public GameObject character1Stats;
    public GameObject FirstStatsA;
    public GameObject character2Stats;
    public GameObject FirstStatsB;
    public GameObject character3Stats;
    public GameObject FirstStatsC;


    public GameObject StoriesMenuUI;
    public GameObject FirstStory;

    
   



    // Start is called before the first frame update
    public bool IsGamePaused = false;
    public void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (IsGamePaused)
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

    //pause menu tings
    public void Continue()
    {
        pauseMenuUI.SetActive(false);
        Background.SetActive(false);
        backscreen.SetActive(false);

        IsGamePaused = false;
        EventSystem.current.SetSelectedGameObject(StartinngButton);
        //replace the Mainmenu.starting button with the starting button on the scene 
        //for example the first character profile

    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Background.SetActive(true);
        backscreen.SetActive(true);

        IsGamePaused = true;
        EventSystem.current.SetSelectedGameObject(firseButton);


    }
    public void ExitToMenue()
    {
        SceneManager.LoadScene("Mainmenu", LoadSceneMode.Single);
        pauseMenuUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(StartinngButton);

    }

    //Main menue tings
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

    //options Menue tings
    public void openOpps()
    {

        optionsMenuUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(StartingButton);
        Debug.Log("The options Menue is open");
       
        pauseMenuUI.SetActive(false);

    }

    public void stopOpps()
    {
        optionsMenuUI.SetActive(false); ;
        Debug.Log("The options Menue is closed");
        
        if (IsGamePaused == true)
        {
            EventSystem.current.SetSelectedGameObject(firseButton);
        }
        else { EventSystem.current.SetSelectedGameObject(StartinngButton); }
        pauseMenuUI.SetActive(true);

    }

    public void volume()
    {
        Debug.Log("Volume is volumeing");
    }


    //Item menue tings
    public void openItems()
    {
        
        ItemsMenuUI.SetActive(true );
        Debug.Log("Item menue is open");
        EventSystem.current.SetSelectedGameObject(FirstItem);

        
        pauseMenuUI.SetActive(false);
    }

    public void closedItems()
    {
        ItemsMenuUI.SetActive(false ) ;
        Debug.Log("Item Menu is closed");
        
        EventSystem.current.SetSelectedGameObject(firseButton) ;
        pauseMenuUI.SetActive(true);
    }
    public void firstitme()
    {
        Debug.Log("Item is iteming");
        
    }


    //stats menue tings
    public void openStats()
    {
        StatsMenuUI.SetActive(true);
        Debug.Log("Stats menue is open");
        EventSystem.current.SetSelectedGameObject(FirstStats);
        
        pauseMenuUI.SetActive(false);
    }

    public void closedStats()
    {
        StatsMenuUI.SetActive(false);
        Debug.Log("Stats menue is closed");
        EventSystem.current.SetSelectedGameObject(firseButton);
        
        pauseMenuUI.SetActive(true);
    }
    public void firstStat()
    {
        Debug.Log("first stat is stating");
        character1Stats.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstStatsA);
        StatsMenuUI.SetActive(false );

    }
    public void atackone()
    {
        Debug.Log("Attack 1 description pops up");
    }

    public void secondstat()
    {
        Debug.Log("second stat is stating");
        character2Stats.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstStatsB);
        StatsMenuUI.SetActive(false);

    }
    public void atacktwo()
    {
        Debug.Log("Attack 2 description pops up");
    }
    public void thirdStat()
    {
        Debug.Log("third stat is stating");
        character3Stats.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstStatsC);
        StatsMenuUI.SetActive(false);
    }
    public void atackthree()
    {
        Debug.Log("Attack 3 description pops up");
    }

    public void closeCurrentStat()
    {
        character1Stats.SetActive(false);
        character2Stats.SetActive(false);
        character3Stats.SetActive(false);
        EventSystem.current.SetSelectedGameObject(FirstStats);
        StatsMenuUI.SetActive(true);
    }
    

    // stories menu tings
    public void openStories()
    {
        StoriesMenuUI.SetActive(true);
        Debug.Log("Stories menue is open");
        EventSystem.current.SetSelectedGameObject(FirstStory);
        
        pauseMenuUI.SetActive(false);

    }

    public void closedStories()
    {
        StoriesMenuUI.SetActive(false);
        Debug.Log("Stories menue is closed");
        EventSystem.current.SetSelectedGameObject(firseButton);
        
        pauseMenuUI.SetActive(true);


    }

    public void firststori()
    {
        Debug.Log("stories are storing");
        
    }

    
    //save data shinanigains
    public void Savestuff()
    {
        Debug.Log("The game data is saving");
    }
}
