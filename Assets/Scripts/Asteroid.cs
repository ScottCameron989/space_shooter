using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 30f;

    [SerializeField]
    private GameObject _explosionVfx;
    
    private SpawnManager _spawnManager;
    
    void Start()
    {
        if (_explosionVfx == null) Debug.LogError("Explosion VFX not set");
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("No SpawnManager found");
    }
    void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.forward);
    }
    
    public void BlowUp()
    {
        Instantiate(_explosionVfx, transform.position, Quaternion.identity);
        _spawnManager.StartSpawn();
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
