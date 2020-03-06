using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ErrorDialog))]
public class OpenFileDialog : MonoBehaviour
{
    private readonly List<GameObject> _items = new List<GameObject>();

    private string _currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                                       Path.DirectorySeparatorChar + "Downloads";

    [SerializeField] private GameObject content;
    [SerializeField] private ErrorDialog errorDialog;
    [SerializeField] private GameObject itemPrefab;

    public void Setup()
    {
        ClearList();
        PopulateList();
    }

    private void PopulateList()
    {
        var directoryInfo = new DirectoryInfo(_currentDirectory);

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
            var lastSlashPos = _currentDirectory.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastSlashPos != -1)
            {
                _currentDirectory = _currentDirectory.Substring(0, lastSlashPos);
                ClearList();
                PopulateList();
            }
        }

        if (itemType == ItemType.Directory)
        {
            _currentDirectory += Path.DirectorySeparatorChar + item.GetComponentInChildren<TextMeshProUGUI>().text;
            ClearList();
            PopulateList();
        }

        if (itemType == ItemType.File)
        {
            // load and parse file
            var status = LoadFile(_currentDirectory + Path.DirectorySeparatorChar +
                                  item.GetComponentInChildren<TextMeshProUGUI>().text);

            if (status)
            {
                gameObject.SetActive(false);

                // write to info panel
                GameManager.Instance.InfoPanel.ShowGameData();
            }
        }
    }

    private bool LoadFile(string fileName)
    {
        Debug.Log("Loading " + fileName);

        try
        {
            var lexer = new PGNLexer(new AntlrInputStream(new StreamReader(fileName)));
            var stream = new CommonTokenStream(lexer);
            var parser = new PGNParser(stream);
            parser.ErrorHandler = new BailErrorStrategy();

            var context = parser.parse();

            Debug.Log("Count games in PGN file: " + context.pgn_database().pgn_game().Length);

            var walker = ParseTreeWalker.Default;

            // listen stream and build game data
            var listener = new PGNListener();
            walker.Walk(listener, context);

            // Copy moves from Listener to Game Manager
            GameManager.Instance.GameData.Moves = new Dictionary<int, List<string>>(listener.Moves);
            GameManager.Instance.GameData.SetTags(listener.Tags);
        }
        catch (Exception e)
        {
            errorDialog.GetComponentInChildren<TextMeshProUGUI>().text = "File cannot be read. " + e.Message;
            errorDialog.gameObject.SetActive(true);
            return false;
        }

        return true;
    }

    private void CreateItem(ItemType itemType, string text)
    {
        var item = Instantiate(itemPrefab, transform);
        item.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnItemClick(item); });
        item.GetComponentInChildren<TextMeshProUGUI>().text = text;
        item.transform.SetParent(content.transform);
        item.GetComponent<FileItem>().ItemType = itemType;
        _items.Add(item);
    }

    private void ClearList()
    {
        foreach (var item in _items)
        {
            Destroy(item);
        }

        _items.Clear();
    }
}