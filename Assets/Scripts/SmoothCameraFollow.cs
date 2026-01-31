using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private Vector3 m_rotation;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Events.PlayerLoaded += onPlayerLoaded;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = Player.Instance.transform.position + m_offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

    private void onPlayerLoaded() {
        transform.position = Player.Instance.transform.position + m_offset;
        transform.localEulerAngles = m_rotation;
    }

}