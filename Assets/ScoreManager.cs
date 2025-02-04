using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    int score = 0;
    int highscore = 0;

    void Start()
    {
        // scoreText.text = score.ToString() + " POINTS";
        // highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
