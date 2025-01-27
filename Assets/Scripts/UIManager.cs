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
    private Sprite[] _livesSprites;
  
    // IS it good to have these here even though we are just reloading the scene?
    // Do coroutines get killed when loading a new scene?
    // are their resources freed/released
    Coroutine _blinkCoroutine;
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
    
    public void UpdateLives(int lives)
    {
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
        _blinkCoroutine = StartCoroutine(BlinkRoutine());
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
