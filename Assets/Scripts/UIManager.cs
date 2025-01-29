using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private Image _livesImage;
      
    [SerializeField]
    private TMP_Text _gameOverText;
    
    [SerializeField]
    private TMP_Text _restartText;
    
    [SerializeField]
    private Slider _fuelGuageSlider;
    
    [SerializeField]
    private Sprite[] _livesSprites;
    
    private bool _shouldBlink = true;

    void Start()
    {
        _scoreText.text = $"Score: 0";
        _gameOverText.gameObject.SetActive(false);
    }
    
    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
    
    public void UpdateFuelGuage(float fuel)
    {
        _fuelGuageSlider.value = fuel;
    }
    
    public void UpdateLives(int lives)
    {
        lives = Mathf.Clamp(lives, 0, _livesSprites.Length - 1);
        _livesImage.sprite = _livesSprites[lives];
        if (lives == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while(_shouldBlink)
        {
            yield return new WaitForSeconds(.6f);
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
        }
    }
}
