using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour, IReset
{
    public static ScoreController instance;
    
    public Text scoreText;
    public Text gameoverScoreText;
    public Text highscoreText;

    private int currentScore;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        if(currentScore > highscore) PlayerPrefs.SetInt("Highscore", currentScore);
        scoreText.text = "score: 0";
        gameoverScoreText.text = $"score: {currentScore}";
        highscoreText.text = $"highscore: {PlayerPrefs.GetInt("Highscore", 0)}";
        currentScore = 0;
    }

    public void AddScore()
    {
        currentScore++;
        scoreText.text = $"score: {currentScore}";
        gameoverScoreText.text = $"score: {currentScore}";
    }
}
