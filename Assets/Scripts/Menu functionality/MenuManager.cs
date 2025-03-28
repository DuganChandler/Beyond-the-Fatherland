using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BagMenuState {
    Options,
    Items,
    Books,
    Status,
    System,
    Using
}

public struct BagMenuStateObject {
    public BagMenuStateObject(BagMenuState state, GameObject menuObject) {
        State = state;
        MenuObject = menuObject;
    }

    public BagMenuState State;
    public GameObject MenuObject;
}

public class MenuManager : MonoBehaviour {
    [Header("UI Menus")]
    [SerializeField] private GameObject bagMenu;
    [SerializeField] private GameObject characterPortraits;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject system;
    [SerializeField] private GameObject items;
    [SerializeField] private GameObject books;
    [SerializeField] private GameObject bookReadingSection;
    [SerializeField] private GameObject status;

    [Header("Menu Buttons")]
    [SerializeField] private List<Button> optionButtonList;
    [SerializeField] private List<Button> characterPortraitButtonList;

    [Header("Character Huds")]
    [SerializeField] private List<CharacterHud> characterHuds;

    [Header("Player Information")]
    [SerializeField] Inventory playerInventory;
    [SerializeField] PartyList partyList;

    [Header("Book Reader")]
    [SerializeField] private BookReader bookReader;

    private Stack<BagMenuStateObject> menuStates;

    private ItemSlot itemToUse = new();
    // ability

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


    void OnEnable() {
        items.GetComponent<ItemMenu>().OnItemSelected += HandleItemSelection;
        books.GetComponent<ItemMenu>().OnItemSelected += HandleBookSelection;

        menuStates = new();

        optionButtonList[0].Select();

        SetupMenuData();

        options.SetActive(true);
        characterPortraits.SetActive(true);

        menuStates.Push(new BagMenuStateObject(BagMenuState.Options, options));
    }

    void OnDisable() {
        items.GetComponent<ItemMenu>().OnItemSelected -= HandleItemSelection;
        books.GetComponent<ItemMenu>().OnItemSelected -= HandleBookSelection;

        EventSystem.current.SetSelectedGameObject(null);
        if (menuStates.Count > 0) {
            menuStates.Pop().MenuObject.SetActive(false);
        }
    }

    public void OnResume() {
        MusicManager.Instance.PlaySound("MenuConfirm");
        GameManager.Instance.GameState = GameState.FreeRoam;
        menuStates.Clear();
        bagMenu.SetActive(false);
    }

    private void SetupMenuData() {
        for (int i = 0; i < characterHuds.Count; ++i) {
            characterHuds[i].SetData(partyList.CharacterList[i]);
        }
    }

    public void OnItemMenu() {
        MusicManager.Instance.PlaySound("MenuConfirm");

        if (menuStates.Peek().MenuObject != null) {
            menuStates.Peek().MenuObject.SetActive(false);
        }

        menuStates.Push(new BagMenuStateObject(BagMenuState.Items, items));
        items.SetActive(true);
        items.GetComponent<ItemMenu>().PopulateInventory(playerInventory, ItemCategory.Combat);
    }

    public void OnStatusMenu () {
        MusicManager.Instance.PlaySound("MenuConfirm");
        menuStates.Peek().MenuObject.SetActive(false);
        menuStates.Push(new BagMenuStateObject(BagMenuState.Status, status));
        status.SetActive(true);
    }

    public void OnSystemMenu () {
        MusicManager.Instance.PlaySound("MenuConfirm");
        menuStates.Peek().MenuObject.SetActive(false);
        menuStates.Push(new BagMenuStateObject(BagMenuState.System, system));
        system.SetActive(true);
    }

    public void OnBookMenu() {
        MusicManager.Instance.PlaySound("MenuConfirm");
        menuStates.Peek().MenuObject.SetActive(false);
        menuStates.Push(new BagMenuStateObject(BagMenuState.Books, books));

        books.GetComponent<ItemMenu>().PopulateInventory(playerInventory, ItemCategory.Story);

        books.SetActive(true);
    }

    public void OnBackButton(InputAction.CallbackContext context) {
        if (!context.started || GameManager.Instance.GameState != GameState.Pause) {
            return;
        }
        Debug.Log(menuStates.Peek().State);

        if (menuStates.Peek().State != BagMenuState.Using) {
            if (menuStates.Peek().State == BagMenuState.Options) {
                OnResume();
                return;
            }

            var prev = menuStates.Pop().MenuObject;

            if (prev != null) {
                prev.SetActive(false);
            }

            menuStates.Peek().MenuObject.SetActive(true);

            foreach(var button in optionButtonList) {
                if (button.gameObject.name == prev.name) {
                    button.Select();
                }
            }
        }

        if (menuStates.Peek().State == BagMenuState.Using) {
            if(itemToUse.Item != null && itemToUse.Item.ItemCategory == ItemCategory.Combat) {
                menuStates.Pop();
                items.GetComponent<ItemMenu>().PopulateInventory(playerInventory, ItemCategory.Combat);
            } else if (itemToUse.Item != null && itemToUse.Item.ItemCategory == ItemCategory.Story) {
                menuStates.Pop();

                bookReadingSection.SetActive(false);

                characterPortraits.SetActive(true);
                books.SetActive(true);

                books.GetComponent<ItemMenu>().PopulateInventory(playerInventory, ItemCategory.Story);
            }
        }
    }

    private void HandleItemSelection(ItemSlot slot) {
        // go to select a character 
        menuStates.Push(new BagMenuStateObject(BagMenuState.Using, null));
        characterPortraitButtonList[0].Select();
        itemToUse = slot;
    }

    private void HandleBookSelection(ItemSlot slot) {
            menuStates.Push(new BagMenuStateObject(BagMenuState.Using, null));
            itemToUse = slot;

            characterPortraits.SetActive(false);
            books.SetActive(false);
            bookReadingSection.SetActive(true);

            BookItemData bookToRead = (BookItemData)itemToUse.Item;
            bookReader.LoadBookByName(bookToRead.BookEntry.bookTitle);
    }

    private void HandleAbilitySelection(AbilityBase ability) {
        // go to select a character
    }

    public void OnUse(int hudIndex) {
        if (itemToUse.Item != null) {
            StartCoroutine(UseItem((CombatItemData)itemToUse.Item,partyList.CharacterList[hudIndex]));
        } else {
            // OnItemMenu();
            Debug.Log("no item bro");
        }
    }

    public IEnumerator UseItem(CombatItemData item, Character target) {
        foreach (ItemEffectBase effect in item.effects) {
            EffectInfo effectInfo = effect.ApplyEffect(null, target);
            Debug.Log(effectInfo.TextInformation);
        }

        playerInventory.RemoveItem(item);
        itemToUse.Item = null;

        yield return new WaitForEndOfFrame();

        items.GetComponent<ItemMenu>().PopulateInventory(playerInventory, ItemCategory.Combat);

        menuStates.Pop();
        menuStates.Pop();
        OnItemMenu();
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

    public void volume()
    {
        Debug.Log("Volume is volumeing");
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
