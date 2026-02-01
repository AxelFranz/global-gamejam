using UnityEngine;
using UnityEngine.Rendering;

public class AmbianceManager : MonoBehaviour
{
    private Volume m_volume;
    private ParticleSystem m_particleSystem;
    [SerializeField] private VolumeProfile m_waterVolumeProfile;
    [SerializeField] private VolumeProfile m_fireVolumeProfile;
    [SerializeField] private VolumeProfile m_plantVolumeProfile;
    [SerializeField] private ParticleSystem m_waterParticleSystem;
    [SerializeField] private ParticleSystem m_fireParticleSystem;
    [SerializeField] private ParticleSystem m_plantParticleSystem;

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
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
        Events.MaskChanged += onMaskChanged;
    }

    void onMaskChanged(MaskState newState)
    {
        m_particleSystem.Stop();
        switch(newState) {
            case MaskState.Fire:
                m_volume.profile = m_fireVolumeProfile;
                m_particleSystem = m_fireParticleSystem;
                break;
            case MaskState.Water:
                m_volume.profile = m_waterVolumeProfile;
                m_particleSystem = m_waterParticleSystem;
                break;
            case MaskState.Plant:
                m_volume.profile = m_plantVolumeProfile;
                m_particleSystem = m_plantParticleSystem;
                break;
        }
        m_particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
