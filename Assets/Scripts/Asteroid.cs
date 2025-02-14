using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 30f;

    [SerializeField]
    private GameObject _explosionVfx;
    
    private WaveManager _waveManager;
    
    void Start()
    {
        if (_explosionVfx == null) Debug.LogError("Explosion VFX not set");
        
        _waveManager = GameObject.FindObjectOfType<WaveManager>();
        if (_waveManager == null) Debug.LogError("No WaveManager found");
    }
    void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.forward);
    }
    
    public void BlowUp()
    {
        Instantiate(_explosionVfx, transform.position, Quaternion.identity);
        _waveManager.StartFirstWave();
        Destroy(gameObject,0.2f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            BlowUp();
            Destroy(other.gameObject);
        }
    }
}
