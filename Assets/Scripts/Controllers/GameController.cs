using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameController : SingletonMonoBehaviour<GameController>, IReset
{
    
    [Header("Game Settings")]
    public bool isPlaying;
    public GameState state;

    [Header("Piece Settings")]
    public Transform spawnPosition;
    public Transform piecesParent;
    public GameObject[] pieces;

    public UnityEvent OnGameOver;

    private int nextPieceID;
    private float height = 17;
    private readonly Dictionary<(int, int, int), Transform> matrix = new Dictionary<(int, int, int), Transform>();

    void Start()
    {
        TimeController.instance.onTick += GameStep;
        Reset();
    }

    public void Reset()
    {
        nextPieceID = -1;
        piecesParent.DestroyAllChildren();
        Transform t = transform;
        for (int x = -3; x < 4; ++x) for (int y = 0; y < height; ++y) for (int z = -3; z < 4; ++z) matrix[(x, y, z)] = null;
        for (int x = -3; x < 4; ++x) for (int z = -3; z < 4; ++z) matrix[(x, -1, z)] = t;
        for (int y = 0; y < height; ++y) for (int i = -4; i < 5; ++i) matrix[(i, y, 4)] = matrix[(i, y, -4)] = matrix[(4, y, i)] = matrix[(-4, y, i)] = t;
        isPlaying = false;
    }

    public void StartGame()
    {
        isPlaying = true;
        state = GameState.SPAWNING;
    }

    private void GameStep()
    {
        if (isPlaying)
        {
            if (state == GameState.SPAWNING)
            {
                if (nextPieceID == -1) nextPieceID = Random.Range(0, pieces.Length);
                Piece piece = Instantiate(
                    pieces[nextPieceID], 
                    spawnPosition.position, Quaternion.identity, 
                    piecesParent
                ).GetComponent<Piece>();
                
                nextPieceID = Random.Range(0, pieces.Length);
                NextController.instance.UpdateNext(nextPieceID);

                for (int i = 0; i < piece.parts.Length; ++i)
                {
                    if (!CheckEmptySlot(piece.parts[i].position))
                    {
                        state = GameState.LOSE;
                        Destroy(piece.gameObject);
                        return;
                    }
                }
                
                PieceController.instance.SetCurrentPiece(piece);
                PreviewController.instance.StartPreview(piece);
                state = GameState.MOVING;
            }
            else if (state == GameState.LOSE)
            {
                ResetAll();
                OnGameOver?.Invoke();
            }
        }
    }

    public bool CanFall(List<Vector3Int> parts)
    {
        for (int i = 0; i < parts.Count; ++i)
        {
            Vector3Int part = parts[i];
            if (!ReferenceEquals(matrix[(part.x, part.y - 1, part.z)], null)) return false;
        }
        return true;
    }

    public void PlacePieceInMatrix(Transform[] parts)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            Vector3Int pi = Vector3Int.RoundToInt(parts[i].position);
            matrix[(pi.x, pi.y, pi.z)] = parts[i];
        }
    }

    public void CheckCompleteLayers(List<Vector3Int> parts)
    {
        state = GameState.CHECKING;
        
        HashSet<int> layers = new HashSet<int>();
        List<int> completeLayers = new List<int>();
        for (int i = 0; i < parts.Count; ++i) layers.Add(parts[i].y);

        foreach (int y in layers)
        {
            if(CheckCompleteLayer(y)) completeLayers.Add(y);
        }

        if (completeLayers.Count == 0)
        {
            state = GameState.SPAWNING;
        }
        else
        {
            state = GameState.CLEARING;
            ClearLayers(completeLayers);
        }
    }

    private bool CheckCompleteLayer(int y)
    {
        for(int x = -3; x < 4; ++x) for (int z = -3; z < 4; ++z) if (ReferenceEquals(matrix[(x, y, z)], null)) return false;
        return true;
    }

    private void ClearLayers(List<int> layers)
    {
        for (int i = 0; i < layers.Count; ++i)
        {
            for (int x = -3; x < 4; ++x)
            {
                for (int z = -3; z < 4; ++z)
                {
                    Destroy(matrix[(x, layers[i], z)].gameObject);
                    matrix[(x, layers[i], z)] = null;
                }
            } 
            ScoreController.instance.AddScore();
        }

        for (int y = 1; y < height; ++y)
        {
            for (int x = -3; x < 4; ++x)
            {
                for (int z = -3; z < 4; ++z)
                {
                    if (!CheckEmptySlot(new Vector3Int(x, y, z)))
                    {
                        int dy = y;
                        while(CheckEmptySlot(new Vector3Int(x, dy - 1, z))) --dy;
                        if (dy != y)
                        {
                            matrix[(x, dy, z)] = matrix[(x, y, z)];
                            matrix[(x, y, z)] = null;
                            matrix[(x, dy, z)].position = new Vector3Int(x, dy,z);
                        }
                    }
                }
            }
        }

        AudioController.instance.ScoreSound();
        state = GameState.SPAWNING;
    }

    public bool CheckEmptySlot(Vector3 position)
    {
        return CheckEmptySlot(Vector3Int.RoundToInt(position));
    }
    public bool CheckEmptySlot(Vector3Int position)
    {
        return ReferenceEquals(matrix[(position.x, position.y, position.z)], null);
    }

    public int DistanceToTouch(List<Vector3Int> parts)
    {
        for (int dist = 1; dist < height; dist++)
        {
            for (int i = 0; i < parts.Count; ++i)
            {
                if(!CheckEmptySlot(parts[i] + Vector3Int.down * dist)) return dist - 1;
            }
        }

        return 0;
    }
    
    public void ResetAll()
    {
        var resets = FindObjectsOfType<MonoBehaviour>().OfType<IReset>();
        foreach (IReset reset in resets) {
            reset.Reset();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public enum GameState
{
    SPAWNING, MOVING, CHECKING, CLEARING, LOSE
}