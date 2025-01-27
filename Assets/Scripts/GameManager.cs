using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver=false;
    
    void Update()
    {
        // Do I also need to stop/kill any running Coroutines?
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    
    public void GameOver()
    {
        _isGameOver = true;
    }
}
