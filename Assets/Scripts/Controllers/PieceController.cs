using System.Collections.Generic;
using UnityEngine;

public class PieceController : SingletonMonoBehaviour<PieceController>, IReset
{
    
    private Piece currentPiece;
    private bool isMoving;

    void Start()
    {
        TimeController.instance.onTick += Fall;
    }

    void Update()
    {
        if (!isMoving) return;
        
        if (Input.GetKeyDown(KeyCode.Z)) currentPiece.Rotate(90, 0, 0);
        if (Input.GetKeyDown(KeyCode.X)) currentPiece.Rotate(0, 90, 0);
        if (Input.GetKeyDown(KeyCode.C)) currentPiece.Rotate(0, 0, 90);
        
        if (Input.GetKeyDown(KeyCode.A)) currentPiece.Translate(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) currentPiece.Translate(1, 0);
        if (Input.GetKeyDown(KeyCode.W)) currentPiece.Translate(0, 1);
        if (Input.GetKeyDown(KeyCode.S)) currentPiece.Translate(0, -1);
    }

    void Fall()
    {
        if (isMoving)
        {
            List<Vector3Int> piecesUnderneath = currentPiece.GetPiecesUnderneath();
            bool canFall = GameController.instance.CanFall(piecesUnderneath);
            if (canFall) currentPiece.Fall();
            else
            {
                isMoving = false;
                GameController.instance.PlacePieceInMatrix(currentPiece.parts);
                GameController.instance.CheckCompleteLayers(piecesUnderneath);
            }
        }
    }

    public void SetCurrentPiece(Piece piece)
    {
        currentPiece = piece;
        isMoving = true;
    }

    public void Reset()
    {
        isMoving = false;
    }
}
