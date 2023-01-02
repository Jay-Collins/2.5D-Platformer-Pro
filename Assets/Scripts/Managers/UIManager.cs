using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("References")]
    [SerializeField] private TMP_Text _orbsText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;

    private void Start()
    {
        UpdateLives();
        UpdateOrbs();
    }

    public void UpdateOrbs()
    {
        _orbsText.text = "Orbs: " + PlayerMovement.orbs;
    }

    public void UpdateLives()
    {
        _livesText.text = "Lives: " + PlayerMovement.lives;
    }

    public void GameOver()
    {
        _gameOverText.enabled = true;
        _restartText.enabled = true;
    }
}