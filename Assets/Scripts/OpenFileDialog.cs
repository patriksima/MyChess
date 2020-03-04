using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class OpenFileDialog : MonoBehaviour
{
    private readonly List<GameObject> items = new List<GameObject>();
    [SerializeField] private GameObject content;

    private string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                                      Path.DirectorySeparatorChar + "Downloads";

    [SerializeField] private GameObject itemPrefab;

    public void Setup()
    {
        ClearList();
        PopulateList();
    }

    private void PopulateList()
    {
        var directoryInfo = new DirectoryInfo(currentDirectory);

        if (directoryInfo.Parent != null)
        {
            CreateItem(ItemType.Root, "..");
        }

        try
        {
            var directories = directoryInfo.GetDirectories();
            foreach (var directory in directories)
            {
                CreateItem(ItemType.Directory, directory.Name);
            }
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.Log(e.Message);
        }

        try
        {
            var fileInfo = directoryInfo.GetFiles("*.pgn", SearchOption.TopDirectoryOnly);
            foreach (var file in fileInfo)
            {
                CreateItem(ItemType.File, file.Name);
            }
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.Log(e.Message);
        }
    }

    private void OnItemClick(GameObject item)
    {
        var itemType = item.GetComponent<FileItem>().ItemType;

        if (itemType == ItemType.Root)
        {
            var lastSlashPos = currentDirectory.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastSlashPos != -1)
            {
                currentDirectory = currentDirectory.Substring(0, lastSlashPos);
                ClearList();
                PopulateList();
            }
        }

        if (itemType == ItemType.Directory)
        {
            currentDirectory += Path.DirectorySeparatorChar + item.GetComponentInChildren<TextMeshProUGUI>().text;
            ClearList();
            PopulateList();
        }
    }

    private void CreateItem(ItemType itemType, string text)
    {
        var item = Instantiate(itemPrefab, transform);
        item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnItemClick(item); });
        item.GetComponentInChildren<TextMeshProUGUI>().text = text;
        item.transform.SetParent(content.transform);
        item.GetComponent<FileItem>().ItemType = itemType;
        items.Add(item);
    }

    private void ClearList()
    {
        foreach (var item in items)
        {
            Destroy(item);
        }

        items.Clear();
    }
}