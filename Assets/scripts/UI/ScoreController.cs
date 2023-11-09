using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int score;
    private void Awake()
    {
        score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
        RefreshUI();
    }

    public void AddScore(int score) {
        this.score += score;
        RefreshUI();
    }

    private void RefreshUI()
    {
        scoreText.text = score.ToString();
    }
}
