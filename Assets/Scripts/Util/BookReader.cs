using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class BookReader : MonoBehaviour {
    [Header("Library & Book Selection")]
    // Reference to the BookLibrary ScriptableObject
    public BookLibraryData bookLibrary;
    // For demo purposes,you can choose a book by index from the library.
    public int selectedBookIndex;

    [Header("UI Elements")]
    // UI Text elements for the left and right pages (using TextMeshProUGUI)
    public TextMeshProUGUI leftPageText;
    public TextMeshProUGUI rightPageText;

    // Internal variables for managing pages
    private string[] pages;
    private int currentPageIndex = 0;

    private Vector2 playerInput;

    private bool canNavigate = true;

    void Start() {
        // Option 1: Load a book by an index from the library.
        // LoadBookByIndex(selectedBookIndex);
        // Option 2: Alternatively, you could expose a method that UI buttons call to select a book.
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            NextPage();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            PreviousPage();
        } 

        if (currentPageIndex == 0) {
            // disable left arrow
        } else if (currentPageIndex == pages.Length - 1) {
            // disable right arrow
        }
    }

    public void OnPageTurn(InputAction.CallbackContext context) {
        if (GameManager.Instance.GameState != GameState.Pause) return;

        playerInput = context.ReadValue<Vector2>();

        if (canNavigate && Mathf.Abs(playerInput.x) > 0.5f) {
            if (playerInput.x > 0.5f) {
                NextPage();
            } else if (playerInput.x < -0.5f) {
                PreviousPage();
            }
            canNavigate = false;
        }

        if (Mathf.Abs(playerInput.x) < 0.5f && Mathf.Abs(playerInput.y) < 0.5f) {
            canNavigate = true;
        }
    }

    /// <summary>
    /// Loads a book from the library based on its index.
    /// </summary>
    public void LoadBookByIndex(int index) {
        if (bookLibrary == null || bookLibrary.books == null || bookLibrary.books.Count == 0) {
            Debug.LogError("BookLibrary is empty or not assigned.");
            return;
        }

        if (index < 0 || index >= bookLibrary.books.Count) {
            Debug.LogError("Invalid book index.");
            return;
        }

        BookEntry entry = bookLibrary.books[index].BookEntry;
        StartCoroutine(LoadBookCoroutine(entry));
    }

    public void LoadBookByName(string title) {
        if (bookLibrary == null || bookLibrary.books == null || bookLibrary.books.Count == 0) {
            Debug.LogError("BookLibrary is empty or not assigned.");
            return;
        }

        BookEntry entry = new();

        foreach (BookItemData book in bookLibrary.books) {
            if (book.BookEntry.bookTitle == title) {
                entry = book.BookEntry;
                break;
            }
        }

        StartCoroutine(LoadBookCoroutine(entry));
    }

    /// <summary>
    /// Loads the book file asynchronously.
    /// </summary>
    IEnumerator LoadBookCoroutine(BookEntry entry) {
        // Build the full path: StreamingAssets + relative file path
        string path = Path.Combine(Application.streamingAssetsPath, "Books", entry.fileName);
        string fileContent = "";

        // On some platforms (like Android) use UnityWebRequest
        if (path.Contains("://") || path.Contains(":///")) {
            using (UnityWebRequest www = UnityWebRequest.Get(path)) {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError("Error loading file: " + www.error);
                    yield break;
                }

                fileContent = www.downloadHandler.text;
            }
        } else {
            fileContent = File.ReadAllText(path);
        }

        // Split the content into pages using a delimiter.
        // For example, in your text file separate pages with "---PAGE BREAK---"
        pages = fileContent.Split(new string[] { "---PAGE BREAK---" }, System.StringSplitOptions.None);
        currentPageIndex = 0;
        DisplayPages();
    }

    /// <summary>
    /// Displays two pages (left and right) based on the currentPageIndex.
    /// </summary>
    void DisplayPages() {
        if (pages == null || pages.Length == 0) {
            leftPageText.text = "";
            rightPageText.text = "";
            return;
        }

        leftPageText.text = currentPageIndex < pages.Length ? pages[currentPageIndex].Trim() : "";
        rightPageText.text = (currentPageIndex + 1) < pages.Length ? pages[currentPageIndex + 1].Trim() : "";
    }

    /// <summary>
    /// Advances to the next two pages.
    /// </summary>
    public void NextPage() {
        currentPageIndex += 2;
        if (currentPageIndex >= pages.Length) {
            // Clamp to the last valid pair of pages
            if (pages.Length % 2 == 0)
                currentPageIndex = pages.Length - 2;
            else
                currentPageIndex = pages.Length - 1;
        }
        DisplayPages();
    }

    /// <summary>
    /// Goes back to the previous two pages.
    /// </summary>
    public void PreviousPage() {
        currentPageIndex -= 2;
        if (currentPageIndex < 0)
            currentPageIndex = 0;
        DisplayPages();
    }
}
