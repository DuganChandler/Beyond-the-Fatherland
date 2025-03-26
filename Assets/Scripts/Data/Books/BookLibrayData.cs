using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookLibrary", menuName = "Books/Book Library")]
public class BookLibraryData: ScriptableObject {
    public List<BookItemData> books;
}

