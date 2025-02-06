using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private bool _isEnemyLaser;

    void Update()
    {
        CalculateMovement();
        DestroyIfOutOfBounds();
    }

    private void CalculateMovement()
    {
        switch (_isEnemyLaser)
        {
            case true:
                transform.Translate(_speed * Time.deltaTime * Vector3.down);
                break;
            case false:
                transform.Translate(_speed * Time.deltaTime * Vector3.up);
                break;
        }
    }

    private void DestroyIfOutOfBounds()
    {
        if (transform.position.y >= 9 || transform.position.y <= -6.5f)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CompareTag("EnemyLaser"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
}