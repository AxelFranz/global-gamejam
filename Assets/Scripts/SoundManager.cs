using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Keep the SoundManager when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {                
            Debug.LogWarning("SoundManager Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
