using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public int totalScore = 0;
    public Text scoreText;
    public Text centerText;
    public float centerTextSize = 1.0f;

    public static GameController instance;

    void Start()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateTotalScore()
    {
        this.scoreText.text = totalScore.ToString();
        this.centerText.text = totalScore.ToString();

    }
}
