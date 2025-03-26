using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookLibrary", menuName = "Books/Book Library")]
public class BookLibraryData: ScriptableObject {
    public List<BookEntry> books;
}

[System.Serializable]
public class BookEntry {
    public string bookTitle;
    public string fileName;
}
