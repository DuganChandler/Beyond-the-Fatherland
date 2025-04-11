using UnityEngine;
using TMPro;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

    private const float heightTolerance = 5f;

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
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "Books", entry.fileName);
        string fileContent = "";

        if (path.Contains("://") || path.Contains(":///")) {
            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path)) {
                yield return www.SendWebRequest();

                if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success) {
                    Debug.LogError("Error loading file: " + www.error);
                    yield break;
                }
                fileContent = www.downloadHandler.text;
            }
        } else {
            fileContent = System.IO.File.ReadAllText(path);
        }

        // Combine manual page breaks with auto-pagination:
        pages = ProcessBookText(fileContent);
        currentPageIndex = 0;
        DisplayPages();
    }
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

    List<string> AutoPaginateText(string text, TextMeshProUGUI textComponent) {
        List<string> pages = new List<string>();
        string[] words = text.Split(' ');
        string currentPage = "";

        // Retrieve the dimensions of the text area.
        RectTransform rectTransform = textComponent.rectTransform;
        float maxHeight = rectTransform.rect.height;
        float maxWidth = rectTransform.rect.width;

        foreach (string word in words) {
            // Build tentative page text with the new word.
            string testPage = string.IsNullOrEmpty(currentPage) ? word : currentPage + " " + word;
            
            // Set the text and force an update to ensure accurate measurement.
            textComponent.text = testPage;
            textComponent.ForceMeshUpdate();
            
            // Measure based on available width.
            Vector2 preferredSize = textComponent.GetPreferredValues(testPage, maxWidth, float.PositiveInfinity);
            
            // Uncomment the following line for debugging:
            // Debug.Log($"Test Page: \"{testPage}\" | Preferred Height: {preferredSize.y}, Max Height: {maxHeight}");

            // Check if adding this word exceeds the maximum allowed height.
            if (preferredSize.y > maxHeight && !string.IsNullOrEmpty(currentPage)) {
                // If yes, commit the current page and start a new one.
                pages.Add(currentPage.Trim());
                currentPage = word; // Start new page with the current word.
            } else {
                // Otherwise, update the current page.
                currentPage = testPage;
            }
        }
        
        // Add any remaining text as a final page.
        if (!string.IsNullOrEmpty(currentPage))
            pages.Add(currentPage.Trim());
        
        return pages;
    }

    /// <summary>
    /// Processes the book text by first splitting at manual page breaks,
    /// then further auto-splitting each manual chunk if necessary.
    /// </summary>
    private string[] ProcessBookText(string fileContent) {
        // Split by manual delimiter
        string[] manualChunks = fileContent.Split(
            new string[] { "---PAGE BREAK---" },
            System.StringSplitOptions.None
        );

        List<string> finalPages = new List<string>();

        foreach (string chunk in manualChunks) {
            string trimmedChunk = chunk.Trim();
            if (string.IsNullOrEmpty(trimmedChunk))
                continue;

            // Use binary search based auto-pagination for each chunk.
            List<string> autoPaginated = AutoPaginateTextBinary(trimmedChunk, leftPageText);
            finalPages.AddRange(autoPaginated);
        }
        return finalPages.ToArray();
    }

    List<string> AutoPaginateTextBinary(string text, TextMeshProUGUI textComponent) {
        List<string> pages = new List<string>();
        RectTransform rectTransform = textComponent.rectTransform;
        float maxWidth = rectTransform.rect.width;
        float maxHeight = rectTransform.rect.height;

        int currentIndex = 0;
        while (currentIndex < text.Length) {
            int pageLength = FindMaxLengthThatFits(text, currentIndex, textComponent, maxWidth, maxHeight);
            if (pageLength == 0) break; // safeguard

            // To avoid breaking words, backtrack to the last space.
            int breakIndex = currentIndex + pageLength;
            if (breakIndex < text.Length) {
                int lastSpace = text.LastIndexOf(' ', breakIndex);
                if (lastSpace > currentIndex)
                    breakIndex = lastSpace;
            }
            
            string pageText = text.Substring(currentIndex, breakIndex - currentIndex).Trim();
            pages.Add(pageText);
            currentIndex = breakIndex;
            // Skip any whitespace for the next page.
            while (currentIndex < text.Length && char.IsWhiteSpace(text[currentIndex]))
                currentIndex++;
        }
        return pages;
    }

    int FindMaxLengthThatFits(string text, int start, TextMeshProUGUI textComponent, float maxWidth, float maxHeight) {
        int low = 1;
        int high = text.Length - start;
        int best = 0;
        while (low <= high) {
            int mid = (low + high) / 2;
            string candidate = text.Substring(start, mid);
            textComponent.text = candidate;
            textComponent.ForceMeshUpdate();
            Vector2 preferredSize = textComponent.GetPreferredValues(candidate, maxWidth, float.PositiveInfinity);

            // Allowing a small tolerance to account for rounding/margins.
            if (preferredSize.y <= maxHeight + heightTolerance) {
                best = mid;
                low = mid + 1;
            } else {
                high = mid - 1;
            }
        }
        return best;
    }
}
