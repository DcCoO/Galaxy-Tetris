using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int id;
    private Transform t;
    public Transform[] parts;

    private static readonly int Color = Shader.PropertyToID("_Color");

    void Start()
    {
        t = transform;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        for (int i = 0; i < parts.Length; ++i)
        {
            Renderer partRenderer = parts[i].GetComponent<Renderer>();
            partRenderer.GetPropertyBlock(block);
            block.SetColor(Color, ColorController.GetPieceColor(id));
            partRenderer.SetPropertyBlock(block);
        }
    }

    public List<Vector3Int> GetPiecesUnderneath()
    {
        List<Vector3Int> list = new List<Vector3Int>();
        
        for (int i = 0; i < parts.Length; ++i)
        {
            Vector3Int u = Vector3Int.RoundToInt(parts[i].position);
            bool isUnderneath = true;
            for (int j = 0; j < parts.Length; ++j)
            {
                Vector3Int v = Vector3Int.RoundToInt(parts[j].position);
                if (u.x == v.x && u.z == v.z && u.y > v.y)
                {
                    isUnderneath = false;
                    break;
                }
            }
            if(isUnderneath) list.Add(u);
        }

        return list;
    }

    public void Fall()
    {
        t.position += Vector3.down;
    }

    public void Rotate(int x, int y, int z)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            Vector3Int beginPos = Vector3Int.RoundToInt(parts[i].position);
            Vector3Int endPos = RotatePointAroundPivot(beginPos, Vector3Int.RoundToInt(t.position), new Vector3Int(x, y, z));
            if (!GameController.instance.CheckEmptySlot(endPos)) return;
        }
        t.Rotate(x, y, z, Space.World);
    }

    public void Translate(int x, int z)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            Vector3Int beginPos = Vector3Int.RoundToInt(parts[i].position);
            Vector3Int endPos = beginPos + new Vector3Int(x, 0, z);
            if (!GameController.instance.CheckEmptySlot(endPos)) return;
        }
        t.position += new Vector3Int(x, 0, z);
    }
    
    private Vector3Int RotatePointAroundPivot(Vector3Int point, Vector3Int pivot, Vector3Int angles) {
        return Vector3Int.RoundToInt(Quaternion.Euler(angles) * (point - pivot) + pivot);
    }
}
