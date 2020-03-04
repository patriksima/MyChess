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

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}