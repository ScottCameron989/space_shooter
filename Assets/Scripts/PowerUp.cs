using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private enum PowerUpType
    {
        TripleShot,
        Speed,
        Shield,
        Ammo,
        Health,
        Bomb
    }
    
    [SerializeField]
    private float _speed = 3;
    
    [SerializeField]
    private PowerUpType _powerUpType = PowerUpType.Speed;
    
    [SerializeField]
    private int _ammoGiven = 15;
    
    [SerializeField]
    private AudioClip _powerUpSound;
    
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y <= -5.6f) 
            Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.CompareTag("Player") )
        {
            Player player = other.GetComponent<Player>();
            if (player == null) Debug.LogError("Player not found");
            AudioSource.PlayClipAtPoint(_powerUpSound, player.transform.position);
            
            switch (_powerUpType)
            {
                case PowerUpType.TripleShot:
                    player?.EnableTripleShot();
                    break;
                case PowerUpType.Speed:
                    player?.EnableSpeedBoost();
                    break;
                case PowerUpType.Shield:
                    player?.ActivateShield();
                    break;
                case PowerUpType.Ammo:
                    player?.AddAmmo(_ammoGiven);
                    break;
                case PowerUpType.Health:
                    player?.AddLife();
                    break;
                case PowerUpType.Bomb:
                    player?.ActivateBomb();
                    break;
            }
          
            Destroy(gameObject);
        }
    }
}


