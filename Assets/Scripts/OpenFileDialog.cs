using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OpenFileDialog : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemPrefab;

    private List<GameObject> items = new List<GameObject>();

    private string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                                      Path.DirectorySeparatorChar + "Downloads";

    public void Setup()
    {
        Debug.Log(Application.dataPath);
        Debug.Log(Application.systemLanguage);
        Debug.Log(Application.persistentDataPath);

        Debug.Log(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        Debug.Log(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

        ClearList();

        DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);
        DirectoryInfo[] directories = directoryInfo.GetDirectories();

        if (directoryInfo.Parent != null)
            CreateItem(ItemType.Root, "..");

        foreach (var directory in directories)
        {
            CreateItem(ItemType.Directory, directory.Name);
        }

        FileInfo[] fileInfo = directoryInfo.GetFiles("*.pgn", SearchOption.TopDirectoryOnly);
        foreach (var file in fileInfo)
        {
            CreateItem(ItemType.File, file.Name);
        }
    }

    private void OnItemClick(GameObject item)
    {
        ItemType itemType = item.GetComponent<FileItem>().ItemType;

        if (itemType == ItemType.Root)
        {
            int lastSlashPos = currentDirectory.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastSlashPos != -1)
            {
                currentDirectory = currentDirectory.Substring(0, lastSlashPos);
                Debug.Log(currentDirectory);
                Setup();
            }
        }

        if (itemType == ItemType.Directory)
        {
            currentDirectory += Path.DirectorySeparatorChar + item.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log(currentDirectory);
            Setup();
        }
    }

    private void CreateItem(ItemType itemType, string text)
    {
        GameObject item = Instantiate(itemPrefab, transform);
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