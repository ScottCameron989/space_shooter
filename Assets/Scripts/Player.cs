using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Inspector_fields
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _speedBoostMultiplier = 2f;
    
    [SerializeField]
    private float _thrustBoostMultiplier = 1.5f;
    
    [SerializeField]
    private float _fireRate = 0.5f;
    
    [SerializeField]
    private int _maxAmmo = 15;
    
    [SerializeField]
    private int _lives = 3;
    
    [SerializeField]
    [Range(0f,1f)]
    private float _fuelPercent = 1f;
    
    [SerializeField]
    private float _fuelRate = 0.2f;
    
    [SerializeField]
    private float _fuelRegenRate = 0.2f;
    
    [SerializeField]
    private GameObject _laserPrefab;
    
    [SerializeField]
    private GameObject _tripleShotPrefab;
    
    [SerializeField]
    private GameObject _shield;
    
    [SerializeField]
    private int _maxShieldStrength = 3;
    
    [SerializeField]
    private GameObject _leftEngineDamage;
    
    [SerializeField]
    private GameObject _rightEngineDamage;
    
    [SerializeField]
    private AudioClip _fireSound;
    
    [SerializeField]
    private GameObject _explosionVfx;
    
    [SerializeField]
    private bool _isTripleShotActive = false;
    
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    
    [SerializeField]
    private bool _isShieldActive = false;
    
    [SerializeField]
    private int _score = 0;
    #endregion
    
    private float _canFire = -1f;
    private int _availableAmmo;
    private bool _isBoosting = false;
    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private Coroutine _tripleShotRoutine;
    private Coroutine _speedBoostRoutine;
    
    private int _lastEngineDamage;
    private AudioSource _audioSource;
    private Transform _laserOffset;
    private Vector3 _moveDirection;
    private int _shieldStrength;
    private readonly Vector3 _maxShieldScale = new Vector3(2f,2f,2f);
    private readonly Vector3 _midShieldScale = new Vector3(1.5f,1.5f,1.5f);
    private readonly Vector3 _minShieldScale = new Vector3(1f,1f,1f);
    
    void Start()
    {
        transform.position = new Vector3(0f,-3.65f,0f);
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _uiManager = GameObject.FindObjectOfType<UIManager>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        _laserOffset = transform.Find("Laser_Offset");    
        
        if (_spawnManager == null) Debug.LogError("No SpawnManager found");
        if (_uiManager == null) Debug.LogError("UI manager not found");
        if (_gameManager == null) Debug.LogError("Game manager not found");
        
        if (_audioSource == null) Debug.LogError("AudioSource not found on Player");
        if (_explosionVfx == null) Debug.LogError("Explosion VFX not Set on Player");
        if (_shield == null) Debug.LogError("Shield not Set on Player");
        if (_laserOffset == null) Debug.LogError("Laser_Offset not found on Player");
        
        _shieldStrength = _maxShieldStrength;
        _availableAmmo = _maxAmmo;
        _uiManager.UpdateFuelGuage(_fuelPercent);
        _uiManager.UpdateAmmo(_availableAmmo);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _availableAmmo > 0) FireLaser();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ThrusterBoost();
        }
        else
        {
            RefuelThrusters();
        }
        CalculateMovement();
    }
    
    private void FireLaser()
    {
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive)
            {
                _availableAmmo -= 3;
                if(_availableAmmo <= 0) _availableAmmo = 0;  
                Instantiate(_tripleShotPrefab, _laserOffset.position, Quaternion.identity);
            }
            else
            {
                _availableAmmo -= 1;
                Instantiate(_laserPrefab, _laserOffset.position, Quaternion.identity);
            }
            _uiManager.UpdateAmmo(_availableAmmo);
            _audioSource.PlayOneShot(_fireSound);
    }
    
     private void ThrusterBoost()
     {
         if (_isBoosting == false)
         {
             _isBoosting = true;
             _speed *= _thrustBoostMultiplier;
         }
         if (_fuelPercent >= 0f){
             _fuelPercent -= _fuelRate * Time.deltaTime;
             _uiManager.UpdateFuelGuage(_fuelPercent);
         }
         if (_fuelPercent <= 0f && _isBoosting == true)
         {
             _isBoosting = false;
             _speed /= _thrustBoostMultiplier;
         }
     }
 
     private void RefuelThrusters()
     {
         if (_isBoosting == true)
         {
             _isBoosting = false;
             _speed /= _thrustBoostMultiplier;
         }
 
         if (_fuelPercent <= 1f) {
             _fuelPercent += _fuelRegenRate * Time.deltaTime;
             _uiManager.UpdateFuelGuage(_fuelPercent);
         }
     }
     
    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _moveDirection.Set(horizontalInput,verticalInput,0);
        transform.Translate( _speed * Time.deltaTime * _moveDirection);
        
        float wrappedX = WrapValue(transform.position.x, -11.3f, 11.3f);
        
        transform.position = new Vector3(wrappedX,Mathf.Clamp(transform.position.y,-3.65f,0f),0);
    }
    
    /// <summary>
    /// This will wrap a value between min and max.
    /// if we hit max it will wrap back to min and
    /// vice versa.
    /// </summary>
    /// <param name="val">The float value we want to wrap</param>
    /// <param name="min">The minimum float value</param>
    /// <param name="max">the maximum float value</param>
    /// <returns>value within min - max inclusive</returns>
    private float WrapValue(float val, float min, float max)
    {
        return (val >= max ? min : 0) +
               (val < min ? max : 0) +
               (val >= min && val < max ? val : 0);
    }
    
    public void Damage()
    {
        if (_isShieldActive)
        {
            DamageShield();
            return;
        }
        
        _lives--;
        _uiManager.UpdateLives(_lives);
        ToggleRandomEngineDamage();
        if (_lives < 1 )
        {
            Instantiate(_explosionVfx, transform.position, Quaternion.identity);
            _spawnManager.StopSpawning(true);
            _gameManager.GameOver();
            Destroy(gameObject);
        }
    }

    private void DamageShield()
    {
        _shieldStrength -=  1;
        ShowShieldDamage();
        if (_shieldStrength <= 0 ) DeactivateShield();
    }

    private void ShowShieldDamage()
    {
        switch(_shieldStrength) {
            case 2:_shield.transform.localScale = _midShieldScale; break;
            case 1:_shield.transform.localScale = _minShieldScale; break;
        }
    }

    /// <summary>
    /// Toggles A Random Engine Damage object on first hit
    /// on second hit it will toggle the other engine. 
    /// </summary>
    private void ToggleRandomEngineDamage()
    {
        switch (Random.Range(0, 2))
        {
            case 0: 
                if (!_leftEngineDamage.activeSelf)
                {
                    _leftEngineDamage.SetActive(true);
                }
                else
                {
                    _rightEngineDamage.SetActive(true);
                }
                break;

            case 1: 
                if (!_rightEngineDamage.activeSelf)
                {
                    _rightEngineDamage.SetActive(true);
                }
                else
                {
                    _leftEngineDamage.SetActive(true);
                }
                break;
        }
    }
    
    public void EnableTripleShot()
    {
        if (_isTripleShotActive && _tripleShotRoutine != null)
        {
            StopCoroutine(_tripleShotRoutine);
            _tripleShotRoutine = null;
        }
            
        _isTripleShotActive = true;
        _tripleShotRoutine = StartCoroutine(TripleShotDisableTimer());
    }

    public void EnableSpeedBoost()
    {
        if (_isSpeedBoostActive && _speedBoostRoutine != null)
        {
            StopCoroutine(_speedBoostRoutine);
            _speed /= _speedBoostMultiplier;
            _speedBoostRoutine = null;
        }
        _isSpeedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        _speedBoostRoutine = StartCoroutine(SpeedBoostDisableTimer()); 
    }
    
    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldStrength = _maxShieldStrength;
        _shield.transform.localScale = _maxShieldScale;
        _shield.SetActive(true);
    }
    
    private void DeactivateShield()
    {
        _isShieldActive = false;
        _shield.SetActive(false);
    }
    
    public void AddAmmo(int ammo)
    {
        _availableAmmo += ammo;
        _availableAmmo = Mathf.Clamp(_availableAmmo, 0, _maxAmmo);
        _uiManager.UpdateAmmo(_availableAmmo);
    }
    IEnumerator TripleShotDisableTimer()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }
    
    IEnumerator SpeedBoostDisableTimer()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
        _speed /= _speedBoostMultiplier;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
