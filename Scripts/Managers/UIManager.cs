using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Scores Display")]
    [SerializeField]
    private int _currentScore;
    [Space]
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [Header("Lives Display")]
    [SerializeField]
    private Image _livesImage;
    [Space]
    [SerializeField]
    private Sprite[] _livesSprites;

    [Header("Game Over")]
    [SerializeField]
    private GameObject _gameOverObject;

    private void OnEnable()
    {
        Player.onPlayerDamaged += UpdateLives;

        Enemy.onEnemyKilled += UpdateScore;
    }

    private void OnDisable()
    {
        Player.onPlayerDamaged -= UpdateLives;

        Enemy.onEnemyKilled -= UpdateScore;
    }

    public void UpdateScore(int earnedPoints)
    {
        _currentScore += earnedPoints;

        _scoreText.text = _currentScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives <= 0)
        {
            GameOverDisplay();
        }
    }

    public void GameOverDisplay()
    {
        _gameOverObject.SetActive(true);
    }
}