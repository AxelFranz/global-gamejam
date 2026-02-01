using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MaskState m_maskState;
    private Renderer[] m_renderers;
    private BoxCollider m_boxCollider;
    private Animator m_animator;
    private bool m_here = true;
    
    void Awake()
    {
        Events.MaskChanged += onMaskChanged;
        Events.ObstacleOffFinished += onObstacleOffFinished;
        Events.ObstacleOnFinished += onObstacleOnFinished;
        m_boxCollider = GetComponent<BoxCollider>();
        //m_meshRenderer = GetComponent<MeshRenderer>();
        m_renderers = GetComponentsInChildren<Renderer>();
        m_animator = GetComponent<Animator>();
    }

    void onMaskChanged(MaskState newState) {
        if (m_maskState == newState) {
            disappear();
        } else {
            if (!m_here) pop();
        }
    }

    void pop()
    {
        if (m_animator != null) {
            m_animator.SetTrigger("ObstacleOn");
        }
        foreach (Renderer renderer in m_renderers) {
            renderer.enabled = true;
        }
        m_boxCollider.enabled = true;
        m_here = true;
    }

    void disappear()
    {
        m_boxCollider.enabled = false;
        m_animator.SetTrigger("ObstacleOff");
        m_here = false;
    }

    void onObstacleOffFinished(MaskState state)
    {
        if (state != m_maskState) return;
        foreach (Renderer renderer in m_renderers) {
            renderer.enabled = false;
        }
    }

    void onObstacleOnFinished(MaskState state)
    {

    }
}
