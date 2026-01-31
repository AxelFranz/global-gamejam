using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MaskState m_maskState;
    private MeshRenderer m_meshRenderer;
    private BoxCollider m_boxCollider;
    
    void Awake()
    {
        Events.MaskChanged += onMaskChanged;
        m_boxCollider = GetComponent<BoxCollider>();
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    void onMaskChanged(MaskState newState) {
        if ((m_maskState == MaskState.Fire && newState == MaskState.Water) ||
            (m_maskState == MaskState.Water && newState == MaskState.Plant) ||
            (m_maskState == MaskState.Plant && newState == MaskState.Fire)) {
            disappear();
        } else {
            pop();
        }
    }

    void pop()
    {
        m_meshRenderer.enabled = true;
        m_boxCollider.enabled = true;
    }

    void disappear()
    {
        m_meshRenderer.enabled = false;
        m_boxCollider.enabled = false;
    }
}
