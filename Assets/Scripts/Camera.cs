using UnityEngine;

public class Camera : MonoBehaviour
{
    public static Camera Instance { get; private set; }
    private void Awake()
    {
        // Keep the camera when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {                
            Debug.LogWarning("Camera Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
    }
}
