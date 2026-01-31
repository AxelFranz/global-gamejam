using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject m_model;
    [SerializeField] private float m_speed = 10;
    [SerializeField] private GameObject m_fireMaskPrefab;
    [SerializeField] private GameObject m_waterMaskPrefab;
    [SerializeField] private GameObject m_plantMaskPrefab;
    [SerializeField] private MaskState m_startingEquippedMask;
    [SerializeField] private MaskState m_startingBackMask;

    private GameObject m_maskEquipped;
    private GameObject m_maskBack;

    public static Player Instance { get; private set; }
    private Rigidbody m_rb;
    private InputAction m_moveAction;
    private InputAction m_switchMask;
    private List<MaskState> m_equippedMasks; // [0] = Equipped mask, [1] = mask on back

    private void Awake()
    {
        // Keep the Player when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("Player Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
        Events.PlayerDetected += onPlayerDetected;
        m_maskEquipped = GetComponentInChildren<MaskEquipped>().gameObject;
        m_maskBack = GetComponentInChildren<MaskBack>().gameObject;
    }

    private void onPlayerDetected(MaskState maskState)
    {
        if (GameManager.Instance.MaskState != maskState) {
            Debug.Log("Merde, chopé");
        }
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_equippedMasks = new List<MaskState>(){ m_startingEquippedMask, m_startingBackMask };
        Events.MaskChanged += onMaskChange;
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_switchMask = InputSystem.actions.FindAction("SwitchMask");
        m_switchMask.started += switchMask;
        renderMasks();

        //Events.PlayerLoaded?.Invoke();
    }
    private void FixedUpdate()
    {
        Vector2 moveValue = m_moveAction.ReadValue<Vector2>();
        moveValue *= m_speed;
        Vector3 velocity = new Vector3(moveValue.x, 0,moveValue.y);
        m_rb.AddForce(velocity);
    }

    private void switchMask(InputAction.CallbackContext context)
    {
        m_equippedMasks.Reverse();
        GameManager.Instance.ChangeMask(m_equippedMasks[0]);
    }

    public void addMask(MaskState mask)
    {
        if(m_equippedMasks[0] == MaskState.Unmasked && m_equippedMasks[1] == MaskState.Unmasked) {
            m_equippedMasks[0] = mask;
            renderMasks();
            return;
        }
        // 1 because we change the mask on the back

        GameObject instantiatedPrefab = null;
        switch(m_equippedMasks[1])
        {
            case MaskState.Fire:
                instantiatedPrefab = Instantiate(m_fireMaskPrefab);
                break;
            case MaskState.Water:
                instantiatedPrefab = Instantiate(m_waterMaskPrefab);
                break;
            case MaskState.Plant:
                instantiatedPrefab = Instantiate(m_plantMaskPrefab);
                break;
        }
        if(instantiatedPrefab != null)
        {
            instantiatedPrefab.transform.position = gameObject.transform.position;
            instantiatedPrefab.GetComponent<DroppedMask>().activeMode = false;
        }

        m_equippedMasks[1] = mask;
        renderMasks();
    }

    private void renderMasks()
    {
        if (m_equippedMasks[0] == MaskState.Unmasked) m_maskEquipped.GetComponent<MeshRenderer>().enabled = false;
        else
        {
            MeshRenderer renderer = m_maskEquipped.GetComponent<MeshRenderer>();
            renderer.enabled = true;
            renderer.material.color = Utils.MaskStateToColor(m_equippedMasks[0]);
        }

        if (m_equippedMasks[1] == MaskState.Unmasked) m_maskBack.GetComponent<MeshRenderer>().enabled = false;
        else
        {
            MeshRenderer renderer = m_maskBack.GetComponent<MeshRenderer>();
            renderer.enabled = true;
            renderer.material.color = Utils.MaskStateToColor(m_equippedMasks[1]);
        }
    }


    private void onMaskChange(MaskState state)
    {
        m_equippedMasks[0] = state;
        renderMasks();

        Debug.Log(state);
    }

}