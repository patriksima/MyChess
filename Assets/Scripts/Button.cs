using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject panel = null;

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
