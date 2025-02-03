using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float _speed =4f;
    
    [SerializeField]
    private float _explodeInSeconds = 1.25f;
    [SerializeField]
    private GameObject _explosionVfx;

    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;
    private Coroutine _explodeRoutine;
    private CircleCollider2D _circleCollider2D;
    private BoxCollider2D _boxCollider2D;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (_spriteRenderer == null) Debug.LogError("No sprite renderer");
        if (_trailRenderer == null) Debug.LogError("No trail renderer");
        if (_circleCollider2D == null) Debug.LogError("No circle collider");
        if (_boxCollider2D == null) Debug.LogError("No box collider");
            
        _explodeRoutine = StartCoroutine(ExplodeTimer());
    }
    
    IEnumerator ExplodeTimer()
    {
        yield return new WaitForSeconds(_explodeInSeconds);
        Explode();
    }
    void Update()
    {
        CalculateMovement();
        ExplodeIfOutOfBounds();
    }
        
    private void CalculateMovement()
    {
        transform.Translate( _speed * Time.deltaTime * Vector3.up);
    }

    private void ExplodeIfOutOfBounds()
    {
        if (transform.position.y >= 9 || transform.position.y <= -6.5f)
        {
            Explode();
        }
    }
     public void OnTriggerEnter2D(Collider2D other)
     {
         if (other.CompareTag("Enemy") || other.CompareTag("Asteroid"))
             Explode();
     }
     
     public void Explode()
     {
         StopCoroutine(_explodeRoutine);
         enabled = false;
         _speed = 0f;
         _spriteRenderer.enabled = false;
         _trailRenderer.enabled = false;
         _boxCollider2D.enabled = false;
         Instantiate(_explosionVfx, transform.position, transform.rotation);
         List<Collider2D> Colliders = new List<Collider2D>();
         _circleCollider2D.OverlapCollider(new ContactFilter2D().NoFilter(), Colliders);
         foreach(Collider2D other in Colliders)
         {
             if (other.CompareTag("Enemy"))
             {
                 Enemy enemy = other.gameObject.GetComponent<Enemy>();
                 if (enemy)
                     enemy.BlowUp(true);
             }
             if (other.CompareTag("Asteroid"))
             {
                 Asteroid asteroid = other.GetComponent<Asteroid>();
                 if (asteroid)
                     asteroid.BlowUp();
             }
         }
         Destroy(gameObject, 2.6f);
         //Collider[] colliders = Physics2d.OverlapSphere(transform.position, _circleCollider2D.radius);
     }
}
