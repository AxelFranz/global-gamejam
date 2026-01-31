using UnityEngine;
using UnityEngine.Rendering;

public class AmbianceManager : MonoBehaviour
{
    private Volume m_volume;
    [SerializeField] private VolumeProfile m_waterVolumeProfile;
    [SerializeField] private VolumeProfile m_fireVolumeProfile;
    [SerializeField] private VolumeProfile m_plantVolumeProfile;

    public static AmbianceManager Instance {  get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("Player Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
        m_volume = GetComponentInChildren<Volume>();
        Events.MaskChanged += onMaskChanged;
    }

    void onMaskChanged(MaskState newState)
    {
        switch(newState) {
            case MaskState.Fire:
                m_volume.profile = m_fireVolumeProfile;
                break;
            case MaskState.Water:
                m_volume.profile = m_waterVolumeProfile;
                break;
            case MaskState.Plant:
                m_volume.profile = m_plantVolumeProfile;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
