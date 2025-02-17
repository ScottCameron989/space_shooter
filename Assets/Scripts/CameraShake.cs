using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _position = Vector3.zero;
    
    [SerializeField]
    private float _shakeAmount = 0.25f;
    
    [SerializeField]    
    private float _decreaseFactor = 4f;
    
    private Coroutine _shakeRoutine;
    private float _shake = 0f;
    
    public void Start()
    {
        _position = transform.localPosition;
    }
    
    public void StartShake()
    {
        _shake = 0.5f;
    }

    public void Update()
    {
        if (_shake > 0)
        {
            Vector3 shakePos = Random.insideUnitSphere * _shakeAmount;
            shakePos.z = _position.z;
            transform.localPosition = shakePos;
            _shake -= Time.deltaTime * _decreaseFactor;
        } else
        {
            _shake = 0f;
            transform.localPosition = _position;
        }
    }
}
