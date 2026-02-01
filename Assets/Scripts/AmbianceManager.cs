using UnityEngine;
using UnityEngine.Rendering;

public class AmbianceManager : MonoBehaviour
{
    private Volume m_volume;
    [SerializeField] private VolumeProfile m_waterVolumeProfile;
    [SerializeField] private VolumeProfile m_fireVolumeProfile;
    [SerializeField] private VolumeProfile m_plantVolumeProfile;
    private ParticleSystem m_waterParticleSystem;
    private ParticleSystem m_fireParticleSystem;
    private ParticleSystem m_plantParticleSystem;

    public static AmbianceManager Instance {  get; private set; }

    private void Awake()
    {
        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("Player Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
        m_volume = GetComponentInChildren<Volume>();
        m_waterParticleSystem = GetComponentInChildren<RainParticle>().GetComponent<ParticleSystem>();
        m_fireParticleSystem = GetComponentInChildren<FireParticle>().GetComponent<ParticleSystem>();
        m_plantParticleSystem = GetComponentInChildren<LeafParticle>().GetComponent<ParticleSystem>();
        Events.MaskChanged += onMaskChanged;
        m_waterParticleSystem.Stop();
        m_fireParticleSystem.Stop();
        m_plantParticleSystem.Stop();
    }
    private void Start()
    {
        m_waterParticleSystem.Stop();
        m_fireParticleSystem.Stop();
        m_plantParticleSystem.Stop();
    }

    void onMaskChanged(MaskState newState)
    {
        switch(newState) {
            case MaskState.Fire:
                m_volume.profile = m_fireVolumeProfile;
                m_fireParticleSystem.Play();
                m_plantParticleSystem.Stop();
                m_waterParticleSystem.Stop();
                break;
            case MaskState.Water:
                m_volume.profile = m_fireVolumeProfile;
                m_fireParticleSystem.Stop();
                m_plantParticleSystem.Stop();
                m_waterParticleSystem.Play();
                break;
            case MaskState.Plant:
                m_volume.profile = m_fireVolumeProfile;
                m_fireParticleSystem.Stop();
                m_plantParticleSystem.Play();
                m_waterParticleSystem.Stop();
                break;
            case MaskState.Unmasked:
                m_fireParticleSystem.Stop();
                m_plantParticleSystem.Stop();
                m_waterParticleSystem.Stop();
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
