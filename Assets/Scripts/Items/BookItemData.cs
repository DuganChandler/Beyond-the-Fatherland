using UnityEngine;

[CreateAssetMenu(fileName = "New Book Item", menuName = "Items/BookItem")]
public class BookItemData : ItemBase {
    [SerializeField] private BookEntry bookEntry;

    public BookEntry BookEntry {
        get {
            return bookEntry;
        }
    }
}

[System.Serializable]
public class BookEntry {
    public string bookTitle;
    public string fileName;
}
