using UnityEngine;

public class ColorController : SingletonMonoBehaviour<ColorController>
{
    public Color[] pieceColor;

    public static Color GetPieceColor(int index)
    {
        if (ReferenceEquals(instance, null)) return Color.white;
        return instance.pieceColor[index];
    }
    
    public static Color GetPiecePreviewColor(int index)
    {
        if (ReferenceEquals(instance, null)) return Color.white;
        Color color = instance.pieceColor[index];
        color.a = 0.2f;
        return color;
    }
}