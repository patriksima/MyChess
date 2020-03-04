using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}