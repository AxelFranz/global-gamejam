using UnityEngine;
using UnityEngine.Rendering;

public class AmbianceManager : MonoBehaviour
{
    private Volume m_volume;
    [SerializeField] private VolumeProfile m_waterVolumeProfile;
    [SerializeField] private VolumeProfile m_fireVolumeProfile;
    [SerializeField] private VolumeProfile m_plantVolumeProfile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
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
