using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject m_model;
    [SerializeField] private float m_speed = 10;
    [SerializeField] private GameObject m_fireDroppedMaskPrefab;
    [SerializeField] private GameObject m_waterDroppedMaskPrefab;
    [SerializeField] private GameObject m_plantDroppedMaskPrefab;

    [SerializeField] private GameObject m_fireMaskPrefab;
    [SerializeField] private GameObject m_waterMaskPrefab;
    [SerializeField] private GameObject m_plantMaskPrefab;

    [SerializeField] private Animator m_anim;


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
            m_anim.SetBool("isMoving", true);
            Quaternion newRot = Quaternion.LookRotation(velocity);
            m_model.transform.rotation = Quaternion.Slerp(m_model.transform.rotation, newRot, 10*Time.fixedDeltaTime);

        } else
        {
            m_anim.SetBool("isMoving", false);
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

        Debug.Log(m_fireDroppedMaskPrefab);
        Debug.Log(m_waterDroppedMaskPrefab);
        Debug.Log(m_plantDroppedMaskPrefab);
        GameObject instantiatedPrefab = null;
        switch(m_equippedMasks[1])
        {
            case MaskState.Fire:
                instantiatedPrefab = Instantiate(m_fireDroppedMaskPrefab);
                break;
            case MaskState.Water:
                instantiatedPrefab = Instantiate(m_waterDroppedMaskPrefab);
                break;
            case MaskState.Plant:
                instantiatedPrefab = Instantiate(m_plantDroppedMaskPrefab);
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

    private GameObject getObject(MaskState state)
    {
        switch(state)
        {
            case MaskState.Fire:
                return m_fireMaskPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            case MaskState.Water:
                return m_waterMaskPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            case MaskState.Plant:
                return m_plantMaskPrefab.GetComponentInChildren<MeshRenderer>().gameObject;
            case MaskState.Unmasked:
                return null;
            default:
                return null;
        }

    }

    private void renderMasks()
    {
        if (m_equippedMasks[0] == MaskState.Unmasked) m_maskEquipped.GetComponent<MeshRenderer>().enabled = false;
        else
        {
            MeshRenderer renderer = m_maskEquipped.GetComponent<MeshRenderer>();
            renderer.enabled = true;
            GameObject mask = getObject(m_equippedMasks[0]);
            renderer.material = mask.GetComponent<MeshRenderer>().sharedMaterial;
            MeshFilter mesh = m_maskEquipped.GetComponent<MeshFilter>();
            mesh.mesh = mask.GetComponent<MeshFilter>().sharedMesh;
        }

        if (m_equippedMasks[1] == MaskState.Unmasked) m_maskBack.GetComponent<MeshRenderer>().enabled = false;
        else
        {
            MeshRenderer renderer = m_maskBack.GetComponent<MeshRenderer>();
            renderer.enabled = true;
            GameObject mask = getObject(m_equippedMasks[1]);
            renderer.material = mask.GetComponent<MeshRenderer>().sharedMaterial;
            MeshFilter mesh = m_maskBack.GetComponent<MeshFilter>();
            mesh.mesh = mask.GetComponent<MeshFilter>().sharedMesh;
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