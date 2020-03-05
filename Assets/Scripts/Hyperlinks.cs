using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Hyperlinks : MonoBehaviour, IPointerClickHandler
{
    private Camera _camera;
    private TextMeshProUGUI _textMeshProUgui;

    private void Start()
    {
        _camera = Camera.current;
        _textMeshProUgui = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        var isHoveringOver =
            TMP_TextUtilities.IsIntersectingRectTransform(_textMeshProUgui.rectTransform, Input.mousePosition, _camera);
        var linkIndex = isHoveringOver
            ? TMP_TextUtilities.FindIntersectingLink(_textMeshProUgui, Input.mousePosition, _camera)
            : -1;

        var i = 0;
        foreach (var linkInfo in _textMeshProUgui.textInfo.linkInfo)
        {
            SetLinkToColor(i, new Color32(255, 255, 225, 255));
            i++;
        }

        if (isHoveringOver && linkIndex != -1)
        {
            SetLinkToColor(linkIndex, new Color32(255, 122, 57, 255));
        }
    }

    /*
     * Thanks to https://deltadreamgames.com/unity-tmp-hyperlinks/
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        var linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshProUgui, Input.mousePosition, _camera);
        if (linkIndex == -1)
        {
            return;
        }

        // was a link clicked?
        var linkInfo = _textMeshProUgui.textInfo.linkInfo[linkIndex];
        var linkId = linkInfo.GetLinkID();

        if (linkId.StartsWith("http"))
        {
            // open the link id as a url, which is the metadata we added in the text field
            Application.OpenURL(linkId);
        }
        else
        {
            Debug.Log("Click to " + linkId);
        }
    }

    private List<Color32[]> SetLinkToColor(int linkIndex, Color32 color)
    {
        var linkInfo = _textMeshProUgui.textInfo.linkInfo[linkIndex];

        var oldVertColors = new List<Color32[]>(); // store the old character colors

        for (var i = 0; i < linkInfo.linkTextLength; i++)
        {
            // for each character in the link string
            var characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
            var charInfo = _textMeshProUgui.textInfo.characterInfo[characterIndex];
            var meshIndex =
                charInfo
                    .materialReferenceIndex; // Get the index of the material / sub text object used by this character.
            var vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

            var vertexColors =
                _textMeshProUgui.textInfo.meshInfo[meshIndex].colors32; // the colors for this character
            oldVertColors.Add(vertexColors.ToArray());

            if (charInfo.isVisible)
            {
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
        }

        // Update Geometry
        _textMeshProUgui.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        return oldVertColors;
    }
}