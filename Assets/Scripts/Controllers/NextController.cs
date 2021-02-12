using UnityEngine;
using UnityEngine.UI;

public class NextController : SingletonMonoBehaviour<NextController>, IReset
{
    public Sprite[] sprites;
    public Image nextImage;

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
