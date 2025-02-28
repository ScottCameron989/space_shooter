using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    
    [SerializeField]
    private TMP_Text _ammoText;

    [SerializeField]
    private Image _livesImage;
      
    [SerializeField]
    private TMP_Text _gameOverText;
    
    [SerializeField]
    private TMP_Text _restartText;
    
    [SerializeField]
    private TMP_Text _waveText;
    
    [SerializeField]
    private Slider _fuelGaugeSlider;
    
    [SerializeField]
    private Sprite[] _livesSprites;
    
    private bool _shouldBlink = true;

    void Start()
    {
        _scoreText.text = $"Score: 0";
        _gameOverText.gameObject.SetActive(false);
    }
    
    public void ShowWave(int waveNum)
    {
        StartCoroutine(WaveDisplay(waveNum));
    }
    
    IEnumerator WaveDisplay(int waveNum)
    {
        _waveText.text = $"Wave {waveNum}";
        _waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _waveText.gameObject.SetActive(false);
    }
    
    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
    
    public void UpdateFuelGauge(float fuel)
    {
        _fuelGaugeSlider.value = fuel;
    }
    
    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = $"Ammo: {ammo}";
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
