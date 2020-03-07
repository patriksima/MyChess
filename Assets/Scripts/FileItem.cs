namespace MyChess
{
    using UnityEngine;

    public enum ItemType
    {
        Root,
        Directory,
        File
    }

    public class FileItem : MonoBehaviour
    {
        public ItemType ItemType { get; set; }
    }
}