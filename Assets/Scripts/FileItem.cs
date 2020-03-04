using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Root,
    Directory,
    File
}
public class FileItem : MonoBehaviour
{
    private ItemType _itemType;

    public ItemType ItemType
    {
        get => _itemType;
        set => _itemType = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
