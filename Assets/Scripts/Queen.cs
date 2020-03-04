using UnityEngine;
using UnityEngine.UI;

public class Queen : Piece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        movement = new Vector3Int(7, 7, 7);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Queen");
    }
}