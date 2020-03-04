using UnityEngine;

public class MyCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}