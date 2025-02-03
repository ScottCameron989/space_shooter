﻿using System.Linq;
using UnityEngine;

    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 3f;
        
        [SerializeField]
        private int _points = 10;
        
        [SerializeField]
        private float _fireRate = 3.0f;
        
        [SerializeField]
        private AudioClip _explodeSound;
        
        [SerializeField]
        private GameObject _enemyLaser;
        
        private Player _player;
        private Animator _animator;
        private float _deathAnimDelay = 0f;
        private AudioSource _audioSource;
        private bool _isDead = false;
        private float _canFire = -1;     
        private Transform _laserOffset;
        private Collider2D _collider;
        
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            if (_player == null) Debug.LogError("Player not found");
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null) Debug.LogError("AudioSource not found on Enemy");
            _laserOffset = transform.Find("Laser_Offset");
            if (_laserOffset == null) Debug.LogError("Laser Offset not found on Enemy");
            _animator = GetComponent<Animator>();
            if (_animator == null) Debug.LogError("Animator not found");
            _collider = GetComponent<Collider2D>();
            
            var deathAnim =  _animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == "EnemyDestroyed_anim");
            if (deathAnim != null)
            {
                _deathAnimDelay = deathAnim.length;
            }
        }

        void Update()
        {
            CalculateMovement();
            
            if (Time.time > _canFire && !_isDead)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                Instantiate(_enemyLaser, _laserOffset.position, Quaternion.identity);
            }
        }

        private void CalculateMovement()
        {
            transform.Translate(  (_speed * Time.deltaTime) * Vector3.down);
            if (transform.position.y <= -5.6f) 
                transform.position = new Vector3(Random.Range(-9f,9f), 8.5f, 0);
        }

        
        public void BlowUp(bool givePoints)
        {
            _isDead = true;
            _speed=0f;
            _animator.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            _audioSource.PlayOneShot(_explodeSound);
            Destroy(gameObject,_deathAnimDelay);
            if (givePoints) _player.AddScore(_points);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ( other.CompareTag("Player") )
            {
                _player.Damage();
                BlowUp(false);
            }
            
            if (other.CompareTag("Laser"))
            {
                BlowUp(true);
                Destroy(other.gameObject);
            }
        }
        
    }
