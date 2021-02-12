using UnityEngine;

public class PreviewController : SingletonMonoBehaviour<PreviewController>, IReset
{
    private bool isPreviewing;
    private Piece currentPiece;
    public Transform previewParent;

    private Transform[] previewParts;
    private static readonly int Color = Shader.PropertyToID("_Color");
    
    void Update()
    {
        if (isPreviewing) UpdatePreview();
    }

    public void StartPreview(Piece piece)
    {
        
        previewParent.DestroyAllChildren();
        
        currentPiece = piece;
        previewParts = new Transform[piece.parts.Length];
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        
        for (int i = 0; i < previewParts.Length; ++i)
        {
            previewParts[i] = Instantiate(piece.parts[i].gameObject, previewParent).transform;
            Renderer partRenderer = previewParts[i].GetComponent<Renderer>();
            partRenderer.GetPropertyBlock(block);
            block.SetColor(Color, ColorController.GetPiecePreviewColor(piece.id));
            partRenderer.SetPropertyBlock(block);
        }

        isPreviewing = true;
    }

    private void UpdatePreview()
    {
        int distanceToTouch = GameController.instance.DistanceToTouch(currentPiece.GetPiecesUnderneath());
        for (int i = 0; i < previewParts.Length; ++i)
        {
            previewParts[i].position = currentPiece.parts[i].position + Vector3Int.down * distanceToTouch;
        }
    }

    public void Reset()
    {
        previewParent.DestroyAllChildren();
        isPreviewing = false;
    }
}
