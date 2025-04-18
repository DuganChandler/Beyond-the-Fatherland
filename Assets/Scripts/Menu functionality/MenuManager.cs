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
    Using,
    Controls
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
    [SerializeField] private GameObject controls;

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


    private static ItemSlot itemToUse = new();

    public GameObject StatsMenuUI;

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

        if (EventSystem.current) {
            EventSystem.current.SetSelectedGameObject(null);
        }

        CheckReversUsingState();

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

    public void OnControls() {
        MusicManager.Instance.PlaySound("MenuConfirm");
        menuStates.Peek().MenuObject.SetActive(false);
        menuStates.Push(new BagMenuStateObject(BagMenuState.Controls, controls));
        controls.SetActive(true);
    }

    public void OnMainMenu() {
        // MusicManager.Instance.PlaySound("MenuConfirm");
        SceneHelper.LoadScene("MainMenu", false, true);
        GameManager.Instance.GameState = GameState.FreeRoam;
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

        CheckReversUsingState();
    }

    void CheckReversUsingState() {
        if (menuStates.Count < 1) return; 

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
            EffectInfo effectInfo = effect.ApplyEffectToCharacter(null, target);
            Debug.Log(effectInfo.TextInformation);
        }

        playerInventory.RemoveItem(item);

        itemToUse = null;

        yield return new WaitForEndOfFrame();

        items.GetComponent<ItemMenu>().PopulateInventory(playerInventory, ItemCategory.Combat);

        menuStates.Pop();
        menuStates.Pop();
        OnItemMenu();
    }
}
