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

    private GameObject m_maskEquipped;
    private GameObject m_maskBack;

    public static Player Instance { get; private set; }
    private Rigidbody m_rb;
    private InputAction m_moveAction;
    private InputAction m_switchMask;
    private List<MaskState> m_equippedMasks; // [0] = Equipped mask, [1] = mask on back

    private Vector3 lastInput;

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
        Events.MaskChanged += onMaskChange;
        Events.MenuClosed += onMenuClosed;
        Events.MenuOpened += onMenuOpened;

        m_rb = GetComponent<Rigidbody>();
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_switchMask = InputSystem.actions.FindAction("SwitchMask");
        m_switchMask.started += switchMask;

        m_maskEquipped = GetComponentInChildren<MaskEquipped>().gameObject;
        m_maskBack = GetComponentInChildren<MaskBack>().gameObject;

        setStartingMasks(MaskState.Unmasked, MaskState.Unmasked);
        lastInput = new Vector3(0,0,0);
    }

    private void onPlayerDetected(MaskState maskState)
    {
        Debug.Log("non");
        if (GameManager.Instance.MaskState != maskState) {
            die();
            Debug.Log("Merde, chopé");
        }
    }

    public void setStartingMasks(MaskState backMask, MaskState equippedMask)
    {
        m_equippedMasks = new List<MaskState>(){ equippedMask, backMask };
        Events.MaskChanged += onMaskChange;
        renderMasks();
        Debug.Log("Setting masks: " + equippedMask + ", " + backMask);
    }

    private void Start()
    {
        renderMasks();
        m_rb = GetComponent<Rigidbody>();
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_switchMask = InputSystem.actions.FindAction("SwitchMask");
        m_switchMask.started += switchMask;

        //Events.PlayerLoaded?.Invoke();
    }
    private void FixedUpdate()
    {
        Vector2 moveValue = m_moveAction.ReadValue<Vector2>();
        Vector3 velocity = new Vector3(moveValue.x, 0,moveValue.y);
        if(velocity != Vector3.zero)
        {
            Quaternion newRot = Quaternion.LookRotation(velocity);
            m_model.transform.rotation = Quaternion.Slerp(m_model.transform.rotation, newRot, 10*Time.fixedDeltaTime);

        }
        velocity *= m_speed;
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
            GameManager.Instance.ChangeMask(mask);
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


    private void onMenuOpened()
    {
        m_moveAction.Disable();
        m_switchMask.Disable();
    }

    private void onMenuClosed()
    {
        m_moveAction.Enable();
        m_switchMask.Enable();

    }

    private void onMaskChange(MaskState state)
    {
        m_equippedMasks[0] = state;
        renderMasks();
    }
    
    private void die()
    {
        Events.Die();
    }

}