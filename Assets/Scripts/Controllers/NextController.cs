using UnityEngine;
using UnityEngine.UI;

public class NextController : MonoBehaviour, IReset
{
    public static NextController instance;

    public Sprite[] sprites;
    public Image nextImage;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Reset();
    }

    public void UpdateNext(int id)
    {
        nextImage.sprite = sprites[id];
        nextImage.color = Color.white;
    }

    public void Reset()
    {
        nextImage.color = Color.clear;
    }
}
